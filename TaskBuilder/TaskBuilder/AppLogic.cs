namespace TaskBuilder_CSharp;

public static class AppLogic
{
    public static void Add(List<string> argv, int argc)
    {
        var description = argv[1];
        var status = argc >= 3 ? argv[2] : "todo";
        TaskManager.AddTask(description, status);
    }

    public static void Update(List<string> argv, int argc)
    {
        var newDescription = argv[2];
        if (!int.TryParse(argv[1], out var taskId))
        {
            throw new InvalidDataException($"\"{argv[1]}\" is not a valid number.\n");
        }
        try
        {
            var status = argv[3];
            TaskManager.UpdateTask(taskId, newDescription, status);
        }
        catch (ArgumentOutOfRangeException)
        {
            TaskManager.UpdateTask(taskId, newDescription);
        }
        
    }

    public static void Delete(List<string> argv, int argc)
    {
        if (!int.TryParse(argv[1], out var taskId))
        {
            throw new InvalidDataException($"\"{argv[1]}\" is not a valid number.\n");
        }
        
        TaskManager.DeleteTask(taskId);
    }

    public static void Status(List<string> argv, int argc)
    {
        var newStatus = argv[2];
        if (!int.TryParse(argv[1], out var taskId))
        {
            throw new InvalidDataException($"\"{argv[1]}\" is not a valid number.\n");
        }
        
        TaskManager.UpdateStatus(taskId, newStatus);
    }

    public static void List(List<string> argv, int argc)
    {
        try
        {
            var status = argv[1];
            if (TaskManager.PossibleStatuses.Contains(status))
            {
                TaskManager.PrintFiltered((task) => task.Status == status);
            }
            else
            {
                TaskManager.PrintFiltered((_) => true);
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            TaskManager.PrintFiltered((_) => true);
        }
    }

    public static void Quit(List<string> argv, int argc)
    {
        FileManager.WriteToFile();
        Tasks.WriteCurrentId();
        Environment.Exit(0);
    }

    public static void Clear(List<string> argv, int argc)
    {
        TaskManager.Clear();
    }
    
    public static void Reset(List<string> argv, int argc)
    {
        TaskManager.Clear(true);
    }
}
