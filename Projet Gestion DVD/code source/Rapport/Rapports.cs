using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationDVD.Rapport
{
    public class Rapports
    {
        public int RapportId { get; set; }
        public DateTime DateGenerated { get; set; }
        public string Content { get; set; }
    }
}
