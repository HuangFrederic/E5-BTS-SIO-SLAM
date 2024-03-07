using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationDVD.Retour
{
    public class Retours
    {
        public int RetourId { get; set; }
        public int LaLocation { get; set; }
        public DateTime? DateReturned { get; set; }
        public decimal LocationPrix { get; set; }
        public int Retourner { get; set; }

        //Rajouter pour la jointure
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Title { get; set; }
    }
}
