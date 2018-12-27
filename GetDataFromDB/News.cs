using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataFromDB
{
    public class News
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public News(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}
