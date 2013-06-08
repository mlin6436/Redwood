using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Redwood
{
    public class Program
    {
        private static bool cancel = false;

        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Redwood has started. Use 'Ctrl + C' to stop.");

                var thread = new Thread(PrintUrls);
                thread.Start();

                var reset = new AutoResetEvent(false);
                Console.CancelKeyPress += (sender, eventArgs) =>
                    {
                        eventArgs.Cancel = true;
                        reset.Set();
                    };

                reset.WaitOne();
                cancel = true;
                Console.WriteLine("Stopping Redwood.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static void PrintUrls()
        {
            var path = ConfigurationManager.AppSettings["path"];
            Console.WriteLine("Reading file {0}...", path);

            if (!File.Exists(path))
            {
                Console.WriteLine("{0} does not exist!", path);
                return;
            }

            var results = new List<string>();
            var bookmark = GetBookmark(path);
            var bookmarkBarChildren = bookmark.roots.Bookmark_bar.Children;
            if (bookmarkBarChildren != null && bookmarkBarChildren.Any())
            {
                foreach (var bookmarkBarChild in bookmarkBarChildren)
                {
                    results = GetChild(results, bookmarkBarChild);
                }
            }

            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }

        private static List<string> GetChild(List<string> s, Child c)
        {
            s.Add(String.Format("{0} - {1}", c.ID, c.Url));

            if (c.Children != null && c.Children.Any())
            {
                foreach (var child in c.Children)
                {
                    GetChild(s, child);
                }
            }

            return s;
        }

        private static Bookmark GetBookmark(string path)
        {
            Bookmark bookmark;

            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        var serialiser = JsonSerializer.Create();
                        bookmark = serialiser.Deserialize<Bookmark>(jsonTextReader);
                    }
                }
            }

            return bookmark;
        }
    }
}
