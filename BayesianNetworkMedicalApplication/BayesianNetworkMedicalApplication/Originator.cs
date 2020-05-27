/**************************************************************************
 *                                                                        *
 *  File:        Originator.cs                                            *
 *  Copyright:   (c) 2020 Gabriel Răileanu, Nicolae Boca, Ștefan Ignătescu*
 *  Website:     https://github.com/arrnaux/bayesian-probabilities        *
 *  Description: Implements the memento pattern.                          *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/

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