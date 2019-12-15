using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataModel;

namespace BayesianNetworkInterface
{
    public partial class MainForm : Form
    {
        // True case is indexed on index 1

        // TODO: read values from file
        private List<TextBox> gripListTextBoxes=new List<TextBox>();
        private Gripa gripa = new Gripa();

        private List<TextBox> abcesListTextBoxes=new List<TextBox>();
        private Abces abces = new Abces();

        private List<TextBox> febraTrueListTextBoxes=new List<TextBox>();
        private List<TextBox> febraFalseListTextBoxes = new List<TextBox>();
        private Febra febra = new Febra();

        private List<TextBox> obosealaTrueTextBoxes=new List<TextBox>();
        private List<TextBox> obosealaFalseTextBoxes = new List<TextBox>();
        private Oboseala oboseala = new Oboseala();

        private List<TextBox> anorexieTrueTextBoxes = new List<TextBox>();
        private List<TextBox> anorexieFalseTextBoxes = new List<TextBox>();

        private Anorexie anorexie = new Anorexie();
        double[] labValues;
        int noValues;

        public MainForm()
        {
            InitializeComponent();

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

        private void label17_Click(object sender, EventArgs e)
        {

        }
        //populare date initiale
        private void button2_Click(object sender, EventArgs e)
        {
            //datele pt gripa
            gripListTextBoxes[0].Text = gripa.PGd.ToString();
            gripListTextBoxes[1].Text = gripa.PGn.ToString();
            //datele pt abces
            abcesListTextBoxes[0].Text = abces.PAd.ToString();
            abcesListTextBoxes[1].Text = abces.PAn.ToString();
            //datele pt febra

            for (int i = 0; i < Febra.NoData; i++)
            {
                febraTrueListTextBoxes[i].Text = febra.PFd[i].ToString();
                febraFalseListTextBoxes[i].Text = febra.PFn[i].ToString();
            }
            //datele pt oboseala

            for (int i = 0; i < Oboseala.NoData; i++)
            {
                obosealaTrueTextBoxes[i].Text = oboseala.POd[i].ToString();
                obosealaFalseTextBoxes[i].Text = oboseala.POn[i].ToString();
            }
            //datele pt anorexie

            for (int i = 0; i < Anorexie.NoData; i++)
            {
                anorexieTrueTextBoxes[i].Text = anorexie.PXd[i].ToString();
                anorexieFalseTextBoxes[i].Text = anorexie.PXn[i].ToString();
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
