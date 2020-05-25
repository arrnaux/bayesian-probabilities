/**************************************************************************
 *                                                                        *
 *  File:        MainFormTests.cs                                         *
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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using BayesianNetworkInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using System.Windows.Forms;

namespace BayesianNetworkInterface.Tests
{
    [TestClass]
    public class MainFormTests
    {
        [TestMethod]
        public void ComputeEvidenceNodeProbabilityTest_1()
        {
            MainForm form = new MainForm();
            form.SetMatrixValues();
            form.EvidenceNode = form.Affections[2];
            form.Affections[0].Status = Status.True;
            form.Affections[1].Status = Status.False;
            form.Affections[2].Status = Status.Na;
            form.Affections[3].Status = Status.True;
            form.Affections[4].Status = Status.Unspecified;
            double val = form.ComputeEvidenceNodeProbability() * 100;
            Assert.AreEqual(87.5, val, 0.01);
        }

        [TestMethod]
        public void ComputeEvidenceNodeProbabilityTest_2()
        {
            MainForm form = new MainForm();
            form.SetMatrixValues();
            form.EvidenceNode = form.Affections[0];
            form.Affections[0].Status = Status.Na;
            form.Affections[1].Status = Status.Unspecified;
            form.Affections[2].Status = Status.Unspecified;
            form.Affections[3].Status = Status.Unspecified;
            form.Affections[4].Status = Status.Unspecified;
            double val = form.ComputeEvidenceNodeProbability() * 100;
            Assert.AreEqual(10, val);
        }

        [TestMethod]
        public void ComputeEvidenceNodeProbabilityTest_3()
        {
            MainForm form = new MainForm();
            form.SetMatrixValues();
            form.EvidenceNode = form.Affections[0];
            form.Affections[0].Status = Status.Na;
            form.Affections[1].Status = Status.True;
            form.Affections[2].Status = Status.False;
            form.Affections[3].Status = Status.True;
            form.Affections[4].Status = Status.False;
            double val = form.ComputeEvidenceNodeProbability() * 100;
            Assert.AreEqual(2.877, val, 0.001);
        }

        [TestMethod]
        public void ComputeEvidenceNodeProbabilityTest_4()
        {
            MainForm form = new MainForm();
            form.SetMatrixValues();
            form.EvidenceNode = form.Affections[3];
            form.Affections[0].Status = Status.False;
            form.Affections[1].Status = Status.True;
            form.Affections[2].Status = Status.Unspecified;
            form.Affections[3].Status = Status.Na;
            form.Affections[4].Status = Status.Unspecified;
            double val = form.ComputeEvidenceNodeProbability() * 100;
            Assert.AreEqual(30, val);
        }

        [TestMethod]
        public void ComputeEvidenceNodeProbabilityTest_5()
        {
            MainForm form = new MainForm();
            form.SetMatrixValues();
            form.EvidenceNode = form.Affections[0];
            form.Affections[0].Status = Status.Na;
            form.Affections[1].Status = Status.True;
            form.Affections[2].Status = Status.True;
            form.Affections[3].Status = Status.True;
            form.Affections[4].Status = Status.True;
            double val = form.ComputeEvidenceNodeProbability() * 100;
            Assert.AreEqual(26.229, val, 0.001);
        }

        [TestMethod]
        public void ComputeEvidenceNodeProbabilityTest_6()
        {
            MainForm form = new MainForm();
            form.SetMatrixValues();
            form.EvidenceNode = form.Affections[1];
            form.Affections[0].Status = Status.False;
            form.Affections[1].Status = Status.Na;
            form.Affections[2].Status = Status.False;
            form.Affections[3].Status = Status.False;
            form.Affections[4].Status = Status.False;
            double val = form.ComputeEvidenceNodeProbability() * 100;
            Assert.AreEqual(3.989, val, 0.001);
        }

        [TestMethod]
        public void ComputeEvidenceNodeProbabilityTest_7()
        {
            MainForm form = new MainForm();
            form.SetMatrixValues();
            form.EvidenceNode = form.Affections[4];
            form.Affections[0].Status = Status.Unspecified;
            form.Affections[1].Status = Status.Unspecified;
            form.Affections[2].Status = Status.Unspecified;
            form.Affections[3].Status = Status.Unspecified;
            form.Affections[4].Status = Status.Na;
            double val = form.ComputeEvidenceNodeProbability() * 100;
            Assert.AreEqual(14.98, val, 0.001);
        }

        [TestMethod]
        public void CheckValidNumberTest_1()
        {
            MainForm form = new MainForm();
            double val = form.CheckValidNumber("2");
            Assert.AreEqual(-1, val);
        }

        [TestMethod]
        public void CheckValidNumberTest_2()
        {
            MainForm form = new MainForm();
            double val = form.CheckValidNumber("-2");
            Assert.AreEqual(-1, val);
        }

        [TestMethod]
        public void CheckValidNumberTest_3()
        {
            MainForm form = new MainForm();
            double val = form.CheckValidNumber("~abc!");
            Assert.AreEqual(-1, val);
        }

        [TestMethod]
        public void CheckValidNumberTest_4()
        {
            MainForm form = new MainForm();
            double val = form.CheckValidNumber("0.3d");
            Assert.AreEqual(-1, val);
        }

        [TestMethod]
        public void CheckValidNumberTest_5()
        {
            MainForm form = new MainForm();
            double val = form.CheckValidNumber("0.5");
            Assert.AreEqual(0.5, val);
        }

        [TestMethod]
        public void MementoTest_1()
        {
            Originator originator = new Originator();
            Caretaker caretaker = new Caretaker();
            List<TextBox> list = new List<TextBox>();
            for(int i=1; i<5; i++)
            {
                TextBox textBox = new TextBox();
                textBox.Text = i.ToString();
                list.Add(textBox);
            }            
            originator.SetTextBoxValues(list);
            caretaker.Memento = originator.SaveMemento();
            list[0].Text = "10";
            originator.SetTextBoxValues(list);
            Assert.AreEqual(10, originator.TextBoxValues[0]);
        }

        [TestMethod]
        public void MementoTest_2()
        {
            Originator originator = new Originator();
            Caretaker caretaker = new Caretaker();
            List<TextBox> list = new List<TextBox>();
            for (int i = 1; i < 5; i++)
            {
                TextBox textBox = new TextBox();
                textBox.Text = i.ToString();
                list.Add(textBox);
            }
            originator.SetTextBoxValues(list);
            caretaker.Memento = originator.SaveMemento();
            list[0].Text = "10";
            originator.SetTextBoxValues(list);
            originator.RestoreMemento(caretaker.Memento);
            Assert.AreEqual(1, originator.TextBoxValues[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void MementoTest_3()
        {
            Originator originator = new Originator();
            Caretaker caretaker = new Caretaker();
            List<TextBox> list = new List<TextBox>();
            for (int i = 1; i < 5; i++)
            {
                TextBox textBox = new TextBox();
                textBox.Text = i.ToString();
                list.Add(textBox);
            }
            originator.SetTextBoxValues(list);
            originator.RestoreMemento(caretaker.Memento);
        }
    }
}