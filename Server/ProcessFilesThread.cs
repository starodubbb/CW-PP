using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ProcessFilesThread
    {
        public List<string> directories;
        public InvertedIndex invertedIndex;
        public int filesPosStart;
        public int filesPosEnd;

        public ProcessFilesThread(List<string> directories, InvertedIndex invertedIndex, int filesPosStart, int filesPosEnd)
        {
            this.directories = directories;
            this.invertedIndex = invertedIndex;
            this.filesPosStart = filesPosStart;
            this.filesPosEnd = filesPosEnd;
        }

        public void ThreadFunction()
        {
            Stopwatch stopWatch = new Stopwatch();
            //Console.WriteLine($"Start measuring time {Thread.CurrentThread.ManagedThreadId}");

            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: posStart = {filesPosStart}, posEnd = {filesPosEnd}");
            stopWatch.Start();

            for (var i = filesPosStart; i < filesPosEnd; i++)
            {
                
                List<string> words = ReadFile.GetAllWordsFromFile(directories[i]);

                string fileName = Path.GetRelativePath(directories[i] + "\\..\\..\\..\\..\\", directories[i]);
                invertedIndex.AddWordsFromFileToInvertedIndex(fileName, words);             
            }

            stopWatch.Stop();
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} = {stopWatch.ElapsedMilliseconds} ms");
        }
    }
}
