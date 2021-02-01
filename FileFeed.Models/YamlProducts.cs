using System;
using System.Collections.Generic;
using System.Text;

namespace FileFeeds.Models
{
    public class RootProducts
    {
        public YamlProducts[] yamlProducts { get; set; }
    }
    public class YamlProducts
    {
        public string tag { get; set; }

        public string name { get; set; }

        public string twitter { get; set; }

    }
}
