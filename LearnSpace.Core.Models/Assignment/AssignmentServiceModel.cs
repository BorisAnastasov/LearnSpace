using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnSpace.Core.Models.Assignment
{
    public class AssignmentServiceModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DueDate { get; set; }
        public bool IsSubmitted { get; set; }
    }
}
