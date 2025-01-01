using System.Globalization;

namespace TaskBuilder_CSharp;

public static class TaskManager
{  
    public static List<Tasks> AllTasks { get; private set; } = [];
    public static List<string> PossibleStatuses { get; } = ["todo", "ongoing", "completed"];
    public static void AddTask(string description, string status = "todo")
    {
        if (!PossibleStatuses.Contains(status))
        {
            Console.WriteLine($"Status \"{status}\" is not a valid status");
        }
        else
        {
            Tasks task = new(description, status);
            AllTasks.Add(task);
            Console.WriteLine($"Added new task with description \"{task.Description}\", status {task.Status} and ID {task.Id}.\n");
        }
    }
    public static void AddTask(Tasks task)
    {
        AllTasks.Add(task);
    }

    public static void UpdateTask(int id, string description)
    {
        try
        {
            var task = AllTasks.Where((task) => task.Id == id).ToList()[0];
            task.Description = description;
            task.UpdatedAt = DateTime.Now.ToString(CultureInfo.GetCultureInfo("pt-br"));
            Console.WriteLine($"Updated task {id} to \"{task.Description}\".\n");
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine($"No task with ID {id} registered.\n");
        }       
    }

    public static void UpdateTask(int id, string description, string status)
    {
        try
        {
            if (!PossibleStatuses.Contains(status))
            {
                Console.WriteLine($"Status \"{status}\" is not a valid status");
            }
            else
            {
                var task = AllTasks.Where((task) => task.Id == id).ToList()[0];
                task.Status = status;
                task.Description = description;
                task.UpdatedAt = DateTime.Now.ToString(CultureInfo.GetCultureInfo("pt-br"));
                Console.WriteLine($"Updated task {id} to \"{task.Description}\" and \"{task.Status}\".\n");
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine($"No task with ID {id} registered.\n");
        }       
    }
    public static void DeleteTask(int id)
    {
        try
        {
            var task = AllTasks.Where((task) => task.Id == id).ToList()[0];
            Console.WriteLine($"Deleted task {task.Id}.\n");
            AllTasks.Remove(task);
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine($"No task with ID {id} registered.\n");         
        }
    }  

    public static void UpdateStatus(int id, string status)
    {
        try
        {
            status = status.ToLower();
            var task = AllTasks.Where((task) => task.Id == id).ToList()[0];
            task.Status = status switch
            {
                "todo" => "todo",
                "ongoing" => "ongoing",
                "completed" => "completed",
                _ => throw new InvalidDataException()
            };
            task.UpdatedAt = DateTime.Now.ToString(CultureInfo.GetCultureInfo("pt-br"));
            Console.WriteLine($"Updated status {id} to \"{task.Status}\".\n");           
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine($"No task with ID {id} registered.\n");
        }
        catch (InvalidDataException)
        {
            Console.WriteLine($"Invalid status \"{status}\"\n.");
        }
    }
    public static void PrintFiltered(Func<Tasks, bool> predicate)
    {
        var statusFiltered = AllTasks.Where(predicate).ToList();
        foreach (var task in statusFiltered)
        {
            Console.WriteLine(task);
        }
        Console.WriteLine();
    }

    public static void Clear(bool reset = false)
    {
        AllTasks.Clear();
        if (reset)
        {
            Tasks.Reset();
            Console.WriteLine("App data has been reset.\n");
        }
        else
        {
            Console.WriteLine("All tasks have been cleared.\n");
        }
    }
}