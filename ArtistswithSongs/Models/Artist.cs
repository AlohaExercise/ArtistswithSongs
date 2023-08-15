using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistswithSongs.Models
{
    public class Artist : Base
    {
        public string ArtistName { get; set; }
        public string Genre { get; set; }

        // Navigation property for Songs
        //public ICollection<Song> Songs { get; set; }
    }
}
