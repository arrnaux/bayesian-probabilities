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

        //
        //public int NoOfParents { get; set; }

        public List<NodeGeneric> ListOfParents;
        //nume nod
        public string Name { get; set; }
        public List<TextBox> ProbTrue;
        public List<TextBox> ProbFalse;
        //valoare observata bool sau index
        public List<NodeGeneric> ListOfChildren;
        public IsUsed NodeStatus;
        public bool IsObservable { get; set; }

        // TODO: decide when this needs to be populated & allocate memory for it.
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
