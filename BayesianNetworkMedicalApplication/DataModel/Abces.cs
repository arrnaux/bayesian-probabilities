﻿using System.Collections.Generic;

namespace DataModel
{
    public class Abces: Node
    {
        public double PAd { get; set; } //Prob abces da
        public double PAn { get; set; } //Prob abces nu

        public Abces()
        {
            PAd = 0.05;
            PAn = 1 - PAd;
        }

        public Abces(double pAd)
        {
            PAd = pAd;
            PAn = 1 - pAd;
        }

        public int GetNoOfParents()
        {
            return 0;
        }

        public IList<string> GetListOfParents()
        {
            return null;
        }
    }
}
