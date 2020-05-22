using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataModel;

namespace BayesianNetworkInterface
{
    public class Originator
    {
        public List<double> textBoxValues { get; set; }
        public Originator()
        {
            textBoxValues = new List<double>();
        }

        
        public void SetTextBoxValues(List<TextBox> list)
        {
            textBoxValues.Clear();
            foreach (var textBox in list)
            {
                try
                {
                    textBoxValues.Add(Double.Parse(textBox.Text));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public Memento SaveMemento()
        {
            MessageBox.Show("Saving system state");
            return new Memento(textBoxValues);
        }

        public void RestoreMemento(Memento memento)
        {
            textBoxValues = memento.textBoxValues;
            MessageBox.Show("Restoring system state");
            //restore data;
        }
    }
}
