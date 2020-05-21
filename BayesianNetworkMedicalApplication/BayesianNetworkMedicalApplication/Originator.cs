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

        }

        public void SetEvidenceNode(GenericNode node)
        {
            EvidenceNode = new GenericNode(node.Name);
            EvidenceNode.FalseProbabilityTextBoxes = new List<TextBox>(node.FalseProbabilityTextBoxes);
            EvidenceNode.TrueProbabilityTextBoxes = new List<TextBox>(node.TrueProbabilityTextBoxes);
            EvidenceNode.ListOfParents = new List<GenericNode>(node.ListOfParents);
            EvidenceNode.Status = node.Status;
        }

        public void SetAffectionList(List<GenericNode> affections)
        {
            AffectionList = new List<GenericNode>(affections);
            //AffectionList.Clear();
            //foreach (var node in affections)
            //{
            //    AffectionList.Add(node);
            //}
        }

        public Memento SaveMemento()
        {
            MessageBox.Show("Saving system state");
            return new Memento(EvidenceNode, AffectionList);
        }

        public void RestoreMemento(Memento memento)
        {
            EvidenceNode = memento.EvidenceNode;
            AffectionList = memento.AffectionList;
            MessageBox.Show("Restoring system state");
            //restore data;
        }
    }
}
