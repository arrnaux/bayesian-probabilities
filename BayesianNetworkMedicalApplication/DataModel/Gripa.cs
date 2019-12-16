using System.Collections.Generic;

namespace DataModel
{
    public class Gripa : Node
    {
        public double PGd { get; set; } //Prob gripa da
        public double PGn { get; set; } //Prob gripa nu


        public Gripa()
        {
            PGd = 0.1;
            PGn = 1 - PGd;
        }

        public Gripa(double pGd)
        {
            PGd = pGd;
            PGn = 1 - pGd;
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
