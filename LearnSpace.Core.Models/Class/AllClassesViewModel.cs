using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnSpace.Core.Models.Class
{
	public class AllClassesViewModel
	{
        public List<ClassInfoModel> Classes { get; set; } = new List<ClassInfoModel>();
        public int TotalClassesCount { get; set; }
    }
}
