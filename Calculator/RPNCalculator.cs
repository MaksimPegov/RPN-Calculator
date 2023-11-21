namespace Calculator;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class RPNCalculator
{
    public static double Calculate(string input)
    {
        var tokens = Tokenize(input);
        var rpn = ConvertToRPN(tokens);
        return EvaluateRPN(rpn);
    }

  private static List<string> Tokenize(string input)
  {
      return Regex.Matches(input, @"[\+\-\*/\(\)]|\d+")
                  .Cast<Match>()
                  .Select(match => match.Value)
                  .ToList();
  }


    private static List<string> ConvertToRPN(List<string> infixTokens)
    {
        var output = new List<string>();
        var operatorStack = new Stack<string>();

        foreach (var token in infixTokens)
        {
            if (double.TryParse(token, out _))
            {
                output.Add(token);
            }
            else if (IsOperator(token))
            {
                while (operatorStack.Count > 0 && Precedence(operatorStack.Peek()) >= Precedence(token))
                {
                    output.Add(operatorStack.Pop());
                }
                operatorStack.Push(token);
            }
            else if (token == "(")
            {
                operatorStack.Push(token);
            }
            else if (token == ")")
            {
                while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                {
                    output.Add(operatorStack.Pop());
                }
                operatorStack.Pop();
            }
        }

        while (operatorStack.Count > 0)
        {
            output.Add(operatorStack.Pop());
        }

        return output;
    }

    private static double EvaluateRPN(List<string> rpnTokens)
    {
        var operandStack = new Stack<double>();

        foreach (var token in rpnTokens)
        {
            if (double.TryParse(token, out var operand))
            {
                operandStack.Push(operand);
            }
            else if (IsOperator(token))
            {
                var operand2 = operandStack.Pop();
                var operand1 = operandStack.Pop();
                operandStack.Push(ApplyOperator(token, operand1, operand2));
            }
        }

        return operandStack.Pop();
    }

    private static bool IsOperator(string token)
    {
        return token == "+" || token == "-" || token == "*" || token == "/";
    }

    private static int Precedence(string op)
    {
        switch (op)
        {
            case "+":
            case "-":
                return 1;
            case "*":
            case "/":
                return 2;
            default:
                return 0;
        }
    }

    private static double ApplyOperator(string op, double operand1, double operand2)
    {
        switch (op)
        {
            case "+":
                return operand1 + operand2;
            case "-":
                return operand1 - operand2;
            case "*":
                return operand1 * operand2;
            case "/":
                return operand1 / operand2;
            default:
                throw new ArgumentException("Invalid operator: " + op);
        }
    }
}

