public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide an expression as an argument.");
            return;
        }

        string input = args[0];
        try
        {
            Expression result = SimplifyExpression(input);
            Console.WriteLine(result);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }

    public static Expression SimplifyExpression(string input)
    {
        Delimiter delimiter;
        string[] parts = Splitter.SplitAndDetectDelimiter(input, out delimiter);

        if (delimiter == Delimiter.None)
        {
            if (input.StartsWith("(") && input.EndsWith(")"))
            {
                string newInput = input.Substring(1, input.Length - 2);
                // Console.WriteLine("() found, casting new SimplifyExpression( " + newInput);
                return SimplifyExpression(newInput);
            }
            else
            {
                return new Expression(input);
            }
        }
        else if (delimiter == Delimiter.Plus || delimiter == Delimiter.Asterisk)
        {
            Expression expression = delimiter == Delimiter.Plus ? new Expression("0") : new Expression("1");

            foreach (string part in parts)
            {
                if (delimiter == Delimiter.Plus)
                {
                    // Console.WriteLine("Delimiter.Plus expression.Add(SimplifyExpression( " + part);
                    expression.Add(SimplifyExpression(part));
                }
                else if (delimiter == Delimiter.Asterisk)
                {
                    // Console.WriteLine("Delimiter.Asterisk expression.Multiply(SimplifyExpression( " + part);
                    expression.Multiply(SimplifyExpression(part));
                }
            }

            return expression;
        }

        throw new ArgumentException("Invalid input: " + input);
    }
}
