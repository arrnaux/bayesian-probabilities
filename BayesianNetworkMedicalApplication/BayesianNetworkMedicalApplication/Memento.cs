using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataModel;

namespace BayesianNetworkInterface
{
    public class Memento
    {
        public string Name { get; set; }
        public GenericNode EvidenceNode { get; set; }
        public List<GenericNode> AffectionList { get; set; }

        public Memento(GenericNode node,List<GenericNode> affection)
        {
            Name = node.Name;
            EvidenceNode=new GenericNode("");
            EvidenceNode.Name = node.Name;
            EvidenceNode.FalseProbabilityTextBoxes = new List<TextBox>(node.FalseProbabilityTextBoxes);
            EvidenceNode.TrueProbabilityTextBoxes = new List<TextBox>(node.TrueProbabilityTextBoxes);
            EvidenceNode.ListOfParents = new List<GenericNode>(node.ListOfParents);
            EvidenceNode.Status = node.Status;
            AffectionList = new List<GenericNode>(affection);
        }
    }
}
