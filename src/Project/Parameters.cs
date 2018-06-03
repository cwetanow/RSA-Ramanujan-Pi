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
}
