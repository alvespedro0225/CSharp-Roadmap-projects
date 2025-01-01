namespace TaskBuilder_CSharp;

public static class Program
{
    public static void Main()
    {
        FileManager.GetFileData();
        Console.WriteLine("""
        Choose an action. Possible:
        add <description> <status*>,
        update <id> <description> <status*>,
        delete <id>,
        status <id> <status>,
        list <status*>,
        clear,
        reset,
        quit

        * = optional
        status = "todo", "ongoing", "completed"
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

                try
                {
                    argv = InputValidator.MakeList(input);
                    break;
                }
                catch (FormatException e)
                {
                        Console.WriteLine(e.Message);
                }
            }
            var argc = argv.Count;
            var action = argv[0];
            Dictionary<string, Action<List<string>, int>> actions = new()
            {
                {
                    "add", AppLogic.Add
                },
                {
                    "update", AppLogic.Update
                },
                {
                    "delete", AppLogic.Delete
                },
                {
                    "status", AppLogic.Status
                },
                {
                    "list", AppLogic.List
                },
                {
                    "clear", AppLogic.Clear
                },
                {
                    "reset", AppLogic.Reset
                },
                {
                    "quit", AppLogic.Quit
                }
            };
            try
            {
                actions[action](argv, argc);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine($"Missing arguments. Passed \"{argv[0]}\".\n");
            }
            catch (InvalidDataException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine($"Action \"{argv[0]}\" is not a valid action.\n");
            }
        }
    }
}