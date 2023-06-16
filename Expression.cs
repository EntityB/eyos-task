using System.Numerics;

public class Expression
{
    private LinkedList<ExpressionValue> expressionValues;

    public Expression(string input)
    {
        expressionValues = new LinkedList<ExpressionValue>();
        ParseInput(input);
    }

    private void ParseInput(string input)
    {
        if (input == "x")
        {
            expressionValues.AddLast(new ExpressionValue(BigInteger.One, 1));
        }
        else
        {
            BigInteger number;
            if (BigInteger.TryParse(input, out number))
            {
                expressionValues.AddLast(new ExpressionValue(number, 0));
            }
            else
            {
                throw new ArgumentException("Invalid input: " + input);
            }
        }
    }

    public Expression Add(Expression exp)
    {
        foreach (ExpressionValue value in exp.expressionValues)
        {
            ExpressionValue? existingValue = FindValueWithExponent(value.Exponent);
            if (existingValue != null)
            {
                existingValue.Multiplier += value.Multiplier;
            }
            else
            {
                expressionValues.AddLast(new ExpressionValue(value.Multiplier, value.Exponent));
            }
        }
        return this;
    }

    public Expression Multiply(Expression exp)
    {
        Expression newExpression = new Expression("0");

        foreach (ExpressionValue value1 in expressionValues)
        {
            foreach (ExpressionValue value2 in exp.expressionValues)
            {
                BigInteger newMultiplier = value1.Multiplier * value2.Multiplier;
                int newExponent = value1.Exponent + value2.Exponent;
                ExpressionValue newValue = new ExpressionValue(newMultiplier, newExponent);
                Expression tempExpression = new Expression("0");
                tempExpression.expressionValues.AddLast(newValue);
                newExpression.Add(tempExpression);
            }
        }

        expressionValues = newExpression.expressionValues;
        return this;
    }

    private ExpressionValue? FindValueWithExponent(int exponent)
    {
        foreach (ExpressionValue value in expressionValues)
        {
            if (value.Exponent == exponent)
            {
                return value;
            }
        }
        return null;
    }

    public override string ToString()
    {
        string result = "";
        LinkedListNode<ExpressionValue>? currentNode = expressionValues.Last;

        while (currentNode != null)
        {
            ExpressionValue value = currentNode.Value;

            if (value.Multiplier != 0)
            {
                string term = "";

                if (value.Multiplier != 1)
                {
                    term += $"{value.Multiplier}*";
                }

                if (value.Exponent == 0)
                {
                    term += "1";
                }
                else
                {
                    term += $"x^{value.Exponent}";
                }

                result += term + " + ";
            }

            currentNode = currentNode.Previous;
        }

        // Remove the trailing " + " from the last term
        result = result.TrimEnd(' ', '+');
        result = result == "" ? "0" : result; 
        return result;
    }

    private class ExpressionValue
    {
        public BigInteger Multiplier { get; set; }
        public int Exponent { get; }

        public ExpressionValue(BigInteger multiplier, int exponent)
        {
            Multiplier = multiplier;
            Exponent = exponent;
        }
    }
}
