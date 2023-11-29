namespace Task10
{
    using System.Collections.Generic;

    class StudentComparer : IComparer<Student>
    {
        private ScoreManager.ComparisonDelegate comparison;

        public StudentComparer(ScoreManager.ComparisonDelegate comparison)
        {
            this.comparison = comparison;
        }

        public int Compare(Student student1, Student student2)
        {
            return comparison(student1, student2);
        }
    }
}
