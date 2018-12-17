using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Models
{
    public class FullAuditory
    {
        public int Pair { get; set; }
        public int Day { get; set; }
        public int DayPeople { get; set; }
        public bool Is_everyweek { get; set; }
        public Auditory Odd { get; set; }
        public Auditory Even { get; set; }
    }
}
