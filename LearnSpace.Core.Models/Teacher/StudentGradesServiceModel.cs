using LearnSpace.Core.Models.Grade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnSpace.Core.Models.Teacher
{
    public class StudentGradesServiceModel
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public List<GradeServiceModel> Grades { get; set; }
    }
}
