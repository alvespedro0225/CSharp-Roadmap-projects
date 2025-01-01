namespace TaskBuilder_CSharp;

public static class AppLogic
{
    public static void Add(List<string> argv, int argc)
    {
        var description = argv[1];
        TaskManager.AddTask(description);
    }

    public static void Update(List<string> argv, int argc)
    {
        var newDescription = argv[2];
        if (!int.TryParse(argv[1], out var taskId))
            throw new InvalidDataException($"\"{argv[1]}\" is not a valid number. Please try again.\n");
        TaskManager.UpdateTask(taskId, newDescription);
    }

    public static void Delete(List<string> argv, int argc)
    {
        if (!int.TryParse(argv[1], out var taskId))
            throw new InvalidDataException($"\"{argv[1]}\" is not a valid number. Please try again.\n");
        TaskManager.DeleteTask(taskId);
    }

    public static void Status(List<string> argv, int argc)
    {
        var newStatus = argv[2];
        if (!int.TryParse(argv[1], out var taskId))
            throw new InvalidDataException($"\"{argv[1]}\" is not a valid number. Please try again.\n");
        TaskManager.UpdateStatus(taskId, newStatus);
    }

    public static void List(List<string> argv, int argc)
    {
        try
        {
            switch (argv[1])
            {
                case "todo":
                    TaskManager.Filter((task) => task.Status == "todo");
                    break;

                case "ongoing":
                    TaskManager.Filter((task) => task.Status == "ongoing");
                    break;

                case "completed":
                    TaskManager.Filter((task) => task.Status == "completed");
                    break;
                default:
                    TaskManager.Filter((_) => true);
                    break;
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            TaskManager.Filter((_) => true);
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
