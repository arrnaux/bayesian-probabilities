﻿/**************************************************************************
 *                                                                        *
 *  File:        GenericNode.cs                                           *
 *  Copyright:   (c) 2020 Gabriel Răileanu, Nicolae Boca, Ștefan Ignătescu*
 *  Website:     https://github.com/arrnaux/bayesian-probabilities        *
 *  Description: In this file is designed the structure and methods for a *
 *               GenericNode.                                             *
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DataModel
{
    static class Constants
    {
        public const int MaxNoParents = 3;
    }

    public enum Status
    {
        True,
        False,
        Unspecified,
        Na
    }

    /// <summary>
    /// A generic node representing an affection.
    /// </summary>
    public class GenericNode : IEquatable<GenericNode>
    {
        public List<GenericNode> ListOfParents { get; set; }

        public string Name { get; set; }
        public List<TextBox> TrueProbabilityTextBoxes { get; set; }
        public List<TextBox> FalseProbabilityTextBoxes { get; set; }

        public Status Status { get; set; }

        /// <summary>
        /// The structure is:
        /// FIRST PARENT    SECOND PARENT   NODE YES    NODE NO
        /// YES             YES             value       value
        /// YES             NO              value       value
        /// NO              YES             value       value
        /// NO              NO              value       value
        /// </summary>
        private double[,] _probabilities;

        public GenericNode(string name)
        {
            Name = name;
            ListOfParents = new List<GenericNode>();
            TrueProbabilityTextBoxes = new List<TextBox>();
            FalseProbabilityTextBoxes = new List<TextBox>(TrueProbabilityTextBoxes.Count);
            Status = Status.Na;
            _probabilities = new double[(int) Math.Pow(2, Constants.MaxNoParents), 2];
        }

        public void LoadProbabilitiesFromMatrix()
        {
            for (var i = 0; i < TrueProbabilityTextBoxes.Count; i++)
            {
                TrueProbabilityTextBoxes[i].Text = _probabilities[i, 0].ToString();
                FalseProbabilityTextBoxes[i].Text = _probabilities[i, 1].ToString();
            }
        }

        public void LoadProbabilitiesFromGui()
        {
            for (var i = 0; i < TrueProbabilityTextBoxes.Count; i++)
            {
                _probabilities[i, 0] = double.Parse(TrueProbabilityTextBoxes[i].Text);
                _probabilities[i, 1] = double.Parse(FalseProbabilityTextBoxes[i].Text);
            }
        }

        /// <summary>
        /// Loads probabilities value from a file. 
        /// </summary>
        /// <param name="labValue">If the parameters is true, then the default-laboratory values will be loaded.
        /// Otherwise, the default file from the project will be used for load the data.</param>
        public void SetProbabilities(bool labValue)
        {
            const string basicPath = @"../../../probabilities/";
            try
            {
                var fileName = (labValue)
                    ? Path.Combine(basicPath, "laborator", Name.ToLower() + ".txt")
                    : Path.Combine(basicPath, "date", Name.ToLower() + ".txt");

                CultureInfo ci = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;

                LoadProbabilitiesFromFile(fileName);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Loads probabilities from a specific file.
        /// </summary>
        /// <param name="fileName"></param>
        private void LoadProbabilitiesFromFile(string fileName)
        {
            var fileStream = File.OpenRead(fileName);
            var streamReader = new StreamReader(fileStream, Encoding.UTF8);
            var content = streamReader.ReadToEnd();
            var values = content.Split(' ', '\n');
            for (var i = 0; i < values.Length; i++)
            {
                _probabilities[i, 0] = double.Parse(values[i]);
                _probabilities[i, 1] = 1 - double.Parse(values[i]);
            }
        }

        /// <summary>
        /// Compute a probability for a node.
        /// </summary>
        /// <returns>A probability of a node with a value (T/F), considering its parents and their values (T/F)</returns>
        public double ComputeProbabilityConsideringParents()
        {
            // The probability of a node is:
            // the probability of that node if has no parents
            // the probability conditioned by the parents if it has any
            if (0 == ListOfParents.Count)
            {
                switch (Status)
                {
                    case Status.True:
                        return _probabilities[0, 0];
                    case Status.False:
                        return _probabilities[0, 1];
                }
            }
            else
            {
                // The line in matrix is determined by a combination between TRUE/FALSE values of the parents.
                // The column is determined by TRUE/FALSE status of current node.
                // In case of TRUE, the column is 0, otherwise 1.

                // By default, the variable is set to TRUE.
                int column = 0;
                if (Status == Status.False)
                {
                    column = 1;
                }

                bool[] correspondingValues = new bool[ListOfParents.Count];
                for (int i = 0; i < ListOfParents.Count; ++i)
                {
                    GenericNode parent = ListOfParents.ElementAt(i);
                    if (parent.Status == Status.False)
                    {
                        correspondingValues[i] = false;
                    }
                    else if (parent.Status == Status.True)
                    {
                        correspondingValues[i] = true;
                    }
                }

                for (int i = 0; i < ListOfParents.Count; i++)
                {
                    correspondingValues[i] = correspondingValues[i] ^ true;
                }

                int val = 0;
                for (int i = 0; i < ListOfParents.Count; ++i)
                {
                    val = (val << 1) | ToDigit(correspondingValues[i]);
                }

                return _probabilities[val, column];
            }

            return -1;
        }

        public static int ToDigit(bool b)
        {
            return b ? 1 : 0;
        }

        public bool Equals(GenericNode other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(ListOfParents, other.ListOfParents) &&
                   Equals(TrueProbabilityTextBoxes, other.TrueProbabilityTextBoxes) &&
                   Equals(FalseProbabilityTextBoxes, other.FalseProbabilityTextBoxes) &&
                   Equals(_probabilities, other._probabilities) &&
                   Name == other.Name && Status == other.Status;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GenericNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (ListOfParents != null ? ListOfParents.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^
                           (TrueProbabilityTextBoxes != null ? TrueProbabilityTextBoxes.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^
                           (FalseProbabilityTextBoxes != null ? FalseProbabilityTextBoxes.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_probabilities != null ? _probabilities.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) Status;
                return hashCode;
            }
        }
    }
}