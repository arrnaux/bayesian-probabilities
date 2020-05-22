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
        public List<double> textBoxValues;
        public Memento(List<double> textBoxValues)
        {
            this.textBoxValues=new List<double>(textBoxValues);
        }
    }
}