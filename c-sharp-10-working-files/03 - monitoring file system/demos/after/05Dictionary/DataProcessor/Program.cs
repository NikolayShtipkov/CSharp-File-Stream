﻿using DataProcessor;
using static System.Console;

WriteLine("Parsing command line options");

var directoryToWatch = args[0];

// Command line validation omitted for brevity

if (!Directory.Exists(directoryToWatch))
{
    WriteLine($"ERROR: {directoryToWatch} does not exist");
    WriteLine("Press enter to quit.");
    ReadLine();
    return;
}

WriteLine($"Watching directory {directoryToWatch} for changes");
using var inputFileWatcher = new FileSystemWatcher(directoryToWatch);
using var timer = new Timer(ProcessFiles, null, 0, 1000);

inputFileWatcher.IncludeSubdirectories = false;
inputFileWatcher.InternalBufferSize = 32_768; // 32 KB
inputFileWatcher.Filter = "*.*"; // this is the default
inputFileWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;

inputFileWatcher.Created += FileCreated;
inputFileWatcher.Changed += FileChanged;
inputFileWatcher.Deleted += FileDeleted;
inputFileWatcher.Renamed += FileRenamed;
inputFileWatcher.Error += WatcherError;

inputFileWatcher.EnableRaisingEvents = true;

WriteLine("Press enter to quit.");
ReadLine();

static void FileCreated(object sender, FileSystemEventArgs e)
{
    WriteLine($"* File created: {e.Name} - type: {e.ChangeType}");

    FilesToProcess.Files.TryAdd(e.FullPath, e.FullPath);
}

static void FileChanged(object sender, FileSystemEventArgs e)
{
    WriteLine($"* File changed: {e.Name} - type: {e.ChangeType}");

    FilesToProcess.Files.TryAdd(e.FullPath, e.FullPath);
}

static void FileDeleted(object sender, FileSystemEventArgs e)
{
    WriteLine($"* File deleted: {e.Name} - type: {e.ChangeType}");
}

static void FileRenamed(object sender, RenamedEventArgs e)
{
    WriteLine($"* File renamed: {e.OldName} to {e.Name} - type: {e.ChangeType}");
}

static void WatcherError(object sender, ErrorEventArgs e)
{
    WriteLine($"ERROR: file system watching may no longer be active: {e.GetException()}");
}


static void ProcessFiles(object stateInfo)
{
    foreach (var fileName in FilesToProcess.Files.Keys)
    {
        if (FilesToProcess.Files.TryRemove(fileName, out _))
        {
            var fileProcessor = new FileProcessor(fileName);
            fileProcessor.Process();
        }
    }
}

