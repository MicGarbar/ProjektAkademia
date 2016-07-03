using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektAkaChasz
{
    public enum Adds { Subtitles, Lector, None, 
                       Grammy_Award, MTV_Award, Billboard_Award, 
                       Age_Of_3, Age_Of_12, Age_Of_18 }

    public class Item : IComparable<Item>
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public Adds Add { get; set; }

        public Item(string name, string author, int year, string type, double price, Adds add)
        {
            this.Name = name;
            this.Author = author;
            this.Year = year;
            this.Type = type;
            this.Price = price;
            this.Add = add;
        }

        public virtual void IncreasePrice(double multiplier)
        {
            this.Price *= multiplier;
        }

        public int CompareTo(Item other)
        {
            return this.Price.CompareTo(other.Price);
        }
    }
}
