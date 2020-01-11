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
    { TRUE, FALSE, UNSPECIFIED, NA }
    public class NodeGeneric
    {
        private const int MAX_PARENTS = 3;
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
        public double[,] probabilities;

        public NodeGeneric()
        {
            ListOfParents = new List<NodeGeneric>();
            ProbTrue = new List<TextBox>();
            ProbFalse = new List<TextBox>(ProbTrue.Count);

            Status = Status.NA;

            probabilities = new double[(int)Math.Pow(2, MAX_PARENTS), 2];
        }

        public void SetProbabilities()
        {
            const string basicPath = @"..\..\..\probabilities\";
            try
            {
                var fileName = Path.Combine(basicPath, Name.ToLower() + ".txt");
                
                CultureInfo ci = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
                var fileStream = File.OpenRead(fileName);
                var streamReader = new StreamReader(fileStream, Encoding.UTF8);
                var content = streamReader.ReadToEnd();
                var values = content.Split(' ', '\n');
                for (var i = 0; i < values.Length; i++)
                {
                    probabilities[i, 0] = Double.Parse(values[i]);
                    probabilities[i, 1] = 1 - Double.Parse(values[i]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void SetProbFalse()
        {
            for (int i = 0; i < ProbTrue.Count; i++)
            {
                var x = Double.Parse(ProbTrue[i].Text);
                ProbFalse[i].Text = (1.0 - x).ToString();
            }
        }

        /// <summary>
        /// Compute a probability for a node.
        /// </summary>
        /// <param name="node"></param>
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
                    case Status.TRUE:
                        return this.probabilities[0, 0];
                    case Status.FALSE:
                        return this.probabilities[0, 1];
                }
            }
            else
            {
                // The line in matrix is determined by a combination between TRUE/FALSE values of the parents.
                // The column is determined by TRUE/FALSE status of current node.
                // In case of TRUE, the column is 0, otherwise 1.

                // By default, the variable is set to TRUE.
                int column = 0;
                if (this.Status == Status.FALSE)
                {
                    column = 1;
                }

                bool[] correspondingValues = new bool[this.ListOfParents.Count];
                for (int i = 0; i < this.ListOfParents.Count; ++i)
                {
                    NodeGeneric parent = this.ListOfParents.ElementAt(i);
                    // TODO: replace this with normal logic, and after that, negate the result.
                    if (parent.Status == Status.FALSE)
                    {
                        correspondingValues[i] = true;
                    }
                    else if (parent.Status == Status.TRUE)
                    {
                        correspondingValues[i] = false;
                    }
                }

                int val = 0;
                for (int i=0; i<this.ListOfParents.Count; ++i)
                {
                    val = (val << 1) | toDigit(correspondingValues[i]);
                }
                return this.probabilities[val, column];
            }
            // TODO: probability 0?
            return -1;
        }

        private int toDigit(Boolean b) {
            return b ? 1 : 0;

        }

    }
}