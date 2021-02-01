using FileFeeds.Models;
using System;
using System.Collections.Generic;
using System.IO;
using JsonSerializer = System.Text.Json.JsonSerializer;
using YamlDotNet.RepresentationModel;

namespace FileFeed.Import.App
{
    class Program
    {
        private static ConsoleKeyInfo cki;

        /// <summary>
        /// The Funtion which immidiately starts to get executed when program starts
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            Product products = new Product();

            Console.WriteLine("====Welcome to CLI tool for processing file feeds=====");
            Console.WriteLine("**Press Close button to stop the application**");
            do
            {
                while (!Console.KeyAvailable)
                {
                    StartProcess();
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);



        }

        /// <summary>
        /// This method is used to start the process for importing the file feed to console application
        /// </summary>
        public static void StartProcess()
        {
            Console.WriteLine("Please enter the file Path");


            // Ask for a file path to accept
            var filePath = Console.ReadLine();


            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException();
                }
                //Once the file path is selected then choose the format of the file 
                string fileFormat = GetFormat(filePath);


                if (fileFormat == ".json")
                {
                    ProcessJsonFile(filePath);
                }
                else if (fileFormat == ".yaml")
                {
                    ProcessYamlFile(filePath);
                }
                else if (fileFormat == ".csv")
                {
                    // TODO : if file added is csv then use this to create new method that can extract 
                    // the information from the file path
                }

                Console.WriteLine();
                Console.WriteLine("Importing Complete");

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Exception Occured");
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine();
            }

        }

        /// <summary>
        /// This return the file format based on which the application will derive which method to initiate
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static string GetFormat(string filePath)
        {
            return Path.GetExtension(filePath);
        }

        /// <summary>
        /// This method will process the data receieved from YAML file
        /// </summary>
        /// <param name="filePath"></param>
        private static void ProcessYamlFile(string filePath)
        {
            TextReader reader = File.OpenText(filePath);

            YamlStream yaml = new YamlStream();
            yaml.Load(reader);

            List<YamlProducts> yamlProducts = new List<YamlProducts>();



            var pTry = (YamlSequenceNode)yaml.Documents[0].RootNode;

            var count = pTry.Children.Count;

            for (int i = 0; i < count; i++)
            {
                YamlProducts product = new YamlProducts();

                var mappingYamlContents = (YamlMappingNode)((YamlSequenceNode)yaml.Documents[0].RootNode)[i];

                foreach (var entry in mappingYamlContents.Children)
                {
                    if (((YamlScalarNode)entry.Key).Value.ToString() == "tags")
                    {
                        product.tag = ((YamlScalarNode)entry.Value).ToString();
                    }

                    if (((YamlScalarNode)entry.Key).Value.ToString() == "name")
                    {
                        product.name = ((YamlScalarNode)entry.Value).ToString();
                    }

                    if (((YamlScalarNode)entry.Key).Value.ToString() == "twitter")
                    {
                        product.twitter = ((YamlScalarNode)entry.Value).ToString();
                    }


                }
                yamlProducts.Add(product);



            }

            foreach (var sectionItems in yamlProducts)
            {
                PrintYamlContent(sectionItems);
                //Save To Database
            }

        }

        /// <summary>
        /// This method is priting the YAML content that is been extracted from the application
        /// </summary>
        /// <param name="sectionItems"></param>
        private static void PrintYamlContent(YamlProducts sectionItems)
        {
            Console.WriteLine(@"Importing: Tags: {0} , Name: {1} , Twitter : {2}", sectionItems.tag, sectionItems.name, sectionItems.twitter);
        }

        /// <summary>
        /// This method will process the data receieved from JSON file
        /// </summary>
        /// <param name="filePath"></param>
        private static void ProcessJsonFile(string filePath)
        {
            Rootobject rootObject = new Rootobject();
            var stringValues = File.ReadAllText(filePath);
            rootObject = JsonSerializer.Deserialize<Rootobject>(stringValues);

            List<Product> ListProductItems = new List<Product>();

            foreach (var i in rootObject.products)
            {
                ListProductItems.Add(i);
            }

            foreach (var item in ListProductItems)
            {
                PrintItems(item);

            }



        }

        private static void PrintItems(Product item)
        {
            Product printItems = new Product();
            List<string> categories = new List<string>();
            printItems.title = item.title;
            printItems.twitter = item.twitter;
            string categoriesString = string.Empty;

            for (int i = 0; i < item.categories.Length; i++)
            {
                if (i == (item.categories.Length - 1))
                {
                    categoriesString += item.categories[i];
                }
                else
                    categoriesString += item.categories[i] + " & ";

            }

            Console.Write(@"Importing : Name : {0},Twitter :{1},Categories: {2}", printItems.title, printItems.twitter, categoriesString);
            Console.WriteLine();

        }
    }
}
