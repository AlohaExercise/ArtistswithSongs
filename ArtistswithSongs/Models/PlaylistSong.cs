using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistswithSongs.Models
{
    public class PlaylistSong
    {
        // Composite primary key
        public int PlaylistID { get; set; }
        public int SongID { get; set; }

        // Navigation properties
        public Playlist Playlist { get; set; }
        public Song Song { get; set; }
    }
}
