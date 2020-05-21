using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataModel;

namespace BayesianNetworkInterface
{
    public class Originator
    {
        public GenericNode EvidenceNode { get; set; }
        public List<GenericNode> AffectionList { get; set; }
        public Originator()
        {
            EvidenceNode = new GenericNode("default");
            AffectionList = new List<GenericNode>();
        }

        public Memento SaveMemento()
        {
            MessageBox.Show("Saving system state");
            return new Memento("memName");
        }

        public void RestoreMemento(Memento memento)
        {
            MessageBox.Show("Restoring system state");
            //restore data;
        }
    }
}
