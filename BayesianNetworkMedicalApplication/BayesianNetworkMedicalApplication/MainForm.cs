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
        private List<GenericNode> _affections;
        private GenericNode _evidenceNode;

        // TODO: use a design pattern on this.
        // Maybe a factory?/decorator?
        private GenericNode gripa = new GenericNode("Gripa");
        private List<TextBox> gripTrueListTextBoxes = new List<TextBox>();
        private List<TextBox> gripFalseListTextBoxes = new List<TextBox>();

        private GenericNode abces = new GenericNode("Abces");
        private List<TextBox> abcesTrueListTextBoxes = new List<TextBox>();
        private List<TextBox> abcesFalseListTextBoxes = new List<TextBox>();

        private GenericNode febra = new GenericNode("Febra");
        private List<TextBox> febraTrueListTextBoxes = new List<TextBox>();
        private List<TextBox> febraFalseListTextBoxes = new List<TextBox>();

        private GenericNode oboseala = new GenericNode("Oboseala");
        private List<TextBox> obosealaTrueTextBoxes = new List<TextBox>();
        private List<TextBox> obosealaFalseTextBoxes = new List<TextBox>();

        private GenericNode anorexie = new GenericNode("Anorexie");
        private List<TextBox> anorexieTrueTextBoxes = new List<TextBox>();
        private List<TextBox> anorexieFalseTextBoxes = new List<TextBox>();

        private List<GroupBox> groupBoxList;

        public MainForm()
        {
            InitializeComponent();
            SetNodeProperties();

            _affections = new List<GenericNode>() {gripa, abces, febra, oboseala, anorexie};
            groupBoxList = new List<GroupBox>()
                {groupBoxGripa, groupBoxAbces, groupBoxFebra, groupBoxOboseala, groupBoxAnorexie};

            SetProbabilitiesFromFile(true);
            SetTextBoxProbabilities();
        }


        // Populare date inițiale.
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

            (button1 as Button).Enabled = true;
        }

        private void inputTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox inputTextBox = (TextBox) sender;

            string textBoxName = inputTextBox.Name;
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
                bool result = double.TryParse(input, out value);
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

            string textBoxName = inputTextBox.Name;
            int lastIndex = textBoxName.Length - 1;
            int textBoxNumber = (textBoxName[lastIndex]) - '0';
            var x = int.Parse(textBoxName.Substring(7)) - 1;
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
                bool result = double.TryParse(input, out value);
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
                                _affections[i]._status = Status.True;
                                break;
                            case "Nu":
                                _affections[i]._status = Status.False;
                                break;
                            case "Necunoscut":
                                _affections[i]._status = Status.Unspecified;
                                break;
                            default:
                                _affections[i]._status = Status.Na;
                                _evidenceNode = _affections[i];
                                break;
                        }
                    }
                }
                else
                {
                    _affections[i]._status = Status.Na;
                    _evidenceNode = _affections[i];
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetMatrixValues();
            SetStatusValue();

            resultBox.Text += "Variabila de interogare: ";
            resultBox.Text += _evidenceNode._name;

            // this.ComputeProbabilityForEvidenceNode();
            double val = this.ComputeEvidenceNodeProbability() * 100;
            resultBox.AppendText("\r\nProbabilitate: " + val + "%\r\n\r\n");
        }

        private double EnumerateAll(List<GenericNode> affections)
        {
            if (affections.Count == 0)
            {
                return 1.0;
            }

            GenericNode affection = affections.ElementAt(0);
            affections.RemoveAt(0);
            if (affection._status == Status.False || affection._status == Status.True)
            {
                double val = affection.ComputeProbabilityConsideringParents();
                return val * EnumerateAll(affections);
            }
            else
            {
                // Use something else here.
                List<GenericNode> copy1 = new List<GenericNode>(affections);
                List<GenericNode> copy2 = new List<GenericNode>(affections);
                affection._status = Status.False;
                double falseValue = affection.ComputeProbabilityConsideringParents();
                falseValue *= EnumerateAll(copy1);

                affection._status = Status.True;
                double trueValue = affection.ComputeProbabilityConsideringParents();
                trueValue *= EnumerateAll(copy2);
                affection._status = Status.Unspecified;
                return falseValue + trueValue;
            }
        }

        /// <summary>
        /// Computes the probability for the evidence node, considering the entire BN.
        /// </summary>
        /// <returns>The value corresponding for evidence node, when its status is T.</returns>
        private double ComputeEvidenceNodeProbability()
        {
            List<GenericNode> copy = new List<GenericNode>(_affections);
            List<GenericNode> copy2 = new List<GenericNode>(_affections);
            _evidenceNode._status = Status.True;
            double trueProb = EnumerateAll(copy);
            _evidenceNode._status = Status.False;
            double falseProb = EnumerateAll(copy2);
            double alfa = 1.0 / (trueProb + falseProb);
            return alfa * trueProb;
        }

        private void SetProbabilitiesFromFile(bool labValue)
        {
            foreach (var node in _affections)
            {
                node.SetProbabilities(labValue);
            }
        }

        private void SetTextBoxProbabilities()
        {
            foreach (var node in _affections)
            {
                node.LoadProabilitiesFromMatrix();
            }
        }

        //??? ce face asta?
        private void SetMatrixValues()
        {
            foreach (var node in _affections)
            {
                node.LoadProbabilitiesFromGUI();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetProbabilitiesFromFile(false);
            SetTextBoxProbabilities();
        }

        // TODO: use a design pattern on this.
        private void SetNodeProperties()
        {
            febra._listOfParents.Add(gripa);
            febra._listOfParents.Add(abces);
            oboseala._listOfParents.Add(febra);
            anorexie._listOfParents.Add(febra);

            gripTrueListTextBoxes.Add(textBox1);
            gripFalseListTextBoxes.Add(textBox2);

            gripa._trueProbabilityTextBoxes = gripTrueListTextBoxes;
            gripa._falseProbabilityTextBoxes = gripFalseListTextBoxes;

            abcesTrueListTextBoxes.Add(textBox3);
            abcesFalseListTextBoxes.Add(textBox4);

            abces._trueProbabilityTextBoxes = abcesTrueListTextBoxes;
            abces._falseProbabilityTextBoxes = abcesFalseListTextBoxes;

            febraTrueListTextBoxes.Add(textBox5);
            febraTrueListTextBoxes.Add(textBox7);
            febraTrueListTextBoxes.Add(textBox9);
            febraTrueListTextBoxes.Add(textBox11);

            febraFalseListTextBoxes.Add(textBox6);
            febraFalseListTextBoxes.Add(textBox8);
            febraFalseListTextBoxes.Add(textBox10);
            febraFalseListTextBoxes.Add(textBox12);

            febra._trueProbabilityTextBoxes = febraTrueListTextBoxes;
            febra._falseProbabilityTextBoxes = febraFalseListTextBoxes;

            obosealaTrueTextBoxes.Add(textBox13);
            obosealaTrueTextBoxes.Add(textBox15);

            obosealaFalseTextBoxes.Add(textBox14);
            obosealaFalseTextBoxes.Add(textBox16);

            oboseala._trueProbabilityTextBoxes = obosealaTrueTextBoxes;
            oboseala._falseProbabilityTextBoxes = obosealaFalseTextBoxes;

            anorexieTrueTextBoxes.Add(textBox17);
            anorexieTrueTextBoxes.Add(textBox19);

            anorexieFalseTextBoxes.Add(textBox18);
            anorexieFalseTextBoxes.Add(textBox20);

            anorexie._trueProbabilityTextBoxes = anorexieTrueTextBoxes;
            anorexie._falseProbabilityTextBoxes = anorexieFalseTextBoxes;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "BayesianNetworkMedicalApplication.chm");
        }
    }
}