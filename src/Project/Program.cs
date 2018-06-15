using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Project
{
    public class Parameters
    {
        public const string DefaultOutputFile = "result.txt";

        // -p
        public int ElementsCount { get; set; }
        // -t
        public int MaxTasks { get; set; }
        // -q
        public bool IsQuiet { get; set; }
        // -o
        public string OutputFile { get; set; }
    }

    public class Program
    {
        private static readonly Stopwatch stopwatch = new Stopwatch();
        private static readonly Queue<int> threads = new Queue<int>();

        private static double Sum;

        public static void Main(string[] args)
        {
            var parameters = ParseParameters(args);

            var result = Solve(parameters);

            WriteToFile(result, parameters.OutputFile);
        }

        public static double Solve(Parameters parameters)
        {
            Sum = 0;
            for (var i = 1; i <= parameters.MaxTasks; i++)
            {
                threads.Enqueue(i);
            }

            stopwatch.Start();

            Parallel.For(0, parameters.ElementsCount,
                new ParallelOptions { MaxDegreeOfParallelism = parameters.MaxTasks },
                i =>
                {
                    if (parameters.IsQuiet)
                    {
                        Sum += Calculate(i);
                    }
                    else
                    {
                        var currentThread = Thread.CurrentThread.ManagedThreadId;
                        var time = stopwatch.ElapsedMilliseconds;
                        Console.WriteLine($"Thread {currentThread} started");

                        Sum += Calculate(i);

                        Console.WriteLine($"Thread {currentThread} stopped");

                        time = stopwatch.ElapsedMilliseconds - time;
                        Console.WriteLine($"Thread {currentThread} execution time is {time}ms");
                    }
                });

            var result = 9801 / (Math.Sqrt(8) * Sum);
            stopwatch.Stop();

            Console.WriteLine($"Execution time: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"Threads used: {parameters.MaxTasks}");

            return result;
        }

        public static void WriteToFile(double result, string fileName)
        {
            using (var writer = File.AppendText(fileName))
            {
                writer.WriteLine(result);
            }
        }

        public static Parameters ParseParameters(string[] args)
        {
            var parameters = new Parameters();

            for (var i = 0; i < args.Length; i++)
            {
                if (args[i] == "-t" || args[i] == "tasks")
                {
                    if (args.Length > i + 1)
                    {
                        parameters.MaxTasks = int.Parse(args[i + 1]);
                        i++;
                    }
                }
                else if (args[i] == "-p")
                {
                    if (args.Length > i + 1)
                    {
                        parameters.ElementsCount = int.Parse(args[i + 1]);
                        i++;
                    }
                }
                else if (args[i] == "-o")
                {
                    if (args.Length > i + 1)
                    {
                        parameters.OutputFile = args[i + 1];
                        i++;
                    }
                }
                else if (args[i] == "-q")
                {
                    parameters.IsQuiet = true;
                }
            }

            if (string.IsNullOrEmpty(parameters.OutputFile))
            {
                parameters.OutputFile = Parameters.DefaultOutputFile;
            }

            return parameters;
        }

        public static double Calculate(int n)
        {
            var result = (1103 + 26390 * n) / Math.Pow(396, 4 * n);

            var numerator = n + 1;

            for (var i = 0; i < 3; i++)
            {
                var denominator = 1d;

                for (var j = 0; j < n; j++)
                {
                    result *= (numerator / denominator);
                    numerator++;
                    denominator++;
                }
            }

            return result;
        }
    }
}
