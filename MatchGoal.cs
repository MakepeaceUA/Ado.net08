using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp62.models
{
    public class MatchGoal
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public virtual Match Match { get; set; } = null!;
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; } = null!;
        public int MinuteScored { get; set; }
    }
}
