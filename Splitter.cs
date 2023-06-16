using System.Text.RegularExpressions;

public enum Delimiter
{
    Plus,
    Asterisk,
    None
}

public class Splitter
{
    public static string RemoveParentheses(string expression)
    {
        string pattern = @"\([^()]*\)";
        Regex regex = new Regex(pattern);

        while (regex.IsMatch(expression))
        {
            expression = regex.Replace(expression, "");
        }

        return expression;
    }

    public static string[] SplitAndDetectDelimiter(string input, out Delimiter delimiter)
    {
        string result = RemoveParentheses(input);
        delimiter = Delimiter.None;

        string[] parts;

        if (result.Contains("+"))
        {
            delimiter = Delimiter.Plus;
        }
        else if (result.Contains("*"))
        {
            delimiter = Delimiter.Asterisk;
        }

        if (delimiter != Delimiter.None)
        {
            string pattern = @"(?<!\([^)]*)([+*])(?![^(]*\))";
            Regex regex = new Regex(pattern);
            parts = regex.Split(input);
        }
        else
        {
            parts = new string[] { input };
        }
        parts = parts.Where(part => part != "*" && part != "+").ToArray();
        return parts;
    }
}
