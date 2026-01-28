using System;
using System.Diagnostics;
using System.Threading;

namespace TargetDummy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "TargetDummy - Reste ouvert";

            int pid = Process.GetCurrentProcess().Id;
            Console.WriteLine("TargetDummy is running.");
            Console.WriteLine($"PID: {pid}");
            Console.WriteLine("Press Ctrl+C to exit.");

            // Maintenir ouvert en permanence
            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
