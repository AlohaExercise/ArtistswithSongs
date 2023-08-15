using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistswithSongs.Models
{
    public class Playlist : Base
    {
        public string PlaylistName { get; set; }
        public string Description { get; set; }

        // Navigation property for PlaylistSongs
        //public ICollection<PlaylistSong> PlaylistSongs { get; set; }
    }
}
