using System.Text.Json;
using NewWorldLib;

var dir =OperatingSystem.IsWindows()
    ? @"D:\SteamLibrary\steamapps\common\New World\assets"
    : "/Users/razfriman/Downloads/NEW WORLD/assets";
var outputDir = "extracted";
var files = Directory.GetFiles(dir, "*.pak", SearchOption.AllDirectories);
var entries = new HashSet<string>();

foreach (var file in files)
{
    using var pakFile = PakFile.Parse(file);
    // pakFile.Save(outputDir);
    foreach (var entryName in pakFile.Entries.Keys)
    {
        var entry = pakFile.Entries[entryName];
        entries.Add(entryName);
        if (entry.Method != 15)
        {
            Console.WriteLine(entryName);
            entry.Save(outputDir);
        }
    }
}



File.WriteAllText("files.txt", JsonSerializer.Serialize(entries));
Console.WriteLine("Done");