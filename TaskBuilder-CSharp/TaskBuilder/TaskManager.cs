using System.Globalization;

namespace TaskBuilder_CSharp;

public static class TaskManager
{  
    public static List<Tasks> AllTasks { get; private set; } = [];
    public static int AddTask(string description, string mark="todo")
    {
        Tasks task = new(description, mark);
        AllTasks.Add(task);
        Console.WriteLine($"Added new task with description \"{task.Description}\" and ID {task.Id}.\n");
        return task.Id;
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

    public static void MarkTask(int id, string mark)
    {
        try
        {
            mark = mark.ToLower();
            var task = AllTasks.Where((task) => task.Id == id).ToList()[0];
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (task.Status.Equals(mark))
            {
                Console.WriteLine($"Current status of task {id} already is \"{mark}\".\n");
                return;
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            var previousMark = task.Status;
            task.Status = mark switch
            {
                "todo" => "todo",
                "ongoing" => "ongoing",
                "completed" => "completed",
                _ => throw new InvalidDataException()
            };
            task.UpdatedAt = DateTime.Now.ToString(CultureInfo.GetCultureInfo("pt-br"));
            Console.WriteLine($"Updated mark {id} from \"{previousMark}\" to \"{task.Status}\".\n");           
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine($"No task with ID {id} registered.\n");
        }
        catch (InvalidDataException)
        {
            Console.WriteLine($"Invalid mark \"{mark}\"\n.");
        }
    }
    public static void Filter(Func<Tasks, bool> predicate)
    {
        var markFiltered = AllTasks.Where(predicate).ToList();
        foreach (var task in markFiltered)
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