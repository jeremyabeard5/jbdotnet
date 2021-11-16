using System;

namespace _20211112
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello!");
            Console.WriteLine("What is your name?");
            var name = Console.ReadLine();
            var currentDate = DateTime.Now;
            Console.WriteLine(
                $"{Environment.NewLine}Hello {name}. Today is {currentDate:d} at {currentDate:t}!");
            Console.WriteLine($"{Environment.NewLine}Press Enter to exit...");
            var exitInput = Console.Read();
            Console.WriteLine("Exiting...");
        }
    }
}
