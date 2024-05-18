namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> directories = ReadFile.GetAllFilePathes();
            InvertedIndex invertedIndex = new();

            Console.WriteLine("Choose the desired point:\n" +
                "1. Parallel scenario for inverted index\n" +
                "2. One thread scenario for inverted index\n" );
            
            bool loop = true;
            while (loop)
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        ScenariosInvertedIndex.ParallelScenarioInvertedIndex(directories, invertedIndex);
                        loop = false;
                        break;
                    case "2":
                        ScenariosInvertedIndex.OneExecutionScenarioInvertedIndex(directories, invertedIndex);
                        loop = false;
                        break;                   
                    default:
                        //Server server = new();
                        Console.WriteLine("Incorrect value");
                        break;
                }
            }


            Console.WriteLine("Choose the desired point:\n" +
                "1. Work with console\n" +
                "2. Start server\n");

            loop = true;
            while (loop)
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        ScenariosInvertedIndex.AskForFilesByWordsFromConsole(invertedIndex);
                        loop = false;
                        break;
                    case "2":
                        Server server = new(invertedIndex);
                        break;
                    default:
                        Console.WriteLine("Incorrect value");
                        break;
                }
            }




            //Console.WriteLine(Directory.GetCurrentDirectory());

            //Server server = new ();



            //server.invertedIndex.ParallelFillInvertedIndex(7, directories);


            //server.FillInvertedIndex(directories);


            //var indexDictionary = server.invertedIndex.indexDictionary;


            ////foreach (var item in indexDictionary)
            ////{
            ////    Console.Write($"{item.Key}: ");
            ////    foreach (var file in item.Value)
            ////    {
            ////        Console.Write($"{file}, ");
            ////    }
            ////    Console.WriteLine();
            ////}


            //List<string> allFilesByWord = server.invertedIndex.GetAllFileNamesByWord("recourse");


            //foreach (string file in allFilesByWord)
            //{
            //    Console.WriteLine(file);
            //}
        }
    }
}