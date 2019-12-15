using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataModel
{
    public class Febra
    {
        public const int NoData = 4;
        
        public double[] PFd { get; set; }
        public double[] PFn { get; set; }
        // index 0 -> gripa da, abces da
        // index 1 -> gripa da, abces nu
        // index 2 -> gripa nu, abces da
        // index 3 -> gripa nu, abces nu

        public Febra()
        {
            PFd = new double[NoData] {0.8, 0.7, 0.25, 0.05};
            PFn=new double[NoData];
            for (int i = 0; i < NoData; i++)
            {
                PFn[i] = 1 - PFd[i];
            }
        }

        public Febra(double[] cPFd)
        {
            if (cPFd != null && cPFd.Length == NoData)
            {
                PFd = new double[NoData];
                PFn = new double[NoData];
                for (int i = 0; i < NoData; i++)
                {
                    PFd[i] = cPFd[i];
                    PFn[i] = 1 - PFd[i];
                }
            }
        }
    }
}
