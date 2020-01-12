using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DataModel
{
    public enum Status
    {
        True,
        False,
        Unspecified,
        Na
    }

    public class NodeGeneric : IEquatable<NodeGeneric>
    {
        private const int MaxParents = 3;
        public List<NodeGeneric> ListOfParents;

        public string Name { get; set; }
        public List<TextBox> ProbTrue;
        public List<TextBox> ProbFalse;

        public Status Status { get; set; }

        /// <summary>
        /// The structure is:
        /// FIRST PARENT    SECOND PARENT   NODE YES    NODE NO
        /// YES             YES             value       value
        /// YES             NO              value       value
        /// NO              YES             value       value
        /// NO              NO              value       value
        /// </summary>
        private double[,] Probabilities;

        public NodeGeneric()
        {
            ListOfParents = new List<NodeGeneric>();
            ProbTrue = new List<TextBox>();
            ProbFalse = new List<TextBox>(ProbTrue.Count);

            Status = Status.Na;

            Probabilities = new double[(int) Math.Pow(2, MaxParents), 2];
        }

        public void SetTextBoxValues()
        {
            for (var i = 0; i < ProbTrue.Count; i++)

            {
                ProbTrue[i].Text = Probabilities[i, 0].ToString();
                ProbFalse[i].Text = Probabilities[i, 1].ToString();
            }
        }

        public void SetMatrixValuesFormTextBox()
        {
            for (var i = 0; i < ProbTrue.Count; i++)
            {
                Probabilities[i, 0] = double.Parse(ProbTrue[i].Text);
                Probabilities[i, 1] = double.Parse(ProbFalse[i].Text);
            }
        }

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
                var fileStream = File.OpenRead(fileName);
                var streamReader = new StreamReader(fileStream, Encoding.UTF8);
                var content = streamReader.ReadToEnd();
                var values = content.Split(' ', '\n');
                for (var i = 0; i < values.Length; i++)
                {
                    Probabilities[i, 0] = double.Parse(values[i]);
                    Probabilities[i, 1] = 1 - double.Parse(values[i]);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                throw;
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
            if (this.ListOfParents.Count == 0)
            {
                switch (this.Status)
                {
                    case Status.True:
                        return this.Probabilities[0, 0];
                    case Status.False:
                        return this.Probabilities[0, 1];
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
                    NodeGeneric parent = this.ListOfParents.ElementAt(i);
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

                return this.Probabilities[val, column];
            }

            return -1;
        }

        private static int ToDigit(bool b)
        {
            return b ? 1 : 0;
        }

        public bool Equals(NodeGeneric other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(ListOfParents, other.ListOfParents) && Equals(ProbTrue, other.ProbTrue) &&
                   Equals(ProbFalse, other.ProbFalse) && Equals(Probabilities, other.Probabilities) &&
                   Name == other.Name && Status == other.Status;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NodeGeneric) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (ListOfParents != null ? ListOfParents.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ProbTrue != null ? ProbTrue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ProbFalse != null ? ProbFalse.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Probabilities != null ? Probabilities.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) Status;
                return hashCode;
            }
        }
    }
}