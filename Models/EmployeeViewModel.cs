using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace temperature_analysis.Models
{
    public class EmployeeViewModel : StandardViewModel
    {

        public bool IsAdmin { get; set; }
        public int PersonID { get; set; }

    }
}