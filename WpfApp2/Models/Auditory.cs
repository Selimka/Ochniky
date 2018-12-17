using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Models
{
    public class Auditory
    {
        public String Group { get; set; }            
        public String Teacher { get; set; }
        public String Discipline { get; set; }
        public String Type { get; set; }
        public int Count { get; set; }

        public int Pair { get; set; }
        public int Day { get; set; }
        public int DayPeople { get; set; }
        public bool Is_everyweek { get; set; }
    }
}
