/**************************************************************************
 *                                                                        *
 *  File:        MainForm.cs                                              *
 *  Copyright:   (c) 2020 Gabriel Răileanu, Nicolae Boca, Ștefan Ignătescu*
 *  Website:     https://github.com/arrnaux/bayesian-probabilities        *
 *  Description: Generates file information headers.                      *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/

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
        private Originator originator = new Originator();
        private Caretaker caretaker = new Caretaker();

        public List<GenericNode> Affections { get; }
        public GenericNode EvidenceNode { get; set; }

        private GenericNode _gripa = new GenericNode("Gripa");
        private GenericNode _abces = new GenericNode("Abces");
        private GenericNode _febra = new GenericNode("Febra");
        private GenericNode _oboseala = new GenericNode("Oboseala");
        private GenericNode _anorexie = new GenericNode("Anorexie");

        private List<GroupBox> _groupBoxList;

        public MainForm()
        {
            InitializeComponent();
            SetNodeProperties();

            Affections = new List<GenericNode>() {_gripa, _abces, _febra, _oboseala, _anorexie};
            _groupBoxList = new List<GroupBox>()
                {groupBoxGripa, groupBoxAbces, groupBoxFebra, groupBoxOboseala, groupBoxAnorexie};

            SetProbabilitiesFromFile(true);
            SetTextBoxProbabilities();
        }


        /// <summary>
        /// Load initial data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            SetProbabilitiesFromFile(true);
            SetTextBoxProbabilities();
        }

        /// <summary>
        /// Callback for stmh.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            computeProbabilityBtn.Enabled = true;
        }

        private void inputTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox inputTextBox = (TextBox) sender;

            string textBoxName = inputTextBox.Name;
            var textBoxNumber = int.Parse(textBoxName.Substring(7)) + 1;
            ChangeDynamicText(textBoxNumber, inputTextBox);
        }

        private void inputTextBox_TextChangedReverse(object sender, EventArgs e)
        {
            TextBox inputTextBox = (TextBox) sender;

            string textBoxName = inputTextBox.Name;
            var textBoxNumber = int.Parse(textBoxName.Substring(7)) - 1;
            ChangeDynamicText(textBoxNumber, inputTextBox);
        }

        private void ChangeDynamicText(int textBoxNumber, TextBox inputTextBox)
        {
            var nextBox = "textBox" + textBoxNumber;
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
            if (!input.Equals(""))
            {
                double value = CheckValidNumber(input);
                if (value != -1)
                {
                    nextTextBox.Text = (1 - value).ToString();
                }
                else
                {
                    MessageBox.Show("Eroare! Caracter invalid sau numar in afara intervalului [0,1]!");
                    inputTextBox.Clear();
                }
            }
        }

        public double CheckValidNumber(string input)
        {
            bool result = double.TryParse(input, out var value);
            if (result)
            {
                if (value >= 0 && value <= 1)
                {
                    return value;
                }
            }

            return -1;
        }

        private void SetStatusValue()
        {
            for (int i = 0; i < _groupBoxList.Count; i++)
            {
                if (_groupBoxList[i].Enabled)
                {
                    var checkedRadio = _groupBoxList[i].Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
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

        private void ComputeProbabilityBtn_Click(object sender, EventArgs e)
        {
            SetMatrixValues();
            SetStatusValue();

            resultBox.Text += "Variabila de interogare: ";
            resultBox.Text += EvidenceNode.Name;

            double val = ComputeEvidenceNodeProbability() * 100;
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
                node.LoadProbabilitiesFromMatrix();
            }
        }

        /// <summary>
        /// Populate the matrix from the values found in GUI textboxes.
        /// </summary>
        public void SetMatrixValues()
        {
            foreach (var node in Affections)
            {
                node.LoadProbabilitiesFromGui();
            }
        }

        /// <summary>
        /// Populate with custom values that were previously defined in default file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            SetProbabilitiesFromFile(false);
            SetTextBoxProbabilities();
        }

        /// <summary>
        /// Setup the bayesian network with nodes corresponding to the affections.
        /// </summary>
        private void SetNodeProperties()
        {
            _febra.ListOfParents.Add(_gripa);
            _febra.ListOfParents.Add(_abces);
            _oboseala.ListOfParents.Add(_febra);
            _anorexie.ListOfParents.Add(_febra);

            _gripa.TrueProbabilityTextBoxes = GetTextBoxesForDiseaseName("Gripa", true);
            _gripa.FalseProbabilityTextBoxes = GetTextBoxesForDiseaseName("Gripa", false);

            _abces.TrueProbabilityTextBoxes = GetTextBoxesForDiseaseName("Abces", true);
            _abces.FalseProbabilityTextBoxes = GetTextBoxesForDiseaseName("Abces", false);

            _febra.TrueProbabilityTextBoxes = GetTextBoxesForDiseaseName("Febra", true);
            _febra.FalseProbabilityTextBoxes = GetTextBoxesForDiseaseName("Febra", false);

            _oboseala.TrueProbabilityTextBoxes = GetTextBoxesForDiseaseName("Oboseala", true);
            _oboseala.FalseProbabilityTextBoxes = GetTextBoxesForDiseaseName("Oboseala", false);

            _anorexie.TrueProbabilityTextBoxes = GetTextBoxesForDiseaseName("Anorexie", true);
            _anorexie.FalseProbabilityTextBoxes = GetTextBoxesForDiseaseName("Anorexie", false);
        }

        /// <summary>
        /// Get the textBoxes associated with a specific disease.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        private List<TextBox> GetTextBoxesForDiseaseName(string name, bool status)
        {
            List<TextBox> diseasesList = new List<TextBox>();
            GroupBox groupBox = null;
            foreach (var obj in Controls)
            {
                if (obj is GroupBox && (obj as GroupBox).Name.Contains(name + "Input"))
                {
                    groupBox = obj as GroupBox;
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
                            diseasesList.Add(textBox as TextBox);
                        }
                    }
                }

            return diseasesList;
        }

        /// <summary>
        /// Shows the help menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpButton_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "BayesianNetworkMedicalApplication.chm");
        }

        /// <summary>
        /// Save the current values from GUI to be restored in a future moment.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveStateBtn_Click(object sender, EventArgs e)
        {
            List<TextBox> textBoxes = new List<TextBox>();
            foreach (var affection in Affections)
            {
                foreach (var textBox in affection.TrueProbabilityTextBoxes)
                {
                    textBoxes.Add(textBox);
                }
            }

            MessageBox.Show("Starea sistemului a fost salvata.");
            originator.SetTextBoxValues(textBoxes);
            caretaker.Memento = originator.SaveMemento();
        }

        private void RestoreStateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                originator.RestoreMemento(caretaker.Memento);
                int i = 0;
                foreach (var affection in Affections)
                {
                    foreach (var textBox in affection.TrueProbabilityTextBoxes)
                    {
                        textBox.Text = originator.TextBoxValues[i].ToString();
                        i++;
                    }
                }

                MessageBox.Show("Starea sistemului a fost restaurata.");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}