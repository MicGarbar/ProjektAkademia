using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektAkaChasz
{
    public class MovieDVD : Item
    {
        public MovieDVD(string name, string author, int year, string type, double price, Adds add) 
            : base(name, author, year, type, price, add)
        {}

        public override void IncreasePrice(double multiplier)
        {
            base.IncreasePrice(multiplier);
            this.Price *= 0.95;
            this.Price = Math.Round(this.Price, 2);
        }
    }
}
