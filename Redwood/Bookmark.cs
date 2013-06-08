using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redwood
{
    public class Bookmark
    {
        public string checksum { get; set; }
        public Root roots { get; set; }
        public int version { get; set; }
    }

    public class Root
    {
        public RootItem Bookmark_bar { get; set; }
        public RootItem Other { get; set; }
    }

    public class RootItem
    {
        public List<Child> Children { get; set; }
        public string Date_added { get; set; }
        public string Date_modified { get; set; }
        public int Id { get; set; }
        public String Name { get; set; }
        public string Type { get; set; }
    }

    public class Child
    {
        public List<Child> Children { get; set; }
        public string date_added { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public String Type { get; set; }
        public Uri Url { get; set; }
    }
}
