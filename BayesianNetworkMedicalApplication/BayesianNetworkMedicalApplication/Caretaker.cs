using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianNetworkInterface
{
    /// <summary>
    /// Part of memento pattern.
    /// </summary>
    public class Caretaker
    {
        public Memento Memento { get; set; }
    }
}