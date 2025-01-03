namespace LearnSpace.Common
{
    public class DataConstants
    {
        public static class Student
        {
            public const int FirstNameMaxLen = 100;
            public const int FirstNameMinLen = 3;

            public const int LastNameMaxLen = 100;
            public const int LastNameMinLen = 3;

        }

        public static class Teacher
        {
            public const int FirstNameMaxLen = 100;
            public const int FirstNameMinLen = 3;

            public const int LastNameMaxLen = 100;
            public const int LastNameMinLen = 3;

            public const int SubjectSpecializationMaxLen = 200;
            public const int SubjectSpecializationMinLen = 3;
        }

        public static class Assignment
        {
            public const int TitleMaxLen = 200;
            public const int TitleMinLen = 3;

            public const int DescriptionMaxLen = 300;
            public const int DescriptionMinLen = 5;
        }

        public static class Course
        {
            public const int NameMaxLen = 100;
            public const int NameMinLen = 3;

            public const int DescriptionMaxLen = 300;
            public const int DescriptionMinLen = 5;

            public const int GroupCapacityMin = 5;
            public const int GroupCapacityMax = 50;
        }

        public static class Grade
        {
            public const int ScoreMax = 6;
            public const int ScoreMin = 2;

            public const int DescriptionMaxLen = 300;
            public const int DescriptionMinLen = 5;
        }

        public static class Notification
        {
            public const int MessageMaxLen = 300;
            public const int MessageMinLen = 5;
        }

        public static class ApplicationUser
        {
            public const int FirstNameMaxLen = 150;
            public const int FirstNameMinLen = 3;

            public const int LastNameMaxLen = 200;
            public const int LastNameMinLen = 3;
        }

        public static class Submission
        {
            public const int FileMaxSize = 5 * 1024 * 1024;
        }
    }
}
