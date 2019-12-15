﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Anorexie
    {
        private const int NoData = 2;

        public double[] PXd { get; set; }
        public double[] PXn { get; set; }
        // index 0 -> febra da
        // index 1 -> febra nu

        public Anorexie()
        {
            PXd = new double[NoData] { 0.5, 0.5 };
            PXn = new double[NoData];
            for (int i = 0; i < NoData; i++)
            {
                PXn[i] = 1 - PXd[i];
            }
        }

        public Anorexie(double[] cPXd)
        {
            if (cPXd != null && cPXd.Length == NoData)
            {
                PXd = new double[NoData];
                PXn = new double[NoData];
                for (int i = 0; i < NoData; i++)
                {
                    PXd[i] = cPXd[i];
                    PXn[i] = 1 - PXd[i];
                }
            }
        }
    }
}
