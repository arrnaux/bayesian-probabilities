/**************************************************************************
 *                                                                        *
 *  File:        Memento.cs                                               *
 *  Copyright:   (c) 2020 Gabriel Răileanu, Nicolae Boca, Ștefan Ignătescu*
 *  Website:     https://github.com/arrnaux/bayesian-probabilities        *
 *  Description: Memento pattern. The current class represents a state of *
 *               the network at a moment of time.                         *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/

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