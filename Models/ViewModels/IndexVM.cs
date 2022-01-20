using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    // 156. Создать в ней класс IndexVM
    public class IndexVM
    {
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set;}
        public int RentDays { get; set; } = 1;
    }
}
