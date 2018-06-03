using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Project
{
	class Program
	{
		private static Stopwatch stopwatch = new Stopwatch();

		private static double Sum;

		static void Main(string[] args)
		{
			var parameters = ParseParameters(args);

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

			var result = Math.Pow(99, 2) / (Math.Sqrt(8) * Sum);
			stopwatch.Stop();

			Console.WriteLine($"Execution time: {stopwatch.ElapsedMilliseconds}ms, Threads: {parameters.MaxTasks}");

			WriteToFile(result, parameters.OutputFile);
		}

		static void WriteToFile(double result, string fileName)
		{
			using (var writer = File.AppendText(fileName))
			{
				writer.WriteLine(result);
			}
		}

		static Parameters ParseParameters(string[] args)
		{
			var parameters = new Parameters();

			for (int i = 0; i < args.Length; i++)
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

		static double Calculate(int n)
		{
			var result = (1103 + 26390 * n) / Math.Pow(396, 4 * n);

			var top = n + 1;

			for (int i = 0; i < 3; i++)
			{
				var bottom = 1d;

				for (int j = 0; j < n; j++)
				{
					result *= (top / bottom);
					top++;
					bottom++;
				}
			}

			return result;
		}
	}
}
