using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp62.models
{
    public class Match
    {
        public int Id { get; set; }
        public int HomeTeamId { get; set; }
        public virtual Team HomeTeam { get; set; } = null!;
        public int AwayTeamId { get; set; }
        public virtual Team AwayTeam { get; set; } = null!;
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public DateTime MatchDate { get; set; }
        public virtual ICollection<MatchGoal> Goals { get; set; } = new List<MatchGoal>();
    }
}
