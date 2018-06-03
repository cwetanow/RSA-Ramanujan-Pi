namespace Project
{
	public class Parameters
	{
		public const string DefaultOutputFile = "result.txt";

		public int ElementsCount { get; set; }
		public int MaxTasks { get; set; }
		public bool IsQuiet { get; set; }
		public string OutputFile { get; set; }
	}
}
