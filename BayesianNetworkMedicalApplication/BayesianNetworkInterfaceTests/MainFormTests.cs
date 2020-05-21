using Microsoft.VisualStudio.TestTools.UnitTesting;
using BayesianNetworkInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;

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
            form._evidenceNode = form._affections[2];
            form._affections[0]._status = Status.True;
            form._affections[1]._status = Status.False;
            form._affections[2]._status = Status.Na;
            form._affections[3]._status = Status.True;
            form._affections[4]._status = Status.Unspecified;
            double val = form.ComputeEvidenceNodeProbability();
            Assert.AreEqual(0.875, val, 0.01);
        }

        [TestMethod]
        public void ComputeEvidenceNodeProbabilityTest_2()
        {
            MainForm form = new MainForm();
            form.SetMatrixValues();
            form._evidenceNode = form._affections[3];
            form._affections[0]._status = Status.False;
            form._affections[1]._status = Status.True;
            form._affections[2]._status = Status.Unspecified;
            form._affections[3]._status = Status.Na;
            form._affections[4]._status = Status.Unspecified;
            double val = form.ComputeEvidenceNodeProbability();
            Assert.AreEqual(0.3, val, 0.01);
        }
    }
}