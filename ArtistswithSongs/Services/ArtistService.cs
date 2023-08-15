using ArtistswithSongs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistswithSongs.Services
{
    public class ArtistService
    {
        private readonly AwSContext _context;

        public ArtistService(AwSContext context)
        {
            _context = context;
        }

        public void AddArtist(Artist artist)
        {
            _context.Artists.Add(artist);
            _context.SaveChanges();
        }

        public List<Artist> GetAllArtists()
        {
            return _context.Artists.ToList();
        }

        public Artist GetArtistById(int artistId)
        {
            return _context.Artists.Find(artistId);
        }

        public void UpdateArtist(Artist artist)
        {
            _context.Artists.Update(artist);
            _context.SaveChanges();
        }

        public void DeleteArtist(int artistId)
        {
            var artistToDelete = _context.Artists.Find(artistId);
            if (artistToDelete != null)
            {
                _context.Artists.Remove(artistToDelete);
                _context.SaveChanges();
            }
        }
    }
}
