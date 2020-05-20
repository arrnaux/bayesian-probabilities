using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            node._status = Status.True;            
            double val = node.ComputeProbabilityConsideringParents();
            Assert.AreEqual(0.1, val);
        }

        [TestMethod]
        public void ComputeProbabilityConsideringParentsTest_3()
        {
            GenericNode node = new GenericNode("Gripa");
            node.LoadProabilitiesFromMatrix();
            node.SetProbabilities(true);
            node._status = Status.False;            
            double val = node.ComputeProbabilityConsideringParents();
            Assert.AreEqual(0.9, val);
        }

        [TestMethod]
        public void ComputeProbabilityConsideringParentsTest_4()
        {
            GenericNode node = new GenericNode("Febra");
            node.LoadProabilitiesFromMatrix();
            node.SetProbabilities(true);
            node._status = Status.True;
            node._listOfParents.Add(new GenericNode("Gripa"));
            node._listOfParents.Add(new GenericNode("Abces"));
            node._listOfParents.ElementAt(0)._status = Status.True;
            node._listOfParents.ElementAt(1)._status = Status.False;
            double val = node.ComputeProbabilityConsideringParents();
            Assert.AreEqual(0.7, val);
        }

        [TestMethod]
        public void ComputeProbabilityConsideringParentsTest_5()
        {
            GenericNode node = new GenericNode("Oboseala");
            node.LoadProabilitiesFromMatrix();
            node.SetProbabilities(true);
            node._status = Status.True;
            node._listOfParents.Add(new GenericNode("Febra"));
            node._listOfParents.ElementAt(0)._status = Status.True;
            double val = node.ComputeProbabilityConsideringParents();
            Assert.AreEqual(0.6, val);
        }
    }
}