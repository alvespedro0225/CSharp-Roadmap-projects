using System.Globalization;

namespace TaskBuilder_CSharp;

public class Tasks
{
    private const string IdFile = "./id.txt";
    private static int _currentId = GetCurrentId();
    private string? _description;
    public int Id { get; set; }
    public string? CreatedAt { get; init; }
    public string? UpdatedAt { get; set; }
    public string? Status { get; set; }
    
    public string? Description 
    {   
        get => _description;
        set
        {
            _description = value;
            UpdatedAt = DateTime.Now.ToString(CultureInfo.GetCultureInfo("pt-br"));
        } 
    }
    
    public Tasks(string description, string status="todo")
    {
        Id = GetId();        
        _description = description;
        CreatedAt = DateTime.Now.ToString(CultureInfo.GetCultureInfo("pt-br"));
        UpdatedAt = DateTime.Now.ToString(CultureInfo.GetCultureInfo("pt-br"));
        Status = status;   
    }
    
    public Tasks() 
    {
        TaskManager.AddTask(this);
    }
    
    private static int GetId()
    {
        return ++_currentId;
    }
    
    public override string ToString()
    {
        return
            $"Task: {{ID: {Id}, Description: {Description}, Status: {Status}, Created at: {CreatedAt}, Updated at {UpdatedAt}}}";
    }
    
    public static int GetCurrentId()
    {
        try
        {
            using StreamReader fileData = new(IdFile);
            var id = fileData.ReadLine();
            return id == null ? 0 : int.Parse(id);
        }
        catch (FileNotFoundException)
        {
            return 0;
        }
    }
    
    public static void WriteCurrentId()
    {
        using StreamWriter idWriter = new(IdFile);
        idWriter.WriteLine(_currentId);
    }

    public static void Reset()
    {
        _currentId = 0;
    }
}