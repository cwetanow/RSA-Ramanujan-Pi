using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Project
{
	class Program
	{
		private static Stopwatch stopwatch = new Stopwatch();

		static void Main(string[] args)
		{
			for (int threads = 1; threads < 9; threads++)
			{
				stopwatch.Restart();
				Parallel.For(0, short.MaxValue * 10, new ParallelOptions { MaxDegreeOfParallelism = threads }, i =>
				{
					Factorial(i);
				});

				Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds}, Threads: {threads}");
				stopwatch.Stop();
			}
		}

		static void Factorial(int n)
		{
			var result = 1;

			for (int i = 1; i < n; i++)
			{
				result *= i;
			}
		}
	}
}
