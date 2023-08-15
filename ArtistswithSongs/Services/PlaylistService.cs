using ArtistswithSongs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistswithSongs.Services
{
    public class PlaylistService
    {
        private readonly AwSContext _context;

        public PlaylistService(AwSContext context)
        {
            _context = context;
        }

        public void AddPlaylist(Playlist playlist)
        {
            _context.Playlists.Add(playlist);
            _context.SaveChanges();
        }

        public List<Playlist> GetAllPlaylists()
        {
            return _context.Playlists.ToList();
        }

        public Playlist GetPlaylistById(int playlistId)
        {
            return _context.Playlists.Find(playlistId);
        }

        public void UpdatePlaylist(Playlist playlist)
        {
            _context.Playlists.Update(playlist);
            _context.SaveChanges();
        }

        public void DeletePlaylist(int playlistId)
        {
            var playlistToDelete = _context.Playlists.Find(playlistId);
            if (playlistToDelete != null)
            {
                _context.Playlists.Remove(playlistToDelete);
                _context.SaveChanges();
            }
        }
    }
}
