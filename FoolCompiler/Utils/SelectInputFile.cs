using System;
using System.Collections.Generic;
using System.IO;

namespace FoolCompiler.Utils
{
    public class SelectInputFile
    {
        private string _input;

        public SelectInputFile(string input)
        {
            _input = input;
        }
        public SelectInputFile()
        { }

        public string Start()
        {
            Console.WriteLine("Please select one of the following options: ");
            Console.WriteLine("\t1. Select one of the default input files from: \"input01\" up to \"input36\"");
            Console.WriteLine("\t2. Insert your own input from Console");

            string fileName = @"C:\Users\Newton\source\repos\FoolCompiler\FoolCompiler\InputFiles";
            DirectoryInfo folder = new DirectoryInfo(fileName);
            FileInfo[] Files = folder.GetFiles("*.fool");
            //string str = "";
            List<string> list = new List<string>();
            foreach (FileInfo file in Files)
            {

                list.Add(file.Name);
                //str = str + ", " + file.Name;
            }

            string userInput;
            userInput = Console.ReadLine();

            foreach (string item in list)
            {
                if (item == userInput)
                {
                    userInput = item;
                }
            }
            return userInput;
        }
    }
}
