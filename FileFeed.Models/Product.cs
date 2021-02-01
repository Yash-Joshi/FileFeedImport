using System;
using System.Collections.Generic;
using System.Text;

namespace FileFeeds.Models
{
    /// <summary>
    /// This is a root class that will store the array of products that is been passed to the solution
    /// </summary>
    public class Rootobject
    {
        public Product[] products { get; set; }
    }

    /// <summary>
    /// This is the base class for which the object will be saved and dispalyed to the console application
    /// This can be further utilized to save data to database   
    /// </summary>
    public class Product
    {
        public string[] categories { get; set; }
        public string twitter { get; set; }
        public string title { get; set; }
    }
}
