using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataModel
{
    public enum Status
    {
        TRUE,
        FALSE,
        UNSPECIFIED,
        NA
    }

    public class NodeGeneric
    {
        private const int MAX_PARENTS = 3;

        public List<NodeGeneric> ListOfParents;
        public string Name { get; set; }
        public List<TextBox> ProbTrue;
        public List<TextBox> ProbFalse;

        public Status NodeStatus;


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

            NodeStatus = Status.NA;

            probabilities = new double[(int)Math.Pow(2, MAX_PARENTS), 2];
        }

        public void SetProbabilitis(string textFileName)
        {
            try
            {

                var fileStream = File.OpenRead(textFileName);
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
        public static double ComputeBayes(NodeGeneric node)
        {
            // The probability of a node is:
            // the probability of that node if has no parents
            // the probability conditioned by the parents if it has any
            if (node.ListOfParents.Count == 0)
            {
                switch (node.NodeStatus)
                {
                    case Status.TRUE:
                        return node.probabilities[0, 0];
                    case Status.FALSE:
                        return node.probabilities[0, 1];
                }
            }
            else
            {
                // The line in matrix is determined by a combination between TRUE/FALSE values of the parents.
                // The column is determined by TRUE/FALSE status of current node.
                // In case of TRUE, the column is 0, otherwise 1.

                // By default, the variable is set to TRUE.
                int column = 0;
                if (node.NodeStatus == Status.FALSE)
                {
                    column = 1;
                }

                bool[] correspondingValues = new bool[node.ListOfParents.Count];
                for (int i = 0; i < node.ListOfParents.Count; ++i)
                {
                    NodeGeneric parent = node.ListOfParents.ElementAt(i);
                    // TODO: replace this with normal logic, and after that, negate the result.
                    if (parent.NodeStatus == Status.FALSE)
                    {
                        correspondingValues[i] = true;
                    }
                    else if (parent.NodeStatus == Status.TRUE)
                    {
                        correspondingValues[i] = false;
                    }
                }

                var bitArray = new BitArray(correspondingValues);
                var array = new int[1];
                bitArray.CopyTo(array, 0);
                return node.probabilities[array[0], column];
            }

            return -1;
        }
    }
}