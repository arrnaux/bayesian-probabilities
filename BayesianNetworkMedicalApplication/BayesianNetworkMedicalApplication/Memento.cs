using System.Collections.Generic;

namespace BayesianNetworkInterface
{
    /// <summary>
    /// Memento pattern. The current class represents a state of the network at a moment of time.
    /// </summary>
    public class Memento
    {
        public List<double> TextBoxValues { get; set; }

        public Memento(List<double> textBoxValues)
        {
            TextBoxValues = new List<double>(textBoxValues);
        }
    }
}