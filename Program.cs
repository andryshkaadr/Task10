namespace Task10
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    class Program
    {
        private static ScoreManager scoreManager;
        private static StudentDataProcessor dataProcessor;
        private static DataIOManager dataIOManager;

        static Program()
        {
            AppDomain.CurrentDomain.ProcessExit += (sender, e) => SaveDataOnExit();
            scoreManager = new ScoreManager();
            dataProcessor = new StudentDataProcessor();
            dataIOManager = new DataIOManager();
            scoreManager.LoadData();
        }

        static void SaveDataOnExit()
        {
            scoreManager.SaveData();
            dataIOManager.SaveData(scoreManager.GetStudentsWithScores(), "students.bin", "students.json");
            Console.WriteLine("Данные сохранены при выходе (Binary и JSON).");
        }

        static void Main(string[] args)
        {
            // Пример создания массива из 1 миллиона объектов в 4 потока
            List<Student> students = dataProcessor.ParallelSort(CreateRandomStudentsArray(1000000, 4), (s1, s2) => s2.GetScore("Math").CompareTo(s1.GetScore("Math")));

            // Замер времени выполнения сортировки
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Пример параллельной сортировки массива по оценкам по математике
            dataProcessor.ParallelSort(students, (s1, s2) => s2.GetScore("Math").CompareTo(s1.GetScore("Math")));

            stopwatch.Stop();
            Console.WriteLine($"Массив отсортирован параллельно. Время выполнения: {stopwatch.ElapsedMilliseconds} миллисекунд");

            // Пример сохранения отсортированных данных
            dataIOManager.SaveData(students, "sorted_students.bin", "sorted_students.json");
        }

        // Метод для создания массива из 1 миллиона объектов в указанное количество потоков
        static List<Student> CreateRandomStudentsArray(int count, int numThreads)
        {
            List<Student> students = new List<Student>();
            object lockObj = new object();
            Parallel.For(0, count, new ParallelOptions { MaxDegreeOfParallelism = numThreads }, i =>
            {
                lock (lockObj)
                {
                    Random random = new Random();
                    string studentName = $"Student{i}";
                    Dictionary<string, int> scores = new Dictionary<string, int>
                    {
                        { "Math", random.Next(0, 101) },
                        { "Physics", random.Next(0, 101) },
                        { "English", random.Next(0, 101) }
                    };

                    scoreManager.AddStudent(studentName, scores);
                }
            });

            Console.WriteLine($"Массив из {count} объектов создан.");

            return scoreManager.GetStudentsWithScores();
        }
    }
}
