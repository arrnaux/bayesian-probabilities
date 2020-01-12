using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Windows.Forms;
using DataModel;

namespace BayesianNetworkInterface
{
    public partial class MainForm : Form
    {
        // Important: always ensure that the affections are in topological order.
        private List<NodeGeneric> affections;
        private NodeGeneric evidenceNode;

        private NodeGeneric gripa = new NodeGeneric()
        {
            Name = "Gripa"
        };

        private List<TextBox> gripTrueListTextBoxes = new List<TextBox>();
        private List<TextBox> gripFalseListTextBoxes = new List<TextBox>();

        private NodeGeneric abces = new NodeGeneric()
        {
            Name = "Abces"
        };

        private List<TextBox> abcesTrueListTextBoxes = new List<TextBox>();
        private List<TextBox> abcesFalseListTextBoxes = new List<TextBox>();

        private NodeGeneric febra = new NodeGeneric()
        {
            Name = "Febra"
        };

        private List<TextBox> febraTrueListTextBoxes = new List<TextBox>();
        private List<TextBox> febraFalseListTextBoxes = new List<TextBox>();

        private NodeGeneric oboseala = new NodeGeneric()
        {
            Name = "Oboseala"
        };

        private List<TextBox> obosealaTrueTextBoxes = new List<TextBox>();
        private List<TextBox> obosealaFalseTextBoxes = new List<TextBox>();

        private NodeGeneric anorexie = new NodeGeneric()
        {
            Name = "Anorexie"
        };

        private List<TextBox> anorexieTrueTextBoxes = new List<TextBox>();
        private List<TextBox> anorexieFalseTextBoxes = new List<TextBox>();

        private List<GroupBox> groupBoxList;

        public MainForm()
        {
            InitializeComponent();

            SetNodeProperties();

            affections = new List<NodeGeneric>() {gripa, abces, febra, oboseala, anorexie};
            groupBoxList = new List<GroupBox>()
                {groupBoxGripa, groupBoxAbces, groupBoxFebra, groupBoxOboseala, groupBoxAnorexie};

            SetProbabilitiesFromFile(true);
            SetTextBoxProbabilities();
        }


        // Populare date inițiale
        private void button2_Click(object sender, EventArgs e)
        {
            SetProbabilitiesFromFile(true);
            SetTextBoxProbabilities();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string data = comboBox1.Text;

            foreach (var c in Controls)
            {
                if (c is GroupBox)
                {
                    string name = (c as GroupBox).Name;
                    if (name.Contains(data))
                    {
                        (c as GroupBox).Enabled = false;
                    }
                    else
                    {
                        (c as GroupBox).Enabled = true;
                    }
                }
            }
        }

        private void inputTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox inputTextBox = (TextBox) sender;

            String textBoxName = inputTextBox.Name;
            int lastIndex = textBoxName.Length - 1;
            int textBoxNumber = (textBoxName[lastIndex]) - '0';
            var x = Int32.Parse(textBoxName.Substring(7)) + 1;
            var nextBox = "textBox" + x;
            TextBox nextTextBox = new TextBox();
            foreach (var c in Controls)
            {
                if (c is TextBox && (c as TextBox).Name == nextBox)
                    nextTextBox = c as TextBox;
            }

            string input = inputTextBox.Text;
            double value;
            if (input != "")
            {
                bool result = Double.TryParse(input, out value);
                if (result)
                {
                    if (value < 0 || value > 1)
                    {
                        MessageBox.Show("Numarul trebuie sa fie in intervalul [0,1]!");
                        inputTextBox.Clear();
                    }
                    else
                    {
                        nextTextBox.Text = (1 - value).ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Caracter invalid!");
                    inputTextBox.Clear();
                }
            }
        }

        private void inputTextBox_TextChangedReverse(object sender, EventArgs e)
        {
            TextBox inputTextBox = (TextBox) sender;

            String textBoxName = inputTextBox.Name;
            int lastIndex = textBoxName.Length - 1;
            int textBoxNumber = (textBoxName[lastIndex]) - '0';
            var x = Int32.Parse(textBoxName.Substring(7)) - 1;
            var nextBox = "textBox" + x;
            TextBox nextTextBox = new TextBox();
            foreach (var c in Controls)
            {
                if (c is TextBox && (c as TextBox).Name == nextBox)
                    nextTextBox = c as TextBox;
            }

            string input = inputTextBox.Text;
            double value;
            if (input != "")
            {
                bool result = Double.TryParse(input, out value);
                if (result)
                {
                    if (value < 0 || value > 1)
                    {
                        MessageBox.Show("Numarul trebuie sa fie in intervalul [0,1]!");
                        inputTextBox.Clear();
                    }
                    else
                    {
                        nextTextBox.Text = (1 - value).ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Caracter invalid!");
                    inputTextBox.Clear();
                }
            }
        }

        private void SetStatusValue()
        {
            for (int i = 0; i < groupBoxList.Count; i++)
            {
                if (groupBoxList[i].Enabled == true)
                {
                    var checkedRadio = groupBoxList[i].Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
                    if (checkedRadio != null)
                    {
                        string checkedRadioText = checkedRadio.Text;

                        switch (checkedRadioText)
                        {
                            case "Da":
                                affections[i].Status = Status.True;
                                break;
                            case "Nu":
                                affections[i].Status = Status.False;
                                break;
                            case "Necunoscut":
                                affections[i].Status = Status.Unspecified;
                                break;
                            default:
                                affections[i].Status = Status.Na;
                                evidenceNode = affections[i];
                                break;
                        }
                    }
                }
                else
                {
                    affections[i].Status = Status.Na;
                    evidenceNode = affections[i];
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetMatrixValues();
            SetStatusValue();

            resultBox.Text += "\r\nNod evidenta: ";
            resultBox.Text += evidenceNode.Name;

            // this.ComputeProbabilityForEvidenceNode();
            double val = this.ComputeEvidenceNodeProbability()*100;
            resultBox.AppendText("\r\n" + val + "%");
        }
        
//        public double ComputeProbabilityForEvidenceNode()
//        {
//            // Compute a probability for the case when the variable is T, one for F, find alpha and serve the probability.
//            double trueProb = 1, falseProb = 1;
//
//            // Evidence node is considered to be T.
//            evidenceNode.Status = Status.True;
//            foreach (var affection in affections)
//            {
//                if (affection != evidenceNode)
//                {
//                    trueProb *= affection.ComputeProbabilityConsideringParents();
//                }
//
//                // TODO: check if same effect can be obtained with equals()
//            }
//
//            // Evidence node is considered to be F.
//            evidenceNode.Status = Status.False;
//            foreach (var affection in affections)
//            {
//                if (affection != evidenceNode)
//                {
//                    falseProb *= affection.ComputeProbabilityConsideringParents();
//                }
//
//                // TODO: check if same effect can be obtained with equals()
//            }
//
//            double alfa = 1.0 / (trueProb + falseProb);
//            return alfa * trueProb;
//        }

        private double EnumerateAll(List<NodeGeneric> affections)
        {
            if (affections.Count == 0)
            {
                return 1.0;
            }
            
            //List<NodeGeneric> updatedAffections = new List<NodeGeneric>(affections);
            NodeGeneric affection = affections.ElementAt(0);
            affections.RemoveAt(0);
            if (affection.Status == Status.False || affection.Status == Status.True)
            {
                double val = affection.ComputeProbabilityConsideringParents();
                return val * EnumerateAll(affections);
            }
            else
            {
                List<NodeGeneric> copy1 = new List<NodeGeneric>(affections);
                List<NodeGeneric> copy2 = new List<NodeGeneric>(affections);
                affection.Status = Status.False;
                double falseValue = affection.ComputeProbabilityConsideringParents();
                falseValue *= EnumerateAll(copy1);
                
                affection.Status = Status.True;
                double trueValue = affection.ComputeProbabilityConsideringParents();
                trueValue *= EnumerateAll(copy2);
                affection.Status = Status.Unspecified;
                return falseValue + trueValue;
            }
        }

        /// <summary>
        /// Computes the probability for the evidence node, considering the entire BN.
        /// </summary>
        /// <returns>The value corresponding for evidence node, when its status is T.</returns>
        private double ComputeEvidenceNodeProbability()
        {
            List<NodeGeneric> copy = new List<NodeGeneric>(affections);
            List<NodeGeneric> copy2 = new List<NodeGeneric>(affections);
            evidenceNode.Status = Status.True;
            double trueProb = EnumerateAll(copy);
            evidenceNode.Status = Status.False;
            double falseProb = EnumerateAll(copy2);
            double alfa = 1.0 / (trueProb + falseProb);
            return alfa * trueProb;
        }

        private void SetProbabilitiesFromFile(bool labValue)
        {
            foreach (var node in affections)
            {
                node.SetProbabilities(labValue);
            }
        }

        private void SetTextBoxProbabilities()
        {
            foreach (var node in affections)
            {
                node.SetTextBoxValues();
            }
        }

        private void SetMatrixValues()
        {
            foreach (var node in affections)
            {
                node.SetMatrixValuesFormTextBox();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetProbabilitiesFromFile(false);
            SetTextBoxProbabilities();
        }

        private void SetNodeProperties()
        {
            febra.ListOfParents.Add(gripa);
            febra.ListOfParents.Add(abces);
            oboseala.ListOfParents.Add(febra);
            anorexie.ListOfParents.Add(febra);

            gripTrueListTextBoxes.Add(textBox1); //PGd
            gripFalseListTextBoxes.Add(textBox2); //PGn

            gripa.ProbTrue = gripTrueListTextBoxes;
            gripa.ProbFalse = gripFalseListTextBoxes;

            abcesTrueListTextBoxes.Add(textBox3); //PAd
            abcesFalseListTextBoxes.Add(textBox4); //PAn

            abces.ProbTrue = abcesTrueListTextBoxes;
            abces.ProbFalse = abcesFalseListTextBoxes;

            febraTrueListTextBoxes.Add(textBox5);
            febraTrueListTextBoxes.Add(textBox7);
            febraTrueListTextBoxes.Add(textBox9);
            febraTrueListTextBoxes.Add(textBox11);

            febraFalseListTextBoxes.Add(textBox6);
            febraFalseListTextBoxes.Add(textBox8);
            febraFalseListTextBoxes.Add(textBox10);
            febraFalseListTextBoxes.Add(textBox12);

            febra.ProbTrue = febraTrueListTextBoxes;
            febra.ProbFalse = febraFalseListTextBoxes;

            obosealaTrueTextBoxes.Add(textBox13);
            obosealaTrueTextBoxes.Add(textBox15);

            obosealaFalseTextBoxes.Add(textBox14);
            obosealaFalseTextBoxes.Add(textBox16);

            oboseala.ProbTrue = obosealaTrueTextBoxes;
            oboseala.ProbFalse = obosealaFalseTextBoxes;

            anorexieTrueTextBoxes.Add(textBox17);
            anorexieTrueTextBoxes.Add(textBox19);

            anorexieFalseTextBoxes.Add(textBox18);
            anorexieFalseTextBoxes.Add(textBox20);

            anorexie.ProbTrue = anorexieTrueTextBoxes;
            anorexie.ProbFalse = anorexieFalseTextBoxes;
        }
    }
}