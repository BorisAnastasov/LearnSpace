using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LearnSpace.Infrastructure.Database.Entities.Account;
using static LearnSpace.Common.DataConstants.Grade;

namespace LearnSpace.Infrastructure.Database.Entities
{
    public class Grade
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(ScoreMin,ScoreMax)]
        public int Score { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime DateGraded { get; set; }
        [Required]
        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        [Required]
        [ForeignKey(nameof(Student))]
        public Guid StudentId { get; set; }
        public virtual Student Student { get; set; } = null!;

    }

}
