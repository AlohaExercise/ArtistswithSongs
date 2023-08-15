using ArtistswithSongs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistswithSongs.Services
{
    public class PlaylistSongService
    {
        private readonly AwSContext _context;

        public PlaylistSongService(AwSContext context)
        {
            _context = context;
        }

        public List<Song> GetSongsByPlaylistId(int playlistId)
        {
            return _context.PlaylistSongs
                .Where(ps => ps.PlaylistID == playlistId)
                .Select(ps => ps.Song)
                .ToList();
        }

        public void AddSongToPlaylist(int playlistId, int songId)
        {
            var playlistSong = new PlaylistSong
            {
                PlaylistID = playlistId,
                SongID = songId
            };
            _context.PlaylistSongs.Add(playlistSong);
            _context.SaveChanges();
        }

        public void RemoveSongFromPlaylist(int playlistId, int songId)
        {
            var playlistSong = _context.PlaylistSongs
                .SingleOrDefault(ps => ps.PlaylistID == playlistId && ps.SongID == songId);

            if (playlistSong != null)
            {
                _context.PlaylistSongs.Remove(playlistSong);
                _context.SaveChanges();
            }
        }
    }
}
