/**************************************************************************
 *                                                                        *
 *  File:        Caretaker.cs                                             *
 *  Copyright:   (c) 2020 Gabriel Răileanu, Nicolae Boca, Ștefan Ignătescu*
 *  Website:     https://github.com/arrnaux/bayesian-probabilities        *
 *  Description: Generates file information headers.                      *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/

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