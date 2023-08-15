using ArtistswithSongs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistswithSongs.Services
{
    public class SongService
    {
        private readonly AwSContext _context;

        public SongService(AwSContext context)
        {
            _context = context;
        }

        public void AddSong(Song song)
        {
            _context.Songs.Add(song);
            _context.SaveChanges();
        }

        public List<Song> GetAllSongs()
        {
            return _context.Songs.ToList();
        }

        public Song GetSongById(int songId)
        {
            return _context.Songs.Find(songId);
        }

        public void UpdateSong(Song song)
        {
            _context.Songs.Update(song);
            _context.SaveChanges();
        }

        public void DeleteSong(int songId)
        {
            var songToDelete = _context.Songs.Find(songId);
            if (songToDelete != null)
            {
                _context.Songs.Remove(songToDelete);
                _context.SaveChanges();
            }
        }
    }
}
