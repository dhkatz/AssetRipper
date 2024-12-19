using AssetRipper.Export.UnityProjects;
using AssetRipper.Export.UnityProjects.Configuration;
using AssetRipper.Import.Logging;
using AssetRipper.Processing;

namespace AssetRipper.Tools.Runner;

static class Program
{
	static void Main(string[] args)
	{
		if (args.Length == 0)
		{
			Console.WriteLine("Usage: AssetRipper.Tools.Runner <settings> <output> <input>...");
		}
		else
		{
			string settingsPath = args[0];
			string outputPath = args[1];
			string[] inputPaths = args.Skip(2).ToArray();
			
			Console.WriteLine("Ripping...");
			Console.WriteLine($"- Settings: {settingsPath}");
			Console.WriteLine($"- Output: {outputPath}");
			Console.WriteLine($"- Inputs: {string.Join(", ", inputPaths)}");
			
			Rip(inputPaths, outputPath, settingsPath);
		}
	}

	private static void Rip(string[] inputPaths, string outputPath, string settingsPath)
	{
		Logger.Clear();
		Logger.Add(new ConsoleLogger(false));

		SerializedSettings settings = SerializedSettings.Load(settingsPath);

		LibraryConfiguration configuration = new()
		{
			ImportSettings = settings.Import,
			ProcessingSettings = settings.Processing,
			ExportSettings = settings.Export,
		};
		configuration.LogConfigurationValues();

		ExportHandler exportHandler = new(configuration);
		GameData gameData = exportHandler.LoadAndProcess(inputPaths);
		PrepareExportDirectory(outputPath);
		exportHandler.Export(gameData, outputPath);
	}
	
	private static void PrepareExportDirectory(string path)
	{
		if (!Directory.Exists(path))
		{
			return;
		}

		Logger.Info(LogCategory.Export, "Clearing export directory...");
		Directory.Delete(path, true);
	}

}
