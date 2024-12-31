namespace TaskBuilder_CSharp;

public static class InputValidator
{
    public static List<string>? MakeList(string input)
    {
        List<string> inputWords = [];
        var word = string.Empty;
        try
        {
            for (var i = 0; i < input.Length; i++)
            {
                if (inputWords.Count == 3) break;
                if (input[i] == '\'' || input[i] == '\"')
                {
                    word += input[i++];
                    while (true)
                    {
                        if (input[i] == '\'' || input[i] == '\"')
                        {
                            break;
                        }

                        word += input[i++];
                        if (i != input.Length) continue;
                        inputWords.Add(word);
                        throw new FormatException();
                    }

                    if (i == input.Length - 1)
                        inputWords.Add(word[1..]);
                    continue;
                }

                if (input[i] != ' ')
                {
                    word += input[i];
                }
                else if (input[i] == ' ' && !string.IsNullOrEmpty(word))
                {
                    inputWords.Add(word);
                    word = string.Empty;
                }

                if (i == (input.Length - 1) && input[i] != ' ')
                {
                    inputWords.Add(word);
                }
            }
        }
        catch (FormatException)
        {
            Console.WriteLine($"Invalid input \"{word}\" doesn't have closing {word[0]}\n");
            return null;
        }
        return inputWords;
    }
}