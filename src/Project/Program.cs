using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Project
{
    public class Program
    {
        private static readonly Stopwatch stopwatch = new Stopwatch();

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
            stopwatch.Start();

            Parallel.For(0, parameters.ElementsCount,
                new ParallelOptions { MaxDegreeOfParallelism = parameters.MaxTasks },
                i =>
                {
                    if (!parameters.IsQuiet)
                    {
                        //Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} started");
                    }

                    Sum += Calculate(i);


                    if (!parameters.IsQuiet)
                    {
                        //Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} stopped");
                    }
                });

            var result = 9801 / (Math.Sqrt(8) * Sum);
            stopwatch.Stop();

            Console.WriteLine($"Execution time: {stopwatch.ElapsedMilliseconds}ms, Threads: {parameters.MaxTasks}");

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
