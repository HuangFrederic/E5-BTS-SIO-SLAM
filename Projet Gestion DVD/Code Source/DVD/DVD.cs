using System.Windows.Media.Imaging;

namespace LocationDVD.DVD
{
    public class DVDs
    {
        public int DVDId { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear { get; set; }
        public int IsAvailable { get; set; }
        public string Image { get; set; }
        public BitmapImage ImageSource { get; set; }
    }
}

