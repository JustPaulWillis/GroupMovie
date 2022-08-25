using AngleSharp;
using AngleSharp.Dom;
using PuppeteerSharp;

namespace MovieScraper
{
    
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                Scrape();
                await Task.Delay(15000, stoppingToken);
            }
        }

        protected static async void Scrape()
        {
            List<Theater> theaterList = new List<Theater>();
            List<Showtime> showtimeList = new List<Showtime>();
            List<Movie> movieList = new List<Movie>();

            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            var page = await browser.NewPageAsync();
            await page.GoToAsync("https://www.fandango.com/85306_movietimes");

            var content = await page.GetContentAsync();

            var context = BrowsingContext.New(Configuration.Default);
            var document = await context.OpenAsync(req => req.Content(content));
            await page.CloseAsync();

            //get list of theaters from given location url
            var theaters = document.QuerySelectorAll("li.fd-theater").ToList();
            
            //for each theater
            foreach (var t in theaters)
            {
                //Add theater name, url, and address to list of theater objects
                var header = t.QuerySelector("div.fd-theater__header");
                var name = header.QuerySelector("h3.fd-theater__name");
                var theaterName = name.QuerySelector("a");
                var theaterURL = theaterName.GetAttribute("href");//.Split('\\')[0];
                var theaterAddress = t.QuerySelector("div.fd-theater__address-wrap").TextContent.Replace("\n", "").Replace("\r", "").Replace("  ", "").Replace(",", ", ");
                theaterList.Add(new Theater(theaterURL, theaterName.Text().Trim(), theaterAddress));

                //Add each showtime at the theater to the showtimes list
                var movies = t.QuerySelectorAll("li.fd-movie");
                foreach (var movie in movies)
                {   
                    var movieURL = movie.QuerySelector("a.fd-movie__link").GetAttribute("href");
                    var showtimes = movie.QuerySelectorAll("a.showtime-btn--available");
                    foreach (var showtime in showtimes)
                    {
                        var ticketURL = showtime.GetAttribute("href");
                        var date = document.QuerySelector("li.theater-calendar__item--selected").GetAttribute("data-show-time-date");
                        var time = showtime.Text().Trim();

                        var ticketingPage = await browser.NewPageAsync();
                        await ticketingPage.GoToAsync(ticketURL);
                        var ticketingContent = await page.GetContentAsync();
                        var browsingContext = BrowsingContext.New(Configuration.Default);
                        var ticketingDocument = await context.OpenAsync(req => req.Content(content));
                        await ticketingPage.CloseAsync();


                        showtimeList.Add(new Showtime(movieURL, ticketURL, date, time, theaterURL));
                    }

                    var thisMovie = new Movie(movieURL);
                    if (!movieList.Contains(thisMovie))
                    {
                        movieList.Add(thisMovie);
                    }
                }
            }

            foreach (var t in theaterList)
                Console.WriteLine(t.Name + ", " + t.Location + ", " + t.TheaterURL);

            foreach (var s in showtimeList)
                Console.WriteLine(s.MovieURL + ", " + s.Date + " " + s.Time + ", " + s.Theater + ", " + s.TicketURL);



            Console.WriteLine("########################");
            Console.ReadLine();

            await browser.CloseAsync();
        }
        protected static async void GetSeats()
        {
            List<Theater> theaterList = new List<Theater>();
            List<Showtime> showtimeList = new List<Showtime>();
            List<Movie> movieList = new List<Movie>();

            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            var page = await browser.NewPageAsync();
            await page.GoToAsync("https://www.fandango.com/85306_movietimes");

            var content = await page.GetContentAsync();

            var context = BrowsingContext.New(Configuration.Default);
            var document = await context.OpenAsync(req => req.Content(content));
            await page.CloseAsync();

            //get list of theaters from given location url
            var theaters = document.QuerySelectorAll("li.fd-theater").ToList();
        }
    }
}