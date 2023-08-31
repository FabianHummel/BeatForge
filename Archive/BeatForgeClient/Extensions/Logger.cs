using System;

namespace BeatForgeClient.Extensions;

public static class Logger
{
    private static int _indentLevel;
    private const int IndentSize = 2; 
    
    public static void Task(string message)
    {
        Console.WriteLine(
            $"{new string(' ', IndentSize * _indentLevel++)}" +
            $"{message}");
    }
    
    public static void Complete(string message)
    {
        Console.WriteLine(
            $"{new string(' ', IndentSize * --_indentLevel)}" +
            $"done: {message}");
    }
}