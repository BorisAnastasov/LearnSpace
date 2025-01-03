using LearnSpace.Core.Models.Enumerations;

namespace LearnSpace.Core.Models.Class
{
    public class AllClassesQueryModel
    {
        public int ClassesPerPage { get; } = 15;

        public string SearchTerm { get; init; } = null!;

        public ClassSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalClassesCount { get; set; }

        public IEnumerable<string> SortingCategories { get; set; } = null!;

        public IEnumerable<ClassInfoModel> Classes { get; set; }
                        = new List<ClassInfoModel>();
    }
}
