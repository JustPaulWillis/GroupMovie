using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieScraper
{
    internal class Movie
    {
        public string MovieURL;
        //rating
        //other metadata

        public Movie(string MovieURL)
        {
            this.MovieURL = MovieURL;
           
        }
    }
}
