using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Configuration;

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
            Console.WriteLine("********************************************************************************");
            Console.WriteLine("                                 ***FOOL COMPILER***");
            Console.WriteLine("********************************************************************************");
            Console.WriteLine("Usage: ");
            Console.WriteLine("\t1.[Select one of the provided input files: \"input01\" up to \"input40\"]");
            Console.WriteLine("\t2.[Insert your input from Console by adding <file name> + <file text>]\n");
            Console.WriteLine("********************************************************************************");

            string fileName = ConfigurationManager.AppSettings.Get("inputFilePath");
            DirectoryInfo folder = new DirectoryInfo(fileName);
            FileInfo[] Files = folder.GetFiles("*.fool");
            List<string> list = new List<string>();
            foreach (FileInfo file in Files)
            {
                list.Add(file.Name);
            }

            string userInput;
            userInput = Console.ReadLine();
            userInput.ToLower();

            if (!list.Contains(userInput))
            {
                string line;
                while (!String.IsNullOrWhiteSpace(line = Console.ReadLine()))
                {
                    using (FileStream fs = File.Create(fileName + userInput + ".fool"))
                    {
                        byte[] info = new UTF8Encoding(true).GetBytes(line);
                        fs.Write(info);
                    }
                }
            }

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
