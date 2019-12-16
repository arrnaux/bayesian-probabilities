using System.Collections.Generic;

namespace DataModel
{
    public class Oboseala : Node
    {
        public const int NoData = 2;

        public double[] POd { get; set; }
        public double[] POn { get; set; }
        // index 0 -> febra da
        // index 1 -> febra nu

        public Oboseala()
        {
            POd = new double[NoData] { 0.6,0.2 };
            POn = new double[NoData];
            for (int i = 0; i < NoData; i++)
            {
                POn[i] = 1 - POd[i];
            }
        }

        public Oboseala(double[] cPOd)
        {
            if (cPOd != null && cPOd.Length == NoData)
            {
                POd = new double[NoData];
                POn = new double[NoData];
                for (int i = 0; i < NoData; i++)
                {
                    POd[i] = cPOd[i];
                    POn[i] = 1 - POd[i];
                }
            }
        }

        public int GetNoOfParents()
        {
            return 1;
        }

        public IList<string> GetListOfParents()
        {
            return new List<string>()
            {
                "Febra"
            };
        }
    }
}
