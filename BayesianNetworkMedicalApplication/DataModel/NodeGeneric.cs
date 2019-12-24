using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class NodeGeneric
    {
        //
        //public int NoOfParents { get; set; }

        public List<NodeGeneric> ListOfParents;
        //nume nod
        public string Name { get; set; }
        public List<double> ProbTrue;
        public List<double> ProbFalse;
        //valoare observata bool sau index
        public List<NodeGeneric> ListOfChildren;
        public bool IsObservable { get; set; }

        public NodeGeneric()
        {
            ListOfParents = new List<NodeGeneric>();
            ListOfChildren=new List<NodeGeneric>();   
            ProbTrue = new List<double>();

            ProbFalse = new List<double>(ProbTrue.Count);
            IsObservable = false;
        }

        public void SetProbFalse()
        {
            for (int i = 0; i < ProbTrue.Count; i++)
            {
                ProbFalse[i] = 1 - ProbTrue[i];
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
                    var e = m.ListOfChildren.FirstOrDefault();
                    if (e != null)
                    {
                        m.ListOfChildren.RemoveAt(0);
                        if (m.ListOfChildren.Count == 0)
                        {
                            s.AddFirst(m);
                        }
                    }
                }
            }

            return l;
        }
    }
}
