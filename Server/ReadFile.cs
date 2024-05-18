using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class ReadFile
    {      
        public static List<string> GetAllWordsFromFile(string filePath)
        {
            List<char> DelimitingCharacters = new()
            { ' ', '\n', '\r', '\t', ',', '.', ':', ';', '(',  ')', '"', '/', '?', '!', '/', '&', '*', '%', '#', '$', '+', '-', '<', '>', '~', '@', '№', '^', '[', ']', '{', '}', '`'};

            string allTextFromFile = File.ReadAllText(filePath);
            return new List<string>(allTextFromFile.Split(DelimitingCharacters.ToArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower()).ToList());
        }

        public static List<string> GetAllFilePathes()
        {
            string docsDirectory = Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\Data\\";
            return Directory.GetFiles(docsDirectory, "*.txt",
                                         SearchOption.AllDirectories).ToList();
        }
    }
}

