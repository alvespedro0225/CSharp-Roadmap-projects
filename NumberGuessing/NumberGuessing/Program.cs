namespace NumberGuessing;

public static class Program
{
    public static void Main(string[] args)
    {
        var rnd = new Random();
        var chosenNumber = (short) rnd.Next(1, 100);
        short tries = 0;
        short maxTries = 0;
        Console.WriteLine("Choose a difficulty:\nEasy (10 tries),\nMedium (5 tries),\nHard (3 tries)\n");
        while (maxTries == 0)
        {
            var difficulty = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(difficulty))
            {
                Console.Write("Please enter a valid difficulty:");
                continue;
            }
            var difficultyTable = new Dictionary<string, short>
            {
                { "easy", 10 },
                { "medium", 5 },
                { "hard", 3 }
            };
            try
            {
                maxTries = difficultyTable[difficulty.ToLower()];
            }
            catch (KeyNotFoundException)
            {
                Console.Write("Please enter a valid difficulty:");
            }
        }
        
        while (true)
        {
            if (tries >= maxTries)
            {
                Console.WriteLine($"You are out of tries. The number was {chosenNumber}.");
                break;
            }
            Console.Write("Choose a number: ");
            var number = Console.ReadLine();
            if (!short.TryParse(number, out var numberGuess))
            {
                Console.WriteLine($"{number} is not a number\n");
                continue;
            }

            if (numberGuess > chosenNumber)
            {
                Console.WriteLine($"{number} is too big\n");
                tries++;
            }
            else if (numberGuess < chosenNumber)
            {
                Console.WriteLine($"{number} is too small\n");
                tries++;
            }
            else
            {
                Console.WriteLine("You entered the right number, you win!\n");
                break;
            }
        }
    }
}