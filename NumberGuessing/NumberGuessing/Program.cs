namespace NumberGuessing;

public static class Program
{
    public static void Main(string[] args)
    {
        var random = new Random();
        var rngNumber = (short) random.Next(1, 100);
        short tries = 0;
        short maxTries = 0;
        var win = false;
        Console.WriteLine("Choose a difficulty:\nEasy (10 tries),\nMedium (5 tries),\nHard (3 tries)\n");
        while (maxTries == 0)
        {
            var difficulty = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(difficulty))
            {
                Console.Write("Please enter a valid difficulty:");
                continue;
            }
            switch (difficulty.ToLower())
            {
                case "easy":
                    maxTries = 10;
                    break;
                
                case "medium":
                    maxTries = 5;
                    break;
                
                case "hard":
                    maxTries = 3;
                    break;
                
                default:
                    Console.Write("Please enter a valid difficulty: ");
                    break;
            }
        }
        
        while (true)
        {
            if (tries >= maxTries)
            {
                Console.WriteLine($"You are out of tries. The number was {rngNumber}.");
                break;
            }
            Console.Write("Choose a number: ");
            var number = Console.ReadLine();

            if (!short.TryParse(number, out var inputNumber))
            {
                Console.WriteLine($"{number} is not a number\n");
                continue;
            }
            switch ( rngNumber - inputNumber )
            {
                case > 0:
                    Console.WriteLine($"{inputNumber} is too small\n");
                    tries++;
                    break;
                case < 0:
                    Console.WriteLine($"{inputNumber} is too big\n");
                    tries++;
                    break;
                default:
                    Console.WriteLine("You entered the right number, you win!\n");
                    win = true;
                    break;
            }

            if (win)
            {
                break;
            }
        }
    }
}