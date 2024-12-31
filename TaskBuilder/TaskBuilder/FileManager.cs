using System.Text.Json;

namespace TaskBuilder_CSharp;

public static class FileManager
{
    private const string FilePath = "./tasks.json";

    private static readonly JsonSerializerOptions Serializer = new()
    {
        AllowTrailingCommas = true,
        WriteIndented = true
    };

    public static void GetFileData()
    {
        using var fileData = File.Open(FilePath, FileMode.OpenOrCreate);
        fileData.Close();
        var file = File.ReadAllText(FilePath); 
        if (string.IsNullOrWhiteSpace(file)) return; 
        JsonSerializer.Deserialize<List<Tasks>>(file, Serializer);
        //foreach (var task in tasks)
    }

    public static void WriteToFile()
    {
        using var outputFile = new StreamWriter(FilePath);
        outputFile.Write("[");
        var allTasks = TaskManager.AllTasks;
        var jsonTasks = allTasks.Select(task => JsonSerializer.Serialize(task, Serializer));
        foreach (var taskJson in jsonTasks)
        {
            outputFile.Write(taskJson + ",\n");
        }
        outputFile.Write("]");
    }
}