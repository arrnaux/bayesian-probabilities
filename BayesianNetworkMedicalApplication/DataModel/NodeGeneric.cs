using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataModel
{
    public enum IsUsed
    {
        TRUE,
        FALSE,
        UNSPECIFIED,
        NA
    }
    public class NodeGeneric
    {
        private const int MAX_PARENTS = 10;

        public List<NodeGeneric> ListOfParents;
        public string Name { get; set; }
        public List<TextBox> ProbTrue;
        public List<TextBox> ProbFalse;
        //valoare observata bool sau index

            // NO LONGER USED
        public List<NodeGeneric> ListOfChildren;
        public IsUsed NodeStatus;
        public bool IsObservable { get; set; }

        // TODO: decide when this needs to be populated & allocate memory for it.
        // Also, this can replace the list for ProbTrue/ProbFalse
        // Should be allocated with 2*nX 2 linesXcolumns, where n = noParents.
        /// <summary>
        /// The structure is:
        /// FIRST PARENT    SECOND PARENT   NODE YES    NODE NO
        /// YES             YES             value       value
        /// YES             NO             value       value
        /// NO             YES             value       value
        /// NO             NO             value       value
        /// </summary>
        public double[,] probabilities=null;

        public NodeGeneric()
        {
            ListOfParents = new List<NodeGeneric>();
            ListOfChildren = new List<NodeGeneric>();
            ProbTrue = new List<TextBox>();

            ProbFalse = new List<TextBox>(ProbTrue.Count);
            IsObservable = false;
            NodeStatus = IsUsed.NA;
            probabilities = new double[MAX_PARENTS, MAX_PARENTS];
        }

        public void SetProbFalse()
        {
            for (int i = 0; i < ProbTrue.Count; i++)
            {
                var x = Double.Parse(ProbTrue[i].Text);
                ProbFalse[i].Text = (1.0 - x).ToString();
            }
        }

        public static double ComputeBayes(NodeGeneric node)
        {
            // The probability of a node is:
            // the probability of that node if has no parents
            // the probability conditionated by the parents if it has any
            if (node.ListOfParents.Count == 0)
            {
                if (node.NodeStatus == IsUsed.TRUE)
                {
                    return node.probabilities[0, 0];
                } else if (node.NodeStatus == IsUsed.FALSE)
                {
                    return node.probabilities[0, 1];
                }
           }


            if (node.ListOfParents.Count==0)
            {
                return double.Parse(node.ProbTrue.FirstOrDefault().Text);
            }
            else
            {
                var sum = 0.0;
                foreach (var parents in node.ListOfParents)
                {
                    switch (parents.NodeStatus)
                    {
                        case IsUsed.TRUE:
                            break;
                        case IsUsed.FALSE:
                            break;
                        case IsUsed.UNSPECIFIED:
                            break;
                        case IsUsed.NA:
                            break;
                    }
                    //do some magic
                }
            }
            return 0;
        }

    }
}
