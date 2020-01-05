using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataModel
{
    public class NodeGeneric
    {
        //
        //public int NoOfParents { get; set; }

        public List<NodeGeneric> ListOfParents;
        //nume nod
        public string Name { get; set; }
        public List<TextBox> ProbTrue;
        public List<TextBox> ProbFalse;
        //valoare observata bool sau index
        public List<NodeGeneric> ListOfChildren;
        public bool IsObservable { get; set; }

        public NodeGeneric()
        {
            ListOfParents = new List<NodeGeneric>();
            ListOfChildren = new List<NodeGeneric>();
            ProbTrue = new List<TextBox>();

            ProbFalse = new List<TextBox>(ProbTrue.Count);
            IsObservable = false;
        }

        public void SetProbFalse()
        {
            for (int i = 0; i < ProbTrue.Count; i++)
            {
                var x = Double.Parse(ProbTrue[i].Text);
                ProbFalse[i].Text = (1.0 - x).ToString();
            }
        }

        public static LinkedList<NodeGeneric> SortareTopologica(LinkedList<NodeGeneric> s)
        {
            LinkedList<NodeGeneric> l = new LinkedList<NodeGeneric>();
            while (s.Count != 0)
            {
                var n = s.First();
                s.RemoveFirst();
                l.AddFirst(n);
                foreach (var m in l)
                {
                    foreach (var x in m.ListOfChildren)
                    {
                        var e = x.ListOfChildren.FirstOrDefault();
                        if (e != null)
                        {
                            x.ListOfChildren.RemoveAt(0);
                            if (x.ListOfChildren.Count == 0)
                            {
                                s.AddFirst(m);
                            }
                        }

                    }
                }
            }

            return l;

        }
    }
}
