using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class InvertedIndex
    {
        // (слово, список файлів з цим словом)
        public ConcurrentDictionary<string, List<string>> indexDictionary = new();

        public void AddWordsFromFileToInvertedIndex(string fileName, List<string> words)
        {
            foreach (var word in words)
            {
                if (indexDictionary.ContainsKey(word))
                {
                    if (!indexDictionary[word].Contains(fileName))
                    {
                        indexDictionary[word].Add(fileName);
                    }
                }
                else
                {
                    indexDictionary[word] = new List<string>();
                    indexDictionary[word].Add(fileName);
                }
            }
        }

        public List<string> GetAllFileNamesByWord (string word)
        {
            indexDictionary.TryGetValue(word, out List<string> result);
            if (result != null)
            {
                return result;
            }
            return new List<string>();
        }

        public void OneThreadFillInvertedIndex(List<string> directories)
        {
            Stopwatch stopWatch = new Stopwatch();
            //Console.WriteLine("Start measuring time (one thread execution)");
            stopWatch.Start();

            foreach (var directory in directories)
            {
                List<string> words = ReadFile.GetAllWordsFromFile(directory);

                string fileName = Path.GetRelativePath(directory + "\\..\\..\\..\\..\\", directory);
                AddWordsFromFileToInvertedIndex(fileName, words);
            }

            stopWatch.Stop();
            Console.WriteLine($"One thread execution time = {stopWatch.ElapsedMilliseconds} ms\n");
        }

        public void ParallelFillInvertedIndex(int threadsAmount, List<string> directories)
        {
            Stopwatch stopWatch = new Stopwatch();
            //Console.WriteLine($"Start measuring time");
            stopWatch.Start();

            int filesPosStart = 0;
            int filesPosEnd = 0;
            Thread[] threads = new Thread[threadsAmount];

            int directoriesAmount = directories.Count;

            if (threadsAmount >= 2 && directoriesAmount >= threadsAmount)
            {
                int threadFilesAmount1 = directoriesAmount / threadsAmount;
                int threadFilesAmount2 = threadFilesAmount1 + 1;
                int threadsAmount2 = directoriesAmount - threadFilesAmount1 * threadsAmount;
                int threadsAmount1 = threadsAmount - threadsAmount2;

                Console.WriteLine($"Directories = {directoriesAmount}");
                Console.WriteLine($"threadsAmount1 = {threadsAmount1}, threadFilesAmount1 = {threadFilesAmount1}");
                Console.WriteLine($"threadsAmount2 = {threadsAmount2}, threadFilesAmount2 = {threadFilesAmount2}");

                for (int i = 0; i < threadsAmount1; i++)
                {
                    filesPosEnd = filesPosStart + threadFilesAmount1 - 1;
                    ProcessFilesThread threadArgument = new ProcessFilesThread(directories, this, filesPosStart, filesPosEnd);
                    threads[i] = new Thread(new ThreadStart(threadArgument.ThreadFunction));
                    threads[i].Start();
                    filesPosStart = filesPosEnd + 1;
                }
                for (int i = threadsAmount1; i < threadsAmount; i++)
                {
                    filesPosEnd = filesPosStart + threadFilesAmount2 - 1;
                    ProcessFilesThread threadArgument = new ProcessFilesThread(directories, this, filesPosStart, filesPosEnd);
                    threads[i] = new Thread(new ThreadStart(threadArgument.ThreadFunction));
                    threads[i].Start();
                    filesPosStart = filesPosEnd + 1;
                }

                for (int i = 0; i < threadsAmount; i++)
                {
                    threads[i].Join();
                }
            }

            stopWatch.Stop();
            Console.WriteLine($"All parallel execution time = {stopWatch.ElapsedMilliseconds} ms\n");
        }
    }
}
