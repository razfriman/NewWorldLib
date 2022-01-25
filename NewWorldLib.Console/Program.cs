using System.Text.Json;
using NewWorldLib;
using NewWorldLib.Datasheets;

void ReadPaks()
{
    Console.WriteLine("Read Paks");
    var dir = OperatingSystem.IsWindows()
        ? @"D:\SteamLibrary\steamapps\common\New World Public Test Realm/assets"
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
}

void ReadDatasheets()
{
    Console.WriteLine("Read Datasheets");
    var dir = @"C:\Users\razfr\Desktop\out\sharedassets\springboardentitites\datatables";
    foreach(var file in Directory.GetFiles(dir, "*.datasheet"))
    {
    var datasheet = Datasheet.Parse(file);
    var json = JsonSerializer.Serialize(datasheet, new JsonSerializerOptions()
    {
        WriteIndented = true
    });
    File.WriteAllText(Path.ChangeExtension(file, "json"), json);
    Console.WriteLine(file);
    }
}

//ReadPaks();
ReadDatasheets();

Console.WriteLine("Done");