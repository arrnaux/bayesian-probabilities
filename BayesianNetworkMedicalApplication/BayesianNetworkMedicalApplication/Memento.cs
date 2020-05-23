using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataModel;

namespace BayesianNetworkInterface
{
    public class Memento
    {
        public List<double> TextBoxValues { get; set; }
        public Memento(List<double> textBoxValues)
        {
            this.TextBoxValues=new List<double>(textBoxValues);
        }
    }
}