using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Text;
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
            ListOfParents = new List<NodeGeneric>()
        };

        private List<TextBox> abcesListTextBoxes = new List<TextBox>();

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

            gripListTextBoxes.Add(textBox1); //PGd
            gripListTextBoxes.Add(textBox2); //PGn

            abcesListTextBoxes.Add(textBox3); //PAd
            abcesListTextBoxes.Add(textBox4); //PAn

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
            // read defaultValues from file
            try
            {
                FileStream fileStream = File.OpenRead("defaultValues.txt");
                StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8);
                String content = streamReader.ReadToEnd();
                string[] values = content.Split(' ', '\n');
                for (int i = 0; i < 1; ++i)
                {
                    gripa.ProbTrue.Add(Double.Parse(values[i]));
                }

                for (int i = 1; i < 2; ++i)
                {
                    gripa.ProbFalse.Add(Double.Parse(values[i]));
                }

                for (int i = 2; i < 3; ++i)
                {
                    abces.ProbTrue.Add(Double.Parse(values[i]));
                }

                for (int i = 3; i < 4; ++i)
                {
                    abces.ProbFalse.Add(Double.Parse(values[i]));
                }

                febra.ProbTrue.Add(Double.Parse(values[4]));
                febra.ProbTrue.Add(Double.Parse(values[6]));
                febra.ProbTrue.Add(Double.Parse(values[8]));
                febra.ProbTrue.Add(Double.Parse(values[10]));
                oboseala.ProbTrue.Add(Double.Parse(values[12]));
                oboseala.ProbTrue.Add(Double.Parse(values[14]));
                anorexie.ProbTrue.Add(Double.Parse(values[16]));
                anorexie.ProbTrue.Add(Double.Parse(values[18]));

                febra.ProbFalse.Add(Double.Parse(values[5]));
                febra.ProbFalse.Add(Double.Parse(values[7]));
                febra.ProbFalse.Add(Double.Parse(values[9]));
                febra.ProbFalse.Add(Double.Parse(values[11]));
                oboseala.ProbFalse.Add(Double.Parse(values[13]));
                oboseala.ProbFalse.Add(Double.Parse(values[15]));
                anorexie.ProbFalse.Add(Double.Parse(values[17]));
                anorexie.ProbFalse.Add(Double.Parse(values[19]));
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception.StackTrace);
            }

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
