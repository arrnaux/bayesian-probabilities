using System.Collections.Generic;

namespace DataModel
{
    public interface Node
    {
        int GetNoOfParents();
        IList<string> GetListOfParents();
        //adaugare lista generica de probabilitati(true si false)? poate
    }
}