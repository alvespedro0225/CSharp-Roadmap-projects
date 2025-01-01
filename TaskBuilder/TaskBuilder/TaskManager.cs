using System.Globalization;

namespace TaskBuilder_CSharp;

public static class TaskManager
{  
    public static List<Tasks> AllTasks { get; private set; } = [];
    public static void AddTask(string description, string status = "todo")
    {
        Tasks task = new(description, status);
        AllTasks.Add(task);
        Console.WriteLine($"Added new task with description \"{task.Description}\" and ID {task.Id}.\n");
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
            var previousDescription = task.Description;
            task.Description = description;
            task.UpdatedAt = DateTime.Now.ToString(CultureInfo.GetCultureInfo("pt-br"));
            Console.WriteLine($"Updated task {id} from \"{previousDescription}\" to \"{task.Description}\".\n");
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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (task.Status.Equals(status))
            {
                Console.WriteLine($"Current status of task {id} already is \"{status}\".\n");
                return;
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            var previousStatus = task.Status;
            task.Status = status switch
            {
                "todo" => "todo",
                "ongoing" => "ongoing",
                "completed" => "completed",
                _ => throw new InvalidDataException()
            };
            task.UpdatedAt = DateTime.Now.ToString(CultureInfo.GetCultureInfo("pt-br"));
            Console.WriteLine($"Updated status {id} from \"{previousStatus}\" to \"{task.Status}\".\n");           
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
    public static void Filter(Func<Tasks, bool> predicate)
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