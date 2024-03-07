using System;

namespace LocationDVD.Location
{
    public class Locations
    {
        public int LocationId { get; set; }
        public int LeClient { get; set; }
        public int LeDVD { get; set; }
        public DateTime dateRented { get; set; }
        public DateTime? dateReturned { get; set; }

        //Rajouter pour la jointure
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Title { get; set; }
    }
}

