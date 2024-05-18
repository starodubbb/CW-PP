using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ScenariosInvertedIndex
    {
        public static void AskForFilesByWordsFromConsole(InvertedIndex invertedIndex)
        {
            while(true) 
            {
                Console.Write("Enter word: ");
                string word = Console.ReadLine().ToLower();

                List<string> listOfFiles = invertedIndex.GetAllFileNamesByWord(word);

                if (listOfFiles.Count != 0)
                {
                    foreach (var file in listOfFiles)
                    {
                        Console.WriteLine(file);
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("This word is not found");
                }
            }           
        }

        public static void ParallelScenarioInvertedIndex(List<string> directories, InvertedIndex invertedIndex)
        {
            int numberOfThreads;
            while (true)
            {
                Console.Write("Enter number of threads: ");
                string ?input = Console.ReadLine();               
                if (int.TryParse(input, out numberOfThreads) && numberOfThreads >= 2)
                    break;
                Console.WriteLine("Incorrect value");
            }

            invertedIndex.ParallelFillInvertedIndex(numberOfThreads, directories);
        }

        public static void OneExecutionScenarioInvertedIndex(List<string> directories, InvertedIndex invertedIndex)
        {
            invertedIndex.OneThreadFillInvertedIndex(directories);
        }

    }
}
