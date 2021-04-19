using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace web_respons_by_Task_and_parrels
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = Enumerable.Range(100, 20000);
            var ParallelQuery =
                from number in numbers.AsParallel()
                where number % 10 == 0
                select number;
            Console.WriteLine("process resulte in ParallelQuery");
            ParallelQuery.ForAll((number) => Print(number));

            var ParallelQuery1 =
              (from number in numbers.AsParallel()
               where number % 10 == 0
               select number).ToArray();
            foreach (var number in ParallelQuery1)
            {
                Print(number);
            }
            

            var ParallelQuery3 =
            numbers.AsParallel().Where(n => n % 10 == 0).Select(n => n);
            ParallelQuery3.ForAll((number) => Print(number));

            Console.Clear();
            Parallel_Foreach();
            Parallel_Foreach();
            Parallel_Invoke();
            Parallel_For();
            DownloadStringsn();
            Async_Downloading_by_Task();
            Console.ReadLine();
        }

        private static void Print(int number)
        {

            Console.WriteLine(number + "   " + Task.CurrentId);
        }

        private static void Parallel_For()
        {
            List<string> htmls = GetListHtml();
            Console.WriteLine("Async : Parallel For\n");
            Stopwatch t = Stopwatch.StartNew();
            List<Action<string>> la = new List<Action<string>>();
            for (int i = 0; i < htmls.Count; i++)
            {
                la.Add(new Action<string>(DownloadHtml));
            }
            Parallel.For(0, la.Count, i => la[i](htmls[i]));
            t.Stop();
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"Total time elapsed Parallel For is: {t.ElapsedMilliseconds}");
            Console.WriteLine("--------------------------------------------------------------\n\n");
        }

        private static void Parallel_Invoke()
        {
            Console.WriteLine("Async : Parallel invoke\n");
            List<string> htmls = GetListHtml();
            Stopwatch t = Stopwatch.StartNew();
            Parallel.Invoke(() => DownloadHtml(htmls[0]), () => DownloadHtml(htmls[1]), () => DownloadHtml(htmls[2]), () => DownloadHtml(htmls[3]), () => DownloadHtml(htmls[4]),
                () => DownloadHtml(htmls[5]), () => DownloadHtml(htmls[6]), () => DownloadHtml(htmls[7]), () => DownloadHtml(htmls[8]), () => DownloadHtml(htmls[9]));
            t.Stop();
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"Total time elapsed Parallel Invoke is: {t.ElapsedMilliseconds}");
            Console.WriteLine("--------------------------------------------------------------\n\n");
        }

        private static void Parallel_Foreach()
        {
            Stopwatch t = Stopwatch.StartNew();
            List<string> Htmls = GetListHtml();
            Console.WriteLine("Async : Parallel For each");
            List<Action<string>> la = new List<Action<string>>();
            for (int i = 0; i < Htmls.Count; i++)
            {
                la.Add(new Action<string>(DownloadHtml));

            }
            int count = -1;

            Parallel.ForEach(la, item =>
            {

                item(Htmls[Interlocked.Increment(ref count)]);

            });
            t.Stop();
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"Total time elapsed Parallel ForEach is: {t.ElapsedMilliseconds}");
            Console.WriteLine("--------------------------------------------------------------\n\n");
        }

        private static void DownloadHtml(string html)
        {

            Stopwatch s = new Stopwatch();
            s.Start();
            WebClient webClient = new WebClient();
            string web = webClient.DownloadString(html);
            s.Stop();
            Console.WriteLine($"size: {web.Length}, {html}, time elapsed: {s.ElapsedMilliseconds}");
        }

        private static List<string> GetListHtml()
        {
            List<string> Htmls = new List<string>()
            {
                "https://www.hackampus.com/","https://www.sabon.co.il/","https://stackoverflow.com/",
                "https://www.linkedin.com/","https://www.facebook.com/","https://he.wikipedia.org/",
                "https://github.com/","https://www.youtube.com/","https://docs.microsoft.com/",
                "https://www.drushim.co.il/"

            };
            return Htmls;
        }

        private static void Async_Downloading_by_Task()
        {
            List<string> Htmls = GetListHtml();
            Console.WriteLine("Async Downloading by ForEach");
            Stopwatch totalTime = Stopwatch.StartNew();
            List<Task> tasks = new List<Task>();
            WebClient webclient = new WebClient();
            foreach (string html in Htmls)
            {
                tasks.Add(new Task(() =>
                {
                    webclient = new WebClient();
                    Stopwatch thisTask = Stopwatch.StartNew();
                    string site = webclient.DownloadString(html);
                    thisTask.Stop();
                    Console.WriteLine($"byts:{site.Length}, {html}, Time: {thisTask.ElapsedMilliseconds} ");

                })
                       );
            }

            tasks.ForEach(i => i.Start());
            tasks.ForEach(i => i.Wait());
            totalTime.Stop();
            Console.WriteLine($"the total time is: {totalTime.ElapsedMilliseconds}");
        }

        static void DownloadStringsn()
        {
            Console.WriteLine("sync Downloading ");
            Stopwatch sn = Stopwatch.StartNew();

            Stopwatch sn0 = Stopwatch.StartNew();
            WebClient webClient0 = new WebClient();
            string hackampus = webClient0.DownloadString("https://www.hackampus.com/");
            Console.Write("https://www.hackampus.com/      ");
            sn0.Stop();
            Console.WriteLine(hackampus.Length + "  byts  Time:" + sn0.ElapsedMilliseconds);

            Stopwatch sn1 = Stopwatch.StartNew();
            WebClient webClient1 = new WebClient();
            string sabon = webClient1.DownloadString("https://www.sabon.co.il/");
            Console.Write("https://www.sabon.co.il/   ");
            sn1.Stop();
            Console.WriteLine(sabon.Length + "  byts  Time:" + sn1.ElapsedMilliseconds);

            Stopwatch sn2 = Stopwatch.StartNew();
            WebClient webClient2 = new WebClient();
            string stackoverflow = webClient2.DownloadString("https://stackoverflow.com/");
            Console.Write("https://stackoverflow.com/   ");
            sn2.Stop();
            Console.WriteLine(stackoverflow.Length + "  byts  Time:" + sn2.ElapsedMilliseconds);

            Stopwatch sn3 = Stopwatch.StartNew();
            WebClient webClient3 = new WebClient();
            string linkedin = webClient3.DownloadString("https://www.linkedin.com/");
            Console.Write("https://www.linkedin.com/   ");
            sn3.Stop();
            Console.WriteLine(linkedin.Length + "  byts  Time:" + sn3.ElapsedMilliseconds);

            Stopwatch sn4 = Stopwatch.StartNew();
            WebClient webClient4 = new WebClient();
            string facebook = webClient4.DownloadString("https://www.facebook.com/");
            Console.Write("https://www.facebook.com/   ");
            sn4.Stop();
            Console.WriteLine(facebook.Length + "  byts  Time:" + sn4.ElapsedMilliseconds);

            Stopwatch sn5 = Stopwatch.StartNew();
            WebClient webClient5 = new WebClient();
            string wikipedia = webClient5.DownloadString("https://he.wikipedia.org/");
            Console.Write("https://he.wikipedia.org/   ");
            sn5.Stop();
            Console.WriteLine(wikipedia.Length + "  byts  Time:" + sn5.ElapsedMilliseconds);

            Stopwatch sn6 = Stopwatch.StartNew();
            WebClient webClient6 = new WebClient();
            string github = webClient6.DownloadString("https://github.com/");
            Console.Write("https://github.com/   ");
            sn6.Stop();
            Console.WriteLine(github.Length + "  byts  Time:" + sn6.ElapsedMilliseconds);

            Stopwatch sn7 = Stopwatch.StartNew();
            WebClient webClient7 = new WebClient();
            string youtube = webClient7.DownloadString("https://www.youtube.com/");
            Console.Write("https://www.youtube.com/   ");
            sn7.Stop();
            Console.WriteLine(youtube.Length + "  byts  Time:" + sn7.ElapsedMilliseconds);

            Stopwatch sn8 = Stopwatch.StartNew();
            WebClient webClient8 = new WebClient();
            string microsoft = webClient8.DownloadString("https://docs.microsoft.com/");
            Console.Write("https://docs.microsoft.com/   ");
            sn8.Stop();
            Console.WriteLine(microsoft.Length + "  byts  Time:" + sn8.ElapsedMilliseconds);

            Stopwatch sn9 = Stopwatch.StartNew();
            WebClient webClient9 = new WebClient();
            string drushim = webClient8.DownloadString("https://www.drushim.co.il/");
            Console.Write("https://www.drushim.co.il/   ");
            sn9.Stop();
            Console.WriteLine(drushim.Length + "  byts  Time:" + sn9.ElapsedMilliseconds);

            sn.Stop();
            Console.Write("The total time of  RAZIF is: ");
            Console.WriteLine(sn.ElapsedMilliseconds + "\n");

        }
    }
}
