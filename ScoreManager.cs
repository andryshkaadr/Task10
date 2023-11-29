namespace Task10
{
    using System;
    using System.Collections.Generic;

    class ScoreManager
    {
        private Dictionary<string, Student> Students { get; set; }

        public ScoreManager()
        {
            Students = new Dictionary<string, Student>();
        }

        public delegate int ComparisonDelegate(Student student1, Student student2);

        public void AddStudent(string name, Dictionary<string, int> scores)
        {
            if (!Students.ContainsKey(name))
            {
                var student = new Student(name);
                foreach (var subject in scores.Keys)
                {
                    student.AddScore(subject, scores[subject]);
                }
                Students[name] = student;
            }
        }

        public void RemoveStudent(string name)
        {
            if (Students.ContainsKey(name))
            {
                Students.Remove(name);
            }
        }

        public void AddScore(string studentName, string subject, int score)
        {
            if (Students.ContainsKey(studentName))
            {
                Students[studentName].AddScore(subject, score);
            }
        }

        public void RemoveScore(string studentName, string subject)
        {
            if (Students.ContainsKey(studentName))
            {
                Students[studentName].RemoveScore(subject);
            }
        }

        public int GetStudentScore(string studentName, string subject)
        {
            return Students.ContainsKey(studentName) ? Students[studentName].GetScore(subject) : -1;
        }

        public List<Student> GetStudentsWithScores()
        {
            return new List<Student>(Students.Values);
        }

        public void ParallelSort(ComparisonDelegate comparison)
        {
            List<Student> sortedStudents = Students.Values.AsParallel().OrderBy(s => s, new StudentComparer(comparison)).ToList();
            DisplayStudents("Отсортированные студенты:", sortedStudents);
        }

        public List<Student> Filter(Func<Student, bool> filter)
        {
            return Students.Values.Where(filter).ToList();
        }

        public void DisplayStudents(string message, List<Student> students)
        {
            Console.WriteLine(message);
            foreach (var student in students)
            {
                Console.WriteLine($"{student.Name}'s Scores:");
                foreach (var item in student.GetScores())
                {
                    Console.WriteLine($"{item.Key}: {item.Value}");
                }
                Console.WriteLine();
            }
        }

        public void SaveData()
        {
            DataSerializer.SaveBinary(Students, "students.bin");
            DataSerializer.SaveJson(Students, "students.json");
        }

        public void LoadData()
        {
            Students = DataSerializer.LoadBinary<Dictionary<string, Student>>("students.bin") ?? new Dictionary<string, Student>();
            if (Students.Count == 0)
            {
                Students = DataSerializer.LoadJson<Dictionary<string, Student>>("students.json") ?? new Dictionary<string, Student>();
            }
        }
    }
}
