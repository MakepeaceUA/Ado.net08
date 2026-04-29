using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp62.models
{
    public class Player
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Country { get; set; } = null!;
        public int ShirtNumber { get; set; }
        public string Position { get; set; } = null!;
        public int TeamId { get; set; }
        public virtual Team Team { get; set; } = null!;
    }
}
