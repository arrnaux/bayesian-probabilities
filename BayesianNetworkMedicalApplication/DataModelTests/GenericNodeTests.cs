﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Tests
{
    [TestClass]
    public class GenericNodeTests
    {
        [TestMethod]
        public void ComputeProbabilityConsideringParentsTest_1()
        {
            GenericNode node = new GenericNode("Gripa");
            double val = node.ComputeProbabilityConsideringParents();
            Assert.AreEqual(-1, val);
        }

        [TestMethod]
        public void ComputeProbabilityConsideringParentsTest_2()
        {
            GenericNode node = new GenericNode("Gripa");
            node.LoadProabilitiesFromMatrix();
            node.SetProbabilities(true);
            node.Status = Status.True;
            double val = node.ComputeProbabilityConsideringParents();
            Assert.AreEqual(0.1, val);
        }

        [TestMethod]
        public void ComputeProbabilityConsideringParentsTest_3()
        {
            GenericNode node = new GenericNode("Gripa");
            node.LoadProabilitiesFromMatrix();
            node.SetProbabilities(true);
            node.Status = Status.False;
            double val = node.ComputeProbabilityConsideringParents();
            Assert.AreEqual(0.9, val);
        }

        [TestMethod]
        public void ComputeProbabilityConsideringParentsTest_4()
        {
            GenericNode node = new GenericNode("Febra");
            node.LoadProabilitiesFromMatrix();
            node.SetProbabilities(true);
            node.Status = Status.True;
            node.ListOfParents.Add(new GenericNode("Gripa"));
            node.ListOfParents.Add(new GenericNode("Abces"));
            node.ListOfParents.ElementAt(0).Status = Status.True;
            node.ListOfParents.ElementAt(1).Status = Status.False;
            double val = node.ComputeProbabilityConsideringParents();
            Assert.AreEqual(0.7, val);
        }

        [TestMethod]
        public void ComputeProbabilityConsideringParentsTest_5()
        {
            GenericNode node = new GenericNode("Febra");
            node.LoadProabilitiesFromMatrix();
            node.SetProbabilities(true);
            node.Status = Status.False;
            node.ListOfParents.Add(new GenericNode("Gripa"));
            node.ListOfParents.Add(new GenericNode("Abces"));
            node.ListOfParents.ElementAt(0).Status = Status.False;
            node.ListOfParents.ElementAt(1).Status = Status.True;
            double val = node.ComputeProbabilityConsideringParents();
            Assert.AreEqual(0.75, val);
        }

        [TestMethod]
        public void ComputeProbabilityConsideringParentsTest_6()
        {
            GenericNode node = new GenericNode("Oboseala");
            node.LoadProabilitiesFromMatrix();
            node.SetProbabilities(true);
            node.Status = Status.True;
            node.ListOfParents.Add(new GenericNode("Febra"));
            node.ListOfParents.ElementAt(0).Status = Status.True;
            double val = node.ComputeProbabilityConsideringParents();
            Assert.AreEqual(0.6, val);
        }

        [TestMethod]
        public void ToDigitTest_1()
        {
            int val = GenericNode.ToDigit(true);
            Assert.AreEqual(1, val);
        }

        [TestMethod]
        public void ToDigitTest_2()
        {
            int val = GenericNode.ToDigit(false);
            Assert.AreEqual(0, val);
        }
    }
}