using System;
using System.Diagnostics;
using System.Numerics;
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

			var defaultTime = 0d;

			for (int threads = 1; threads < 9; threads++)
			{
				Sum = 0;
				stopwatch.Restart();
				Parallel.For(0, parameters.ElementsCount,
					new ParallelOptions { MaxDegreeOfParallelism = threads },
					i =>
				{
					Sum += Calculate(i);
				});

				var result = Math.Pow(99, 2) / (Math.Sqrt(8) * Sum);
				stopwatch.Stop();

				if (threads == 1)
				{
					defaultTime = stopwatch.ElapsedMilliseconds;
				}

				Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds}, Threads: {threads}, Speedup: {string.Format("{0:F6}", defaultTime / stopwatch.ElapsedMilliseconds)}, Result: {result}");
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

		static long Factorial(int n)
		{
			var result = 1L;

			for (int i = 1; i < n; i++)
			{
				result *= i;
			}

			return result;
		}
	}
}
