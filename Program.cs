


using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

class Program
{
    static void Main()
    {
        Console.Write("Enter folder path: ");
        string path = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(path) ||
            !Directory.Exists(path))
        {
            Console.WriteLine("Invalid folder path.");
            return;
        }

        var filesBySize = new Dictionary<long, List<string>>();

        foreach (string file in GetFiles(path))
        {
            try
            {
                long size = new FileInfo(file).Length;

                if (!filesBySize.ContainsKey(size))
                    filesBySize[size] = new List<string>();

                filesBySize[size].Add(file);
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
        }

        int duplicateGroups = 0;

        foreach (var sizeGroup in filesBySize.Values)
        {
            if (sizeGroup.Count < 2)
                continue;

            var filesByHash = new Dictionary<string, List<string>>();

            foreach (string file in sizeGroup)
            {
                try
                {
                    string hash = GetHash(file);

                    if (!filesByHash.ContainsKey(hash))
                        filesByHash[hash] = new List<string>();

                    filesByHash[hash].Add(file);
                }
                catch (IOException) { }
                catch (UnauthorizedAccessException) { }
            }

            foreach (var group in filesByHash.Values)
            {
                if (group.Count < 2)
                    continue;

                duplicateGroups++;
                Console.WriteLine($"\nDuplicate group {duplicateGroups}:");

                foreach (string file in group)
                    Console.WriteLine(file);
            }
        }

        if (duplicateGroups == 0)
            Console.WriteLine("No duplicates found.");
    }

    static IEnumerable<string> GetFiles(string path)
    {
        var pending = new Stack<string>();
        pending.Push(path);

        while (pending.Count > 0)
        {
            string directory = pending.Pop();

            string[] files;
            try
            {
                files = Directory.GetFiles(directory);
            }
            catch (IOException) { continue; }
            catch (UnauthorizedAccessException) { continue; }

            foreach (string file in files)
                yield return file;

            string[] folders;
            try
            {
                folders = Directory.GetDirectories(directory);
            }
            catch (IOException) { continue; }
            catch (UnauthorizedAccessException) { continue; }

            foreach (string folder in folders)
                pending.Push(folder);
        }
    }

    static string GetHash(string file)
    {
        using (SHA256 sha = SHA256.Create())
        using (FileStream stream = File.OpenRead(file))
        {
            return BitConverter.ToString(sha.ComputeHash(stream));
        }
    }
}
