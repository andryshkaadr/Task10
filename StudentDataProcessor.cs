namespace Task10
{
    using System;
    using System.Collections.Generic;

    class StudentDataProcessor
    {
        public List<Student> ParallelSort(List<Student> students, ScoreManager.ComparisonDelegate comparison)
        {
            return students.AsParallel().OrderBy(s => s, new StudentComparer(comparison)).ToList();
        }

        public List<Student> Filter(List<Student> students, Func<Student, bool> filter)
        {
            return students.Where(filter).ToList();
        }
    }
}
