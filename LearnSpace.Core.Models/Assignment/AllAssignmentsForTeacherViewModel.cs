using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnSpace.Core.Models.Assignment
{
    public class AllAssignmentsForTeacherViewModel
    {
        public List<AssignmentForTeacherModel> Assignments { get; set; }
        public int ClassId { get; set; }
    }
}
