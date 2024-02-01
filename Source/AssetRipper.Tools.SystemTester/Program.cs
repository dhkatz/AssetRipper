using AssetRipper.Export.UnityProjects;
using AssetRipper.Export.UnityProjects.Configuration;
using AssetRipper.Import.Logging;
using AssetRipper.Processing;

namespace AssetRipper.Tools.SystemTester
{
	static class Program
	{
		// private const string TestsDirectory = "../../Tests";

		static void Main(string[] args)
		{
			Logger.Add(new ConsoleLogger(true));
			Logger.Add(new FileLogger("AssetRipper.Tools.SystemTester.log"));
			Logger.LogSystemInformation("System Tester");
			Logger.BlankLine();

			if (args.Length != 0)
			{
				Rip(args[0], args[1]);
			}
		}

		private static void Rip(string inputPath, string outputPath)
		{
			LibraryConfiguration settings = new();
			settings.LogConfigurationValues();
			GameData gameData = Ripper.Load(new[] { inputPath }, settings);
			Ripper.Process(gameData, Ripper.GetDefaultProcessors(settings));
			PrepareExportDirectory(outputPath);
			Ripper.ExportProject(gameData, settings, outputPath);
		}

		private static void PrepareExportDirectory(string path)
		{
			if (Directory.Exists(path))
			{
				Logger.Info(LogCategory.Export, "Clearing export directory...");
				Directory.Delete(path, true);
			}
		}

		private static T[] Combine<T>(T[] array1, T[] array2)
		{
			ArgumentNullException.ThrowIfNull(array1);
			ArgumentNullException.ThrowIfNull(array2);

			T[] result = new T[array1.Length + array2.Length];
			for (int i = 0; i < array1.Length; i++)
			{
				result[i] = array1[i];
			}
			for (int j = 0; j < array2.Length; j++)
			{
				result[j + array1.Length] = array2[j];
			}
			return result;
		}
	}
}
