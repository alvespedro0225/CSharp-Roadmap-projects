namespace TaskBuilder_CSharp;

public static class Program
{
    public static void Main()
    {
        Tasks.GetCurrentId();
        FileManager.GetFileData();
        Console.WriteLine("""
        Choose an action. Possible:
        add <description>,
        update <id> <description>,
        delete <id>,
        mark <id> <mark>,
        list <mark*>,
        clear,
        reset,
        quit

        * = optional
        marks = "todo", "ongoing", "completed"
        """);
        Console.WriteLine();
        while (true)
        {
            List<string>? argv;
            while (true)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine($"Invalid input \n{input}\n");
                    continue;
                }
                argv = InputValidator.MakeList(input);
                if (argv != null) break;
            }
            var argc = argv.Count;
            try
            {
                switch (argv[0])
                {
                    case "add":
                    {
                        var description = argv[1];
                        TaskManager.AddTask(description);
                        break;
                    }
                    case "update":
                        var newDescription = argv[2];
                    {
                        if (!int.TryParse(argv[1], out var taskId))
                            throw new InvalidDataException($"\"{argv[1]}\" is not a valid number. Please try again.\n");
                        TaskManager.UpdateTask(taskId, newDescription);
                        break;
                    }
                    case "delete":
                    {
                        if (!int.TryParse(argv[1], out var taskId))
                            throw new InvalidDataException($"\"{argv[1]}\" is not a valid number. Please try again.\n");
                        TaskManager.DeleteTask(taskId);
                        break;
                    }
                    case "mark":
                    {
                        var newStatus = argv[2];
                        if (!int.TryParse(argv[1], out var taskId))
                            throw new InvalidDataException($"\"{argv[1]}\" is not a valid number. Please try again.\n");
                        TaskManager.MarkTask(taskId, newStatus);
                        break;
                    }
                    case "list":
                    {
                        if (argc > 1)
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
                            }
                        }
                        else
                        {
                            TaskManager.Filter((_) => true);
                        }

                        break;
                    }
                    case "quit":
                    {
                        FileManager.WriteToFile();
                        Tasks.WriteCurrentId();
                        Environment.Exit(0);
                        break;
                    }
                    case "clear":
                    {
                        TaskManager.Clear();
                        break;
                    }
                    case "reset":
                    {
                        TaskManager.Clear(true);
                        break;
                    }
                    default:
                    {
                        Console.WriteLine($"Wrong arguments provided \"{argv[0]}\".\n");
                        break;
                    }

                }
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine($"Missing arguments, please try again. Passed \"{argv[0]}\".\n");
            }
            catch (InvalidDataException)
            {
                Console.WriteLine($"\"{argv[1]}\" is not a valid number. Please try again.\n");
            }
            catch (FormatException)
            {
                Console.WriteLine($"\"{argv[1]}\" is not a valid number. Please try again.\n");
            }
        }
    }
}