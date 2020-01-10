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
        // TODO: read values from file
        private List<TextBox> gripTrueListTextBoxes = new List<TextBox>();
        private List<TextBox> gripFalseListTextBoxes = new List<TextBox>();
        private List<NodeGeneric> allNodeGenerics;
        private NodeGeneric gripa = new NodeGeneric()
        {
            Name = "Gripa",
            ListOfParents = new List<NodeGeneric>()
        };

        private List<TextBox> abcesTrueListTextBoxes = new List<TextBox>();
        private List<TextBox> abcesFalseListTextBoxes = new List<TextBox>();

        private NodeGeneric abces = new NodeGeneric()
        {
            Name = "Abces",
            ListOfParents = new List<NodeGeneric>()
        };

        private List<TextBox> febraTrueListTextBoxes = new List<TextBox>();
        private List<TextBox> febraFalseListTextBoxes = new List<TextBox>();

        private NodeGeneric febra = new NodeGeneric()
        {
            Name = "Febra",
            ListOfParents = new List<NodeGeneric>()
        };

        private List<TextBox> obosealaTrueTextBoxes = new List<TextBox>();
        private List<TextBox> obosealaFalseTextBoxes = new List<TextBox>();

        private NodeGeneric oboseala = new NodeGeneric()
        {
            Name = "Oboseala",
            ListOfParents = new List<NodeGeneric>()
        };

        private List<TextBox> anorexieTrueTextBoxes = new List<TextBox>();
        private List<TextBox> anorexieFalseTextBoxes = new List<TextBox>();

        private NodeGeneric anorexie = new NodeGeneric()
        {
            Name = "Anorexie",
            ListOfParents = new List<NodeGeneric>(),
        };

        private List<GroupBox> groupBoxList;

        public MainForm()
        {
            InitializeComponent();

            febra.ListOfParents.Add(abces);
            febra.ListOfParents.Add(gripa);
            oboseala.ListOfParents.Add(febra);
            anorexie.ListOfParents.Add(febra);

            gripa.ListOfChildren.Add(febra);
            abces.ListOfChildren.Add(febra);
            febra.ListOfChildren.Add(oboseala);
            febra.ListOfChildren.Add(anorexie);

            gripa.SetProbFalse();
            abces.SetProbFalse();
            febra.SetProbFalse();
            oboseala.SetProbFalse();
            anorexie.SetProbFalse();

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

            allNodeGenerics = new List<NodeGeneric>() { gripa, anorexie, febra, abces, oboseala };
            groupBoxList = new List<GroupBox>() { groupBoxGripa, groupBoxAnorexie, groupBoxFebra, groupBoxAbces, groupBoxOboseala };
        }

        //populare date initiale
        private void button2_Click(object sender, EventArgs e)
        {
            //read defaultValues from file
            //try
            //{
            //    var fileStream = File.OpenRead("defaultValues.txt");
            //    var streamReader = new StreamReader(fileStream, Encoding.UTF8);
            //    var content = streamReader.ReadToEnd();
            //    var values = content.Split(' ', '\n');
            //    for (var index = 0; index < values.Length; ++index)
            //    {
            //        var probValue = double.Parse(values[index]);
            //        if (index % 2 != 0) continue;
            //        if (index <= 1)
            //        {
            //            gripa.ProbTrue.Add(probValue);
            //            gripa.ProbFalse.Add(1 - probValue);
            //        }
            //        else
            //        {
            //            if (index <= 3)
            //            {
            //                abces.ProbTrue.Add(probValue);
            //                abces.ProbFalse.Add(1 - probValue);
            //            }
            //            else
            //            {
            //                if (index <= 11)
            //                {
            //                    febra.ProbTrue.Add(probValue);
            //                    febra.ProbFalse.Add(1 - probValue);
            //                }
            //                else
            //                {
            //                    if (index <= 15)
            //                    {
            //                        oboseala.ProbTrue.Add(probValue);
            //                        oboseala.ProbFalse.Add(1 - probValue);
            //                    }
            //                    else
            //                    {
            //                        anorexie.ProbTrue.Add(probValue);
            //                        anorexie.ProbFalse.Add(1 - probValue);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (IOException exception)
            //{
            //    Console.WriteLine(exception.StackTrace);
            //}

            ////datele pt gripa
            //gripListTextBoxes[0].Text = gripa.ProbTrue.FirstOrDefault().ToString();
            //gripListTextBoxes[1].Text = gripa.ProbFalse.FirstOrDefault().ToString();

            ////datele pt abces
            //abcesListTextBoxes[0].Text = abces.ProbTrue.FirstOrDefault().ToString();
            //abcesListTextBoxes[1].Text = abces.ProbFalse.FirstOrDefault().ToString();

            ////datele pt febra
            //for (var i = 0; i < febra.ProbTrue.Count; i++)
            //{
            //    febraTrueListTextBoxes[i].Text = febra.ProbTrue[i].ToString();
            //    febraFalseListTextBoxes[i].Text = febra.ProbFalse[i].ToString();
            //}

            ////datele pt oboseala
            //for (var i = 0; i < oboseala.ProbTrue.Count; i++)
            //{
            //    obosealaTrueTextBoxes[i].Text = oboseala.ProbTrue[i].ToString();
            //    obosealaFalseTextBoxes[i].Text = oboseala.ProbFalse[i].ToString();
            //}

            ////datele pt anorexie
            //for (var i = 0; i < anorexie.ProbTrue.Count; i++)
            //{
            //    anorexieTrueTextBoxes[i].Text = anorexie.ProbTrue[i].ToString();
            //    anorexieFalseTextBoxes[i].Text = anorexie.ProbFalse[i].ToString();
            //}
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
            TextBox inputTextBox = (TextBox)sender;

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
            TextBox inputTextBox = (TextBox)sender;

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

        private void setNodeStatusValue()
        {
            for (int i = 0; i < groupBoxList.Count; i++)
            {
                if (groupBoxList[i].Enabled == true)
                {
                    var checkedRadio = groupBoxList[i].Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
                    string checkedRadioText = checkedRadio.Text;

                    switch (checkedRadioText)
                    {
                        case "Da":
                            allNodeGenerics[i].NodeStatus = IsUsed.TRUE;
                            break;
                        case "Nu":
                            allNodeGenerics[i].NodeStatus = IsUsed.FALSE;
                            break;
                        case "Necunoscut":
                            allNodeGenerics[i].NodeStatus = IsUsed.UNSPECIFIED;
                            break;
                        default:
                            allNodeGenerics[i].NodeStatus = IsUsed.NA;
                            break;
                    }
                }
                else
                {
                    allNodeGenerics[i].NodeStatus = IsUsed.NA;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            setNodeStatusValue();
            for (int i = 0; i < allNodeGenerics.Count; i++)
            {
                resultBox.Text += allNodeGenerics[i].Name;
                resultBox.Text += "->";
                resultBox.Text += allNodeGenerics[i].NodeStatus;
                resultBox.Text += "\r\n";
            }

            //string nodeName = comboBox1.Text;
            //foreach (var node in allNodeGenerics)
            //{
            //    if (node.Name == nodeName)
            //    {
            //        resultBox.Text += NodeGeneric.ComputeBayes(node).ToString();
            //        resultBox.Text += "\r\n";
            //    }
            //}


            // stabileste ce variabila este nod evidenta
            // @Nicu -> pune-o in variabila de jos

            NodeGeneric evidenceNode = febra;
            gripa.SetProbabilitis("gripa.txt");
            // aici vin calculate 2 sume, una cu true si una cu false
            //double trueValue=EnumerateAll(evidenceNode true)
            //double falseValue=EnumerateAll(evidenceNode false)
       
        }

        private double EnumerateAll (List<NodeGeneric> nodesThatMatter, NodeGeneric evidenceNode)
        {
            NodeGeneric smth = nodesThatMatter.ElementAt(0);
            if (smth.NodeStatus != IsUsed.UNSPECIFIED)
            {
                nodesThatMatter.Remove(smth);
                //return probabilitate  * enumerateAll)
               // functie creata de Igna

            }
            return -1;
        }
    }
}
