﻿using static System.Console;

namespace DataProcessor;

internal class FileProcessor
{
    public string InputFilePath { get; }

    public FileProcessor(string filePath) => InputFilePath = filePath;

    public void Process()
    {
        WriteLine($"Begin process of {InputFilePath}");

        // Check if file exists
        if (!File.Exists(InputFilePath))
        {
            WriteLine($"ERROR: file {InputFilePath} does not exist.");
            return;
        }
    }
}
