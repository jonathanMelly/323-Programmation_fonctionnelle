using System;
using System.IO;
using System.Linq;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceDirectory = (@"..\..\..\..\files");
            //string fullDir = Path.GetDirectoryName(sourceDirectory);


                if (Directory.Exists(sourceDirectory))
                {
                
                    // This path is a directory
                    ProcessDirectory(sourceDirectory);
            }
                else
                {
                    Console.WriteLine("{0} is not a valid directory.");
                }
        }

        // Process all files in the directory passed in, recurse on any directories
        // that are found, and process the files they contain.
        public static void ProcessDirectory(string targetDirectory)
        {
            int fileAmount = 0;
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                fileAmount++;
            }
            WriteDownFile(targetDirectory, fileAmount);
        }

        // Insert logic for processing found files here.
        public static void WriteDownFile(string path, int fileAmount)
        {
            Console.WriteLine("There are " + fileAmount + " files in " + path);
        }
    }
}