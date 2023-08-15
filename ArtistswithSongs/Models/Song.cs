using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistswithSongs.Models
{
    public class Song : Base
    {
        public string SongTitle { get; set; }
        public int Duration { get; set; }
        public int ReleaseYear { get; set; }

        // Foreign key for Artist
        public int ArtistID { get; set; }
        // Navigation property for Artist
        public Artist Artist { get; set; }
    }
}
