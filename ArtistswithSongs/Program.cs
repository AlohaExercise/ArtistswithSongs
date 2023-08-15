using ArtistswithSongs.Models;
using ArtistswithSongs.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ArtistswithSongs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
            .AddDbContext<AwSContext>()
            .AddScoped<ArtistService>()
            .AddScoped<SongService>()
            .AddScoped<PlaylistService>()
            .AddScoped<PlaylistSongService>()
            .BuildServiceProvider();

            var artistService = serviceProvider.GetRequiredService<ArtistService>();
            var songService = serviceProvider.GetRequiredService<SongService>();
            var playlistService = serviceProvider.GetRequiredService<PlaylistService>();
            var playlistSongService = serviceProvider.GetRequiredService<PlaylistSongService>();

            while (true)
            {
                Console.WriteLine("1. Tüm Şarkıları Listele");
                Console.WriteLine("2. Şarkı Ekle");
                Console.WriteLine("3. Şarkıyı Düzenle");
                Console.WriteLine("4. Şarkıyı Sil");
                Console.WriteLine("5. Tüm Sanatçıları Listele");
                Console.WriteLine("6. Sanatçı Ekle");
                Console.WriteLine("7. Sanatçıyı Düzenle");
                Console.WriteLine("8. Sanatçıyı Sil");
                Console.WriteLine("9. Tüm Çalma Listelerini Listele");
                Console.WriteLine("10. Çalma Listesini Görüntüle");
                Console.WriteLine("11. Çalma Listesi Ekle");
                Console.WriteLine("12. Çalma Listesini Düzenle");
                Console.WriteLine("13. Çalma Listesini Sil");
                Console.WriteLine("14. Çalma Listesine Şarkı Ekle");
                Console.WriteLine("15. Çalma Listesinden Şarkı Kaldır");
                Console.WriteLine("16. Çıkış");
                Console.Write("Bir opsiyon seçiniz: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int option))
                {
                    switch (option)
                    {
                        case 1:
                            ListAllSongs(artistService, songService);
                            break;
                        case 2:
                            AddSong(artistService, songService);
                            break;
                        case 3:
                            EditSong(artistService, songService);
                            break;
                        case 4:
                            DeleteSong(artistService, songService);
                            break;
                        case 5:
                            ListAllArtists(artistService);
                            break;
                        case 6:
                            AddArtist(artistService);
                            break;
                        case 7:
                            EditArtist(artistService);
                            break;
                        case 8:
                            DeleteArtist(artistService);
                            break;
                        case 9:
                            ListAllPlaylists(playlistService);
                            break;
                        case 10:
                            SelectFromPlaylists(playlistService, artistService, playlistSongService);
                            break;
                        case 11:
                            AddPlaylist(playlistService);
                            break;
                        case 12:
                            EditPlaylist(playlistService);
                            break;
                        case 13:
                            DeletePlaylist(playlistService);
                            break;
                        case 14:
                            AddSongToPlaylist(playlistService, artistService, songService, playlistSongService);
                            break;
                        case 15:
                            RemoveSongFromPlaylist(playlistService, artistService, songService, playlistSongService);
                            break;
                        case 16:
                            Console.WriteLine("Çıkış yapılıyor...");
                            return;
                        default:
                            Console.WriteLine("Geçersiz işlem. Lütfen tekrar deneyiniz.");
                            break;
                    }
                }
            }
        }

        private static void ListAllSongs(ArtistService artistService, SongService songService)
        {
            var songs = songService.GetAllSongs();
            Console.WriteLine("Tüm Şarkıları Listele:");
            foreach (var song in songs)
            {
                string artistName = artistService.GetArtistById(song.ArtistID).ArtistName;
                Console.WriteLine($"Şarkı ID: {song.Id}, Başlık: {song.SongTitle}, Süre: {song.Duration}, Çıkış Yılı: {song.ReleaseYear}, Sanatçı Adı: {artistName}");
            }
        }

        private static void AddSong(ArtistService artistService, SongService songService)
        {
            Console.Write("Şarkının adını yazınız: ");
            string title = Console.ReadLine();
            if (!string.IsNullOrEmpty(title))
            {
                Console.Write("şarkının süresini yazınız: ");
                if (int.TryParse(Console.ReadLine(), out int duration))
                {
                    Console.Write("Şarkının çıkış yılını yazınız: ");
                    if (int.TryParse(Console.ReadLine(), out int releaseYear))
                    {
                        ListAllArtists(artistService);
                        Console.Write("Şarkının sanatçısını listeden seçiniz (Sanatçı ID giriniz): ");
                        if (int.TryParse(Console.ReadLine(), out int artistId))
                        {
                            var artist = artistService.GetArtistById(artistId);
                            if (artist != null)
                            {
                                var newSong = new Song
                                {
                                    SongTitle = title,
                                    Duration = duration,
                                    ReleaseYear = releaseYear,
                                    ArtistID = artistId
                                };
                                songService.AddSong(newSong);
                                Console.WriteLine("Şarkı başarıyla eklendi!");
                            }
                            else
                            {
                                Console.WriteLine("Geçersiz Sanatçı ID. Şarkı eklenemedi.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Sanatçı ID için geçersiz girdi. Şarkı eklenemedi.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Çıkış Yılı için geçersiz girdi. Şarkı eklenemedi.");
                    }
                }
                else
                {
                    Console.WriteLine("Şarkı Süresi için geçersiz girdi. Şarkı eklenemedi.");
                }
            }
            else
            {
                Console.WriteLine("Şarkı Adı için geçersiz girdi. Şarkı eklenemedi.");
            }
        }

        private static void EditSong(ArtistService artistService, SongService songService)
        {
            ListAllSongs(artistService, songService);
            Console.Write("Şarkıyı listeden seçiniz (Şarkı ID giriniz): ");
            if (int.TryParse(Console.ReadLine(), out int songId))
            {
                var songToEdit = songService.GetSongById(songId);

                Console.Write("Şarkının adını değiştirmek için yeni şarkı adı yazınız (Boş bırakarak bu alanı pas geçebilirsiniz): ");
                string title = Console.ReadLine();
                if (!string.IsNullOrEmpty(title))
                {
                    songToEdit.SongTitle = title;
                }
                Console.Write("şarkının süresini istiyorsanız değer giriniz (Boş bırakarak bu alanı pas geçebilirsiniz): ");
                if (int.TryParse(Console.ReadLine(), out int duration))
                {
                    songToEdit.Duration = duration;
                }
                Console.Write("Şarkının çıkış yılını düzenlemek istiyorsanız değer giriniz (Boş bırakarak bu alanı pas geçebilirsiniz): ");
                if (int.TryParse(Console.ReadLine(), out int releaseYear))
                {
                    songToEdit.ReleaseYear = releaseYear;
                }
                ListAllArtists(artistService);
                Console.Write("Şarkının sanatçısını değiştirmek istiyorsanız listeden yeni Sanatçı ID değeri seçiniz (Boş bırakarak bu alanı pas geçebilirsiniz): ");
                if (int.TryParse(Console.ReadLine(), out int artistId))
                {
                    var artist = artistService.GetArtistById(artistId);
                    if (artist != null)
                    {
                        songToEdit.ArtistID = artistId;
                    }
                    else
                    {
                        Console.WriteLine("Geçersiz Sanatçı ID. Sanatçı alanı düzenlenmeden pas geçildi.");
                    }
                }
                songService.UpdateSong(songToEdit);
                Console.WriteLine("Şarkı başarıyla düzenlendi!");
            }
            else
            {
                Console.WriteLine("Şarkı ID için geçersiz girdi. Şarkı düzenlenemedi.");
            }
        }

        private static void DeleteSong(ArtistService artistService, SongService songService)
        {
            ListAllSongs(artistService, songService);
            Console.Write("Silinmesini istediğiniz şarkıyı listeden seçiniz (Şarkı ID giriniz): ");
            if (int.TryParse(Console.ReadLine(), out int songId))
            {
                Console.Write("Şarkıyı silmek istediğinize emin misiniz? (E/H)");
                string choice = Console.ReadLine();
                if (!string.IsNullOrEmpty(choice) && choice.ToUpper() == "E")
                {
                    songService.DeleteSong(songId);
                    Console.WriteLine("Şarkı başarıyla silindi!");
                }
            }
            else
            {
                Console.WriteLine("Şarkı ID için geçersiz girdi. Şarkı silinemedi.");
            }
        }

        private static void ListAllArtists(ArtistService artistService)
        {
            var artists = artistService.GetAllArtists();
            Console.WriteLine("Tüm Sanatçıları Listele:");
            foreach (var artist in artists)
            {
                Console.WriteLine($"Sanatçı ID: {artist.Id}, İsim: {artist.ArtistName}, Genre: {artist.Genre}");
            }
        }
        
        private static void AddArtist(ArtistService artistService)
        {
            Console.Write("Sanatçının adını ve soyadını yazınız:");
            string name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name))
            {
                Console.Write("Sanatçının genre türünü yazınız:");
                string genre = Console.ReadLine();
                if (!string.IsNullOrEmpty(genre))
                {
                    var newArtist = new Artist
                    {
                        ArtistName = name,
                        Genre = genre
                    };
                    artistService.AddArtist(newArtist);
                    Console.WriteLine("Sanatçı başarıyla eklendi!");
                }
                else
                {
                    Console.WriteLine("Geçersiz bir genre türü girdiniz. Sanatçı eklenemedi.");
                }
            }
            else
            {
                Console.WriteLine("Geçersiz bir isim girdiniz. Sanatçı eklenemedi.");
            }
        }

        private static void EditArtist(ArtistService artistService)
        {
            ListAllArtists(artistService);
            Console.Write("Sanatçıyı listeden seçiniz (Sanatçı ID giriniz): ");
            if (int.TryParse(Console.ReadLine(), out int artistId))
            {
                var artistToEdit = artistService.GetArtistById(artistId);

                Console.Write("Sanatçının adını değiştirmek için yeni sanatçı adı yazınız (Boş bırakarak bu alanı pas geçebilirsiniz): ");
                string name = Console.ReadLine();
                if (!string.IsNullOrEmpty(name))
                {
                    artistToEdit.ArtistName = name;
                }
                Console.Write("Sanatçının genre türünü değiştirmek istiyorsanız yeni değer giriniz (Boş bırakarak bu alanı pas geçebilirsiniz): ");
                string genre = Console.ReadLine();
                if (!string.IsNullOrEmpty(genre))
                {
                    artistToEdit.Genre = genre;
                }
                artistService.UpdateArtist(artistToEdit);
                Console.WriteLine("Sanatçı başarıyla düzenlendi!");
            }
            else
            {
                Console.WriteLine("Sanatçı ID için geçersiz girdi. Sanatçı düzenlenemedi.");
            }
        }

        private static void DeleteArtist(ArtistService artistService)
        {
            ListAllArtists(artistService);
            Console.Write("Silinmesini istediğiniz sanatçıyı listeden seçiniz (Sanatçı ID giriniz): ");
            if (int.TryParse(Console.ReadLine(), out int artistId))
            {
                Console.Write("Sanatçıyı silmek istediğinize emin misiniz? (E/H)");
                string choice = Console.ReadLine();
                if (!string.IsNullOrEmpty(choice) && choice.ToUpper() == "E")
                {
                    artistService.DeleteArtist(artistId);
                    Console.WriteLine("Sanatçı başarıyla silindi!");
                }
            }
            else
            {
                Console.WriteLine("Sanatçı ID için geçersiz girdi. Sanatçı silinemedi.");
            }
        }

        private static void ListAllPlaylists(PlaylistService playlistService)
        {
            var playlists = playlistService.GetAllPlaylists();
            Console.WriteLine("Tüm Çalma Listelerini Listele:");
            if (playlists.Count > 0)
            {
                foreach (var playlist in playlists)
                {
                    Console.WriteLine($"Playlist ID: {playlist.Id}, Liste Adı: {playlist.PlaylistName}, Açıklama: {playlist.Description}");
                }
            }
            else
            {
                Console.WriteLine("Sistemde çalma Listesi bulunamadı.");
            }

        }

        private static int SelectFromPlaylists(PlaylistService playlistService, ArtistService artistService, PlaylistSongService playlistSongService)
        {
            ListAllPlaylists(playlistService);
            Console.Write("Çalma listesini listeden seçiniz (Playlist ID giriniz): ");
            if (int.TryParse(Console.ReadLine(), out int playlistId))
            {
                var playlist = playlistService.GetPlaylistById(playlistId);
                var songs = playlistSongService.GetSongsByPlaylistId(playlistId);
                if (playlist != null)
                {
                    if (songs.Count > 0)
                    {
                        Console.WriteLine("-------------------------------------------------------------");
                        Console.WriteLine("Listedeki şarkılar:");
                        foreach (var song in songs)
                        {
                            string artistName = artistService.GetArtistById(song.ArtistID).ArtistName;
                            Console.WriteLine($"Şarkı ID: {song.Id}, Başlık: {song.SongTitle}, Süre: {song.Duration}, Çıkış Yılı: {song.ReleaseYear}, Sanatçı Adı: {artistName}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Çalma listesi boş.");
                    }
                }
                else
                {
                    Console.WriteLine("Geçersiz Playlist ID. Çalma listesi eklenemedi.");
                }
            }
            else
            {
                Console.WriteLine("Playlist ID için geçersiz girdi. Çalma listesi eklenemedi.");
            }
            return playlistId;
        }

        private static void AddPlaylist(PlaylistService playlistService)
        {
            Console.Write("Çalma listesinin adını yazınız:");
            string name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name))
            {
                Console.Write("Çalma listesinin açıklamasını yazınız:");
                string desc = Console.ReadLine();
                if (!string.IsNullOrEmpty(desc))
                {
                    var newPlaylist = new Playlist
                    {
                        PlaylistName = name,
                        Description = desc
                    };
                    playlistService.AddPlaylist(newPlaylist);
                    Console.WriteLine("Sanatçı başarıyla eklendi!");
                }
                else
                {
                    Console.WriteLine("Geçersiz bir açıklama girdiniz. Çalma listesi eklenemedi.");
                }
            }
            else
            {
                Console.WriteLine("Geçersiz bir isim girdiniz. Çalma listesi eklenemedi.");
            }
        }

        private static void EditPlaylist(PlaylistService playlistService)
        {
            ListAllPlaylists(playlistService);
            Console.Write("Çalma listesini listeden seçiniz (Sanatçı ID giriniz): ");
            if (int.TryParse(Console.ReadLine(), out int playlistId))
            {
                var playlistToEdit = playlistService.GetPlaylistById(playlistId);

                Console.Write("Çalma listesinin adını değiştirmek için yeni çalma listesi adı yazınız (Boş bırakarak bu alanı pas geçebilirsiniz): ");
                string name = Console.ReadLine();
                if (!string.IsNullOrEmpty(name))
                {
                    playlistToEdit.PlaylistName = name;
                }
                Console.Write("Çalma listesinin açıklamasını değiştirmek istiyorsanız yeni değer giriniz (Boş bırakarak bu alanı pas geçebilirsiniz): ");
                string desc = Console.ReadLine();
                if (!string.IsNullOrEmpty(desc))
                {
                    playlistToEdit.Description = desc;
                }
                playlistService.UpdatePlaylist(playlistToEdit);
                Console.WriteLine("Çalma listesi başarıyla düzenlendi!");
            }
            else
            {
                Console.WriteLine("Çalma Listesi ID için geçersiz girdi. Çalma listesi düzenlenemedi.");
            }
        }

        private static void DeletePlaylist(PlaylistService playlistService)
        {
            ListAllPlaylists(playlistService);
            Console.Write("Silinmesini istediğiniz çalma listesini listeden seçiniz (Çalma Listesi ID giriniz): ");
            if (int.TryParse(Console.ReadLine(), out int playlistId))
            {
                Console.Write("Çalma listesini silmek istediğinize emin misiniz? (E/H)");
                string choice = Console.ReadLine();
                if (!string.IsNullOrEmpty(choice) && choice.ToUpper() == "E")
                {
                    playlistService.DeletePlaylist(playlistId);
                    Console.WriteLine("Çalma listesi başarıyla silindi!");
                }
            }
            else
            {
                Console.WriteLine("Çalma Listesi ID için geçersiz girdi. Çalma Listesi silinemedi.");
            }
        }

        private static void AddSongToPlaylist(PlaylistService playlistService, ArtistService artistService, SongService songService, PlaylistSongService playlistSongService)
        {

            var playlistId = SelectFromPlaylists(playlistService, artistService, playlistSongService);
            ListAllSongs(artistService, songService);
            Console.Write("Şarkıyı listeden seçiniz (Şarkı ID giriniz): ");
            if (int.TryParse(Console.ReadLine(), out int songId))
            {
                if (songId != null)
                {
                    var songsInPlayList = playlistSongService.GetSongsByPlaylistId(playlistId).Select(s => s.Id);
                    if (!songsInPlayList.Contains(songId))
                    {
                        playlistSongService.AddSongToPlaylist(playlistId, songId);
                        Console.WriteLine("Şarkı çalma listesine başarıyla eklendi!");
                    }
                    else
                    {
                        Console.WriteLine("Eklemek istediğiniz şarkı zaten listede mevcut.");
                    }
                }
                else
                {
                    Console.WriteLine("Geçersiz Şarkı ID. Şarkı çalma listesine eklenemedi.");
                }
            }
            else
            {
                Console.WriteLine("Şarkı ID için geçersiz girdi. Şarkı çalma listesine eklenemedi.");
            }

        }

        private static void RemoveSongFromPlaylist(PlaylistService playlistService, ArtistService artistService, SongService songService, PlaylistSongService playlistSongService)
        {

            var playlistId = SelectFromPlaylists(playlistService, artistService, playlistSongService);
            var songsInList = playlistSongService.GetSongsByPlaylistId(playlistId);
            if (songsInList.Count > 0)
            {
                Console.Write("Kaldırılacak şarkıyı listeden seçiniz (Şarkı ID giriniz): ");
                if (int.TryParse(Console.ReadLine(), out int songId))
                {
                    if (songId != null)
                    {
                        playlistSongService.RemoveSongFromPlaylist(playlistId, songId);
                        Console.WriteLine("Şarkı çalma listesinden başarıyla kaldırıldı!");
                    }
                    else
                    {
                        Console.WriteLine("Geçersiz Şarkı ID. Şarkı çalma listesine eklenemedi.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Çalma listesi boş.");
            }
        }
    }
}