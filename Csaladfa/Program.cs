using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csaladfa
{
    class Program
    {
        static void Main(string[] args)
        {
            Csaladtag ős = new Csaladtag();
            ős.Családfagenerálás_Vezetéknevek_kifogyásáig();
        }
    }
}
