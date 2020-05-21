using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DataModel
{
    static class Constants
    {
        public const int MAX_NO_PARENTS = 3;
    }

    public enum Status
    {
        True,
        False,
        Unspecified,
        Na
    }


    //TODO: refactor varible names (c# convecntions)
    public class GenericNode : IEquatable<GenericNode>
    {
        public List<GenericNode> ListOfParents { get; set; }

        public string Name { get; set; }
        public List<TextBox> TrueProbabilityTextBoxes { get; set; }
        public List<TextBox> FalseProbabilityTextBoxes { get; set; }

        public Status Status { get; set; }

        /// <summary>
        /// The structure is:
        /// FIRST PARENT    SECOND PARENT   NODE YES    NODE NO
        /// YES             YES             value       value
        /// YES             NO              value       value
        /// NO              YES             value       value
        /// NO              NO              value       value
        /// </summary>
        private double[,] _probabilities;

        public GenericNode(string name)
        {
            Name = name;
            ListOfParents = new List<GenericNode>();
            TrueProbabilityTextBoxes = new List<TextBox>();
            FalseProbabilityTextBoxes = new List<TextBox>(TrueProbabilityTextBoxes.Count);
            Status = Status.Na;
            _probabilities = new double[(int)Math.Pow(2, Constants.MAX_NO_PARENTS), 2];
        }

        public void LoadProabilitiesFromMatrix()
        {
            for (var i = 0; i < TrueProbabilityTextBoxes.Count; i++)
            {
                TrueProbabilityTextBoxes[i].Text = _probabilities[i, 0].ToString();
                FalseProbabilityTextBoxes[i].Text = _probabilities[i, 1].ToString();
            }
        }

        public void LoadProbabilitiesFromGUI()
        {
            for (var i = 0; i < TrueProbabilityTextBoxes.Count; i++)
            {
                _probabilities[i, 0] = double.Parse(TrueProbabilityTextBoxes[i].Text);
                _probabilities[i, 1] = double.Parse(FalseProbabilityTextBoxes[i].Text);
            }
        }

        // TODO: replace its parameter with a value from DataSource
        public void SetProbabilities(bool labValue)
        {
            const string basicPath = @"../../../probabilities/";
            try
            {
                var fileName = (labValue)
                    ? Path.Combine(basicPath, "laborator", Name.ToLower() + ".txt")
                    : Path.Combine(basicPath, "date", Name.ToLower() + ".txt");

                CultureInfo ci = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;

                LoadProbabilitiesFromFile(fileName);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                throw;
            }
        }

        private void LoadProbabilitiesFromFile(string fileName)
        {
            var fileStream = File.OpenRead(fileName);
            var streamReader = new StreamReader(fileStream, Encoding.UTF8);
            var content = streamReader.ReadToEnd();
            var values = content.Split(' ', '\n');
            for (var i = 0; i < values.Length; i++)
            {
                //TODO: add check values between [0,1]
                _probabilities[i, 0] = double.Parse(values[i]);
                _probabilities[i, 1] = 1 - double.Parse(values[i]);
            }
        }

        /// <summary>
        /// Compute a probability for a node.
        /// </summary>
        /// <returns>A probability of a node with a value (T/F), considering its parents and their values (T/F)</returns>
        public double ComputeProbabilityConsideringParents()
        {
            // The probability of a node is:
            // the probability of that node if has no parents
            // the probability conditioned by the parents if it has any
            if (0 == ListOfParents.Count)
            {
                switch (this.Status)
                {
                    case Status.True:
                        return this._probabilities[0, 0];
                    case Status.False:
                        return this._probabilities[0, 1];
                }
            }
            else
            {
                // The line in matrix is determined by a combination between TRUE/FALSE values of the parents.
                // The column is determined by TRUE/FALSE status of current node.
                // In case of TRUE, the column is 0, otherwise 1.

                // By default, the variable is set to TRUE.
                int column = 0;
                if (this.Status == Status.False)
                {
                    column = 1;
                }

                bool[] correspondingValues = new bool[this.ListOfParents.Count];
                for (int i = 0; i < this.ListOfParents.Count; ++i)
                {
                    GenericNode parent = this.ListOfParents.ElementAt(i);
                    if (parent.Status == Status.False)
                    {
                        correspondingValues[i] = false;
                    }
                    else if (parent.Status == Status.True)
                    {
                        correspondingValues[i] = true;
                    }
                }

                for (int i = 0; i < ListOfParents.Count; i++)
                {
                    correspondingValues[i] = correspondingValues[i] ^ true;
                }

                int val = 0;
                for (int i = 0; i < this.ListOfParents.Count; ++i)
                {
                    val = (val << 1) | ToDigit(correspondingValues[i]);
                }

                return this._probabilities[val, column];
            }

            return -1;
        }

        private static int ToDigit(bool b)
        {
            return b ? 1 : 0;
        }

        public bool Equals(GenericNode other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(ListOfParents, other.ListOfParents) && Equals(TrueProbabilityTextBoxes, other.TrueProbabilityTextBoxes) &&
                   Equals(FalseProbabilityTextBoxes, other.FalseProbabilityTextBoxes) && Equals(_probabilities, other._probabilities) &&
                   Name == other.Name && Status == other.Status;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GenericNode)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (ListOfParents != null ? ListOfParents.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (TrueProbabilityTextBoxes != null ? TrueProbabilityTextBoxes.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FalseProbabilityTextBoxes != null ? FalseProbabilityTextBoxes.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_probabilities != null ? _probabilities.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)Status;
                return hashCode;
            }
        }
    }
}