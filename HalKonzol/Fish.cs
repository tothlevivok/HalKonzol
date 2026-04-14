using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalKonzol
{
    public class Fish
    {
        public string name { get; set; }
        public double weight { get; set; }
        public int userId { get; set; }

        public Fish(string name, double weight)
        {
            this.name = name;
            this.weight = weight;
        }
    }
}