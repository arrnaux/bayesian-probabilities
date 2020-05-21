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
            form.EvidenceNode = form.Affections[2];
            form.Affections[0].Status = Status.True;
            form.Affections[1].Status = Status.False;
            form.Affections[2].Status = Status.Na;
            form.Affections[3].Status = Status.True;
            form.Affections[4].Status = Status.Unspecified;
            double val = form.ComputeEvidenceNodeProbability();
            Assert.AreEqual(0.875, val, 0.01);
        }

        [TestMethod]
        public void ComputeEvidenceNodeProbabilityTest_2()
        {
            MainForm form = new MainForm();
            form.SetMatrixValues();
            form.EvidenceNode = form.Affections[3];
            form.Affections[0].Status = Status.False;
            form.Affections[1].Status = Status.True;
            form.Affections[2].Status = Status.Unspecified;
            form.Affections[3].Status = Status.Na;
            form.Affections[4].Status = Status.Unspecified;
            double val = form.ComputeEvidenceNodeProbability();
            Assert.AreEqual(0.3, val, 0.01);
        }
    }
}