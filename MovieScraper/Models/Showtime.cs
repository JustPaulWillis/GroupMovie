using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieScraper
{
    internal class Showtime
    {
        public string MovieURL;
        public string TicketURL;
        public string Date;
        public string Time;
        public string Theater;
        //public System.Collections.IDictionary AvailableSeats;

        public Showtime(string MovieURL, string TicketURL, string Date, string Time, string Theater) //, Dictionary<string, int> Seats)
        {
            this.MovieURL = MovieURL;
            this.TicketURL = TicketURL;
            this.Date = Date;
            this.Time = Time;  
            this.Theater = Theater;
            //AvailableSeats = Seats;
        } 
    }
}
