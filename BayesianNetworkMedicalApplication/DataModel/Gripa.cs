namespace DataModel
{
    public class Gripa
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

    }
}
