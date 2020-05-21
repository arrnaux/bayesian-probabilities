using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BayesianNetworkInterface
{
    public partial class MainForm : Form
    {
        // Important: always ensure that the affections are in topological order.
        public List<GenericNode> Affections { get; set; }
        public GenericNode EvidenceNode { get; set; }

        private Originator originator=new Originator();
        private Caretaker caretaker=new Caretaker();

        private GenericNode gripa = new GenericNode("Gripa");

        private GenericNode abces = new GenericNode("Abces");

        private GenericNode febra = new GenericNode("Febra");
        
        private GenericNode oboseala = new GenericNode("Oboseala");
        
        private GenericNode anorexie = new GenericNode("Anorexie");

        private List<GroupBox> groupBoxList;

        public MainForm()
        {
            InitializeComponent();
            SetNodeProperties();

            Affections = new List<GenericNode>() {gripa, abces, febra, oboseala, anorexie};
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
                    if (name.Contains(data) && !name.Contains("Input"))
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
            ChangeDynamicText(x,inputTextBox);
        }

        private void inputTextBox_TextChangedReverse(object sender, EventArgs e)
        {
            TextBox inputTextBox = (TextBox) sender;

            string textBoxName = inputTextBox.Name;
            int lastIndex = textBoxName.Length - 1;
            int textBoxNumber = (textBoxName[lastIndex]) - '0';
            var x = int.Parse(textBoxName.Substring(7)) - 1;
            ChangeDynamicText(x, inputTextBox);
        }

        private void ChangeDynamicText(int x, TextBox inputTextBox)
        {
            var nextBox = "textBox" + x;
            TextBox nextTextBox = new TextBox();
            foreach (var c in Controls)
            {
                if (c is GroupBox && (c as GroupBox).Name.Contains("Input"))
                {
                    GroupBox groupBox = c as GroupBox;
                    foreach (var textBoxControl in groupBox.Controls)
                    {
                        if (textBoxControl is TextBox && (textBoxControl as TextBox).Name == nextBox)
                            nextTextBox = textBoxControl as TextBox;
                    }
                }
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
                                Affections[i].Status = Status.True;
                                break;
                            case "Nu":
                                Affections[i].Status = Status.False;
                                break;
                            case "Necunoscut":
                                Affections[i].Status = Status.Unspecified;
                                break;
                            default:
                                Affections[i].Status = Status.Na;
                                EvidenceNode = Affections[i];
                                break;
                        }
                    }
                }
                else
                {
                    Affections[i].Status = Status.Na;
                    EvidenceNode = Affections[i];
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetMatrixValues();
            SetStatusValue();

            resultBox.Text += "Variabila de interogare: ";
            resultBox.Text += EvidenceNode.Name;

            // this.ComputeProbabilityForEvidenceNode();
            double val = this.ComputeEvidenceNodeProbability() * 100;
            resultBox.AppendText("\r\nProbabilitate: " + val + "%\r\n\r\n");
        }

        public double EnumerateAll(List<GenericNode> affections)
        {
            if (affections.Count == 0)
            {
                return 1.0;
            }

            GenericNode affection = affections.ElementAt(0);
            affections.RemoveAt(0);
            if (affection.Status == Status.False || affection.Status == Status.True)
            {
                double val = affection.ComputeProbabilityConsideringParents();
                return val * EnumerateAll(affections);
            }
            else
            {
                // Use something else here.
                List<GenericNode> copy1 = new List<GenericNode>(affections);
                List<GenericNode> copy2 = new List<GenericNode>(affections);
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
        public double ComputeEvidenceNodeProbability()
        {
            List<GenericNode> copy = new List<GenericNode>(Affections);
            List<GenericNode> copy2 = new List<GenericNode>(Affections);
            EvidenceNode.Status = Status.True;
            double trueProb = EnumerateAll(copy);
            EvidenceNode.Status = Status.False;
            double falseProb = EnumerateAll(copy2);
            double alfa = 1.0 / (trueProb + falseProb);
            return alfa * trueProb;
        }

        private void SetProbabilitiesFromFile(bool labValue)
        {
            foreach (var node in Affections)
            {
                node.SetProbabilities(labValue);
            }
        }

        private void SetTextBoxProbabilities()
        {
            foreach (var node in Affections)
            {
                node.LoadProabilitiesFromMatrix();
            }
        }

        //??? ce face asta?
        public void SetMatrixValues()
        {
            foreach (var node in Affections)
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
            febra.ListOfParents.Add(gripa);
            febra.ListOfParents.Add(abces);
            oboseala.ListOfParents.Add(febra);
            anorexie.ListOfParents.Add(febra);

            gripa.TrueProbabilityTextBoxes = GetTextBoxesForDiseaseName("Gripa",true);
            gripa.FalseProbabilityTextBoxes = GetTextBoxesForDiseaseName("Gripa", false);

            abces.TrueProbabilityTextBoxes = GetTextBoxesForDiseaseName("Abces", true);
            abces.FalseProbabilityTextBoxes = GetTextBoxesForDiseaseName("Abces", false);

            febra.TrueProbabilityTextBoxes = GetTextBoxesForDiseaseName("Febra", true);
            febra.FalseProbabilityTextBoxes = GetTextBoxesForDiseaseName("Febra", false);

            oboseala.TrueProbabilityTextBoxes = GetTextBoxesForDiseaseName("Oboseala", true); ;
            oboseala.FalseProbabilityTextBoxes = GetTextBoxesForDiseaseName("Oboseala", false);

            anorexie.TrueProbabilityTextBoxes = GetTextBoxesForDiseaseName("Anorexie", true); ;
            anorexie.FalseProbabilityTextBoxes = GetTextBoxesForDiseaseName("Anorexie", false); ;
        }

        private List<TextBox> GetTextBoxesForDiseaseName(string name, bool status)
        {
            List<TextBox> diseaseList = new List<TextBox>();
            GroupBox groupBox = null;
            foreach (var obj in Controls)
            {
                if (obj is GroupBox && (obj as GroupBox).Name.Contains(name + "Input"))
                {
                    groupBox=obj as GroupBox;
                }
            }

            if (groupBox != null)
                foreach (var textBox in groupBox.Controls)
                {
                    if (textBox is TextBox)
                    {
                        string textBoxName = (textBox as TextBox).Name;
                        int lastIndex = textBoxName.Length - 1;
                        bool textBoxType = ((textBoxName[lastIndex]) - '0') % 2 == 0;
                        if (textBoxType != status)
                        {
                            diseaseList.Add(textBox as TextBox);
                        }
                    }

                }

            return diseaseList;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "BayesianNetworkMedicalApplication.chm");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //save curremt state;
            //set originator Properties;
            originator.EvidenceNode.Name = EvidenceNode.Name;
            caretaker.Memento = originator.SaveMemento();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //restor saved state;
            originator.RestoreMemento(caretaker.Memento);
        }
    }
}