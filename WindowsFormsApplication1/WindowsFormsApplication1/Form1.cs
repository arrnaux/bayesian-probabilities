using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class dro : Form
    {
        // True case is indexed on index 1

        // TODO: read values from file
        double[] labValues;
        int noValues;

        public dro()
        {
            InitializeComponent();
            labValues = new double[] {-1,
                0.1, 0.9, 0.05, 0.95,
            0.8, 0.2, 0.7, 0.3,
            0.25, 0.75, 0.05, 0.95,
            0.6, 0.4, 0.2, 0.8,
            0.5, 0.5, 0.1, 0.9
         };
            noValues = labValues.Length;
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            resultBox.Text = noValues.ToString();
            for (int i = 1; i < noValues; ++i)
            {
                String name = "textBox" + i;
                TextBox currentTextBox = (TextBox)Controls.Find(name, true)[0];
                currentTextBox.Text = labValues[i].ToString();
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
