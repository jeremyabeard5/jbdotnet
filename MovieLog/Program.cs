using System;
using System.IO;

namespace MovieLog
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Movie Log");
            String moviedir = "/media/jbmedia/Movies";
            Console.WriteLine(moviedir);
            Console.WriteLine("Reading files from following directory:\n" + moviedir + "\n");
            string[] allfiles = Directory.GetFiles(moviedir, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < allfiles.Length; i++) {
                Console.WriteLine(allfiles[i]);
            }
            Console.WriteLine("\nFinished.");
        }
    }
}
