using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace BayesianNetworkInterface
{
    public partial class MainForm : Form
    {
        // TODO: read values from file
        private List<TextBox> gripListTextBoxes = new List<TextBox>();

        private NodeGeneric gripa = new NodeGeneric()
        {
            Name = "Gripa",
            ListOfParents = new List<NodeGeneric>(),
            ProbTrue = new List<double>() { 0.1 },
            ProbFalse = new List<double>() { 0.9 }
        };

        private List<TextBox> abcesListTextBoxes = new List<TextBox>();

        private NodeGeneric abces = new NodeGeneric()
        {
            Name = "Abces",
            ListOfParents = new List<NodeGeneric>(),
            ProbTrue = new List<double>() { 0.05 },
            ProbFalse = new List<double>() { 0.95 }
        };

        private List<TextBox> febraTrueListTextBoxes = new List<TextBox>();
        private List<TextBox> febraFalseListTextBoxes = new List<TextBox>();


        private NodeGeneric febra = new NodeGeneric()
        {
            Name = "Febra",
            ListOfParents = new List<NodeGeneric>(),
            ProbTrue = new List<double>()
            {
                0.8, 0.7, 0.25, 0.05
            },
            ProbFalse = new List<double>()
            {
                0.2,0.3,0.75,0.95
            }
        };

        private List<TextBox> obosealaTrueTextBoxes = new List<TextBox>();
        private List<TextBox> obosealaFalseTextBoxes = new List<TextBox>();
        private NodeGeneric oboseala = new NodeGeneric()
        {
            Name = "Oboseala",
            ListOfParents = new List<NodeGeneric>(),
            ProbTrue = new List<double>()
            {
                0.6,0.2
            },
            ProbFalse = new List<double>()
            {
            0.4,0.8
        }
        };

        private List<TextBox> anorexieTrueTextBoxes = new List<TextBox>();
        private List<TextBox> anorexieFalseTextBoxes = new List<TextBox>();

        private NodeGeneric anorexie = new NodeGeneric()
        {
            Name = "Anorexie",
            ListOfParents = new List<NodeGeneric>(),
            ProbTrue = new List<double>()
            {
                0.5,0.1
            },
            ProbFalse = new List<double>()
            {
                0.5,0.8
            }
        };

        public MainForm()
        {
            InitializeComponent();

            febra.ListOfParents.Add(abces);
            febra.ListOfParents.Add(gripa);
            oboseala.ListOfParents.Add(febra);
            anorexie.ListOfParents.Add(febra);

            gripa.SetProbFalse();
            abces.SetProbFalse();
            febra.SetProbFalse();
            oboseala.SetProbFalse();
            anorexie.SetProbFalse();

            gripListTextBoxes.Add(textBox1);//PGd
            gripListTextBoxes.Add(textBox2);//PGn

            abcesListTextBoxes.Add(textBox3);//PAd
            abcesListTextBoxes.Add(textBox4);//PAn

            febraTrueListTextBoxes.Add(textBox5);
            febraTrueListTextBoxes.Add(textBox7);
            febraTrueListTextBoxes.Add(textBox9);
            febraTrueListTextBoxes.Add(textBox11);

            febraFalseListTextBoxes.Add(textBox6);
            febraFalseListTextBoxes.Add(textBox8);
            febraFalseListTextBoxes.Add(textBox10);
            febraFalseListTextBoxes.Add(textBox12);

            obosealaTrueTextBoxes.Add(textBox13);
            obosealaTrueTextBoxes.Add(textBox15);

            obosealaFalseTextBoxes.Add(textBox14);
            obosealaFalseTextBoxes.Add(textBox16);

            anorexieTrueTextBoxes.Add(textBox17);
            anorexieTrueTextBoxes.Add(textBox19);

            anorexieFalseTextBoxes.Add(textBox18);
            anorexieFalseTextBoxes.Add(textBox20);

        }

        //populare date initiale
        private void button2_Click(object sender, EventArgs e)
        {
            //datele pt gripa
            gripListTextBoxes[0].Text = gripa.ProbTrue.FirstOrDefault().ToString();
            gripListTextBoxes[1].Text = gripa.ProbFalse.FirstOrDefault().ToString();
            //datele pt abces
            abcesListTextBoxes[0].Text = abces.ProbTrue.FirstOrDefault().ToString();
            abcesListTextBoxes[1].Text = abces.ProbTrue.FirstOrDefault().ToString();
            //datele pt febra

            for (var i = 0; i < febra.ProbTrue.Count; i++)
            {
                febraTrueListTextBoxes[i].Text = febra.ProbTrue[i].ToString();
                febraFalseListTextBoxes[i].Text = febra.ProbFalse[i].ToString();
            }
            //datele pt oboseala

            for (var i = 0; i < oboseala.ProbTrue.Count; i++)
            {
                obosealaTrueTextBoxes[i].Text = oboseala.ProbTrue[i].ToString();
                obosealaFalseTextBoxes[i].Text = oboseala.ProbFalse[i].ToString();
            }
            //datele pt anorexie

            for (var i = 0; i < anorexie.ProbTrue.Count; i++)
            {
                anorexieTrueTextBoxes[i].Text = anorexie.ProbTrue[i].ToString();
                anorexieFalseTextBoxes[i].Text = anorexie.ProbFalse[i].ToString();
            }
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

        public double ComputeBayesianNetwork(NodeGeneric currentNode)
        {
            if (currentNode.ListOfParents.Count == 0)
            {
                return currentNode.ProbTrue.FirstOrDefault();
            }
            else
            {
            }
            return 0;
        }


    }
}
