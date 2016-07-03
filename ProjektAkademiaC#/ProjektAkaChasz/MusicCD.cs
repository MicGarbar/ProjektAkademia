﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektAkaChasz
{
    public class MusicCD : Item
    {
        public MusicCD(string name, string author, int year, string type, double price, Adds add) 
            : base(name, author, year, type, price, add)
        {}

        public override void IncreasePrice(double multiplier)
        {
            base.IncreasePrice(multiplier);
            this.Price += multiplier;
            this.Price = Math.Round(this.Price, 2);
        }
    }
}
