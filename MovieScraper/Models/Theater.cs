using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieScraper
{
    internal class Theater
    {
        public string TheaterURL;
        public string Name;
        public string Location;

        public Theater(string TheaterURL, string Name, string Location)
        {
            this.TheaterURL = TheaterURL;
            this.Name = Name;
            this.Location = Location;
        }
    }
}
