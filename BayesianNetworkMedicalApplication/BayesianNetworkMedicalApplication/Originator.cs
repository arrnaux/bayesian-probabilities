using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataModel;

namespace BayesianNetworkInterface
{
    /// <summary>
    /// Implements the memento pattern.
    /// </summary>
    public class Originator
    {
        public List<double> TextBoxValues { get; set; }

        public Originator()
        {
            TextBoxValues = new List<double>();
        }


        public void SetTextBoxValues(List<TextBox> list)
        {
            TextBoxValues.Clear();
            foreach (var textBox in list)
            {
                try
                {
                    TextBoxValues.Add(double.Parse(textBox.Text));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public Memento SaveMemento()
        {
            return new Memento(TextBoxValues);
        }

        public void RestoreMemento(Memento memento)
        {
            if (memento == null)
            {
                throw new Exception("Nu exista o stare care sa poata fi restaurata.");
            }

            TextBoxValues = memento.TextBoxValues;
        }
    }
}