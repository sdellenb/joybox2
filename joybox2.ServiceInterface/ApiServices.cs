using System;
using System.Linq;
using joybox2.ServiceModel;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace joybox2.ServiceInterface {
    public class ApiServices : Service {
        private readonly OrmLiteConnectionFactory dbFactory;

        public ApiServices(IDbConnectionFactory dbFactory) {
            this.dbFactory = (OrmLiteConnectionFactory)dbFactory;

            using (var db = this.dbFactory.Open()) {
                #region Initialize with some hardcoded data.
                if (db.CreateTableIfNotExists<Category>()) {
                    db.Insert(new Category { Name = "audiobooks", Thumbnail = "/svg/audiobooks.svg" });
                    db.Insert(new Category { Name = "music", Thumbnail = "/svg/music.svg" });
                }
                if (db.CreateTableIfNotExists<Album>()) {
                    var audiobooksCategoryId = db.Single<Category>(c => c.Name == "audiobooks").Id;
                    var musicCategoryId = db.Single<Category>(c => c.Name == "music").Id;
                    db.Insert(new Album {
                        Name = "Alexander Steffensmeier - Ein Geburtstagsfest für Lieselotte",
                        Thumbnail = Guid.Parse("b1544882-d3bb-465e-a24c-f3c5a6ce535c"),
                        CategoryId = audiobooksCategoryId
                    });
                    db.Insert(new Album {
                        Name = "Alexander Steffensmeier - Lieselotte bleibt wach",
                        Thumbnail = Guid.Parse("043ac73f-8fc5-4172-be64-63ffc15cf7da"),
                        CategoryId = audiobooksCategoryId
                    });
                    db.Insert(new Album {
                        Name = "Alexander Steffensmeier - Lieselotte im Schnee",
                        Thumbnail = Guid.Parse("539e88c5-b1dd-4f53-b01b-1d607818461b"),
                        CategoryId = audiobooksCategoryId
                    });
                    db.Insert(new Album {
                        Name = "Bob Marley & The Wailers - One Love",
                        Thumbnail = Guid.Parse("690bdc80-7f45-4b19-8711-5da51a532927"),
                        CategoryId = musicCategoryId
                    });
                }
                if (db.CreateTableIfNotExists<Track>()) {
                    db.Insert(new Track {
                        TrackIndex = 1,
                        Path = @"media\audiobooks\Alexander Steffensmeier - Ein Geburtstagsfest für Lieselotte\001_010_9783732447169_DEUE11619029.mp3",
                        AlbumId = 1
                    });
                    db.Insert(new Track {
                        TrackIndex = 2,
                        Path = @"media\audiobooks\Alexander Steffensmeier - Ein Geburtstagsfest für Lieselotte\002_010_9783732447169_DEUE11619030.mp3",
                        AlbumId = 1
                    });
                    db.Insert(new Track {
                        TrackIndex = 3,
                        Path = @"media\audiobooks\Alexander Steffensmeier - Ein Geburtstagsfest für Lieselotte\003_010_9783732447169_DEUE11619031.mp3",
                        AlbumId = 1
                    });
                    db.Insert(new Track {
                        TrackIndex = 1,
                        Path = @"media\audiobooks\Alexander Steffensmeier - Lieselotte bleibt wach\008_014_9783732447121_DEUD91899509.mp3",
                        AlbumId = 2
                    });
                    db.Insert(new Track {
                        TrackIndex = 2,
                        Path = @"media\audiobooks\Alexander Steffensmeier - Lieselotte bleibt wach\009_014_9783732447121_DEUD91899510.mp3",
                        AlbumId = 2
                    });
                    db.Insert(new Track {
                        TrackIndex = 1,
                        Path = @"media\audiobooks\Alexander Steffensmeier - Lieselotte im Schnee\003_014_9783732447121_DEUD91899504.mp3",
                        AlbumId = 3
                    });
                    db.Insert(new Track {
                        TrackIndex = 2,
                        Path = @"media\audiobooks\Alexander Steffensmeier - Lieselotte im Schnee\004_014_9783732447121_DEUD91899505.mp3",
                        AlbumId = 3
                    });
                    db.Insert(new Track {
                        TrackIndex = 1,
                        Path = @"media\music\Bob Marley & The Wailers - One Love\(05) No Woman No Cry (live).mp3",
                        AlbumId = 4
                    });
                    db.Insert(new Track {
                        TrackIndex = 2,
                        Path = @"media\music\Bob Marley & The Wailers - One Love\(12) One Love _ People Get Ready.mp3",
                        AlbumId = 4
                    });
                }
                #endregion
            }
        }

        public object GetJson(GetCategories request) {
            using (var db = dbFactory.Open()) {
                var categories = db.Select<Category>();

                return new CategoriesResponse {
                    Status = "success",
                    Data = categories
                };
            }
        }

        public object GetJson(GetCategory request) {
            using (var db = dbFactory.Open()) {
                var category = db.SelectByIds<Category>(new[] { request.Id });
                if (category.Any()) {
                    return new CategoriesResponse {
                        Status = "success",
                        Data = category
                    };

                }
                else {
                    throw HttpError.NotFound("Category does not exist.");
                }
            }
        }

        public object GetJson(GetAlbums request) {
            using (var db = dbFactory.Open()) {
                var albums = db.Select<Album>().Where(a => a.CategoryId == request.CategoryId);
                if (albums.Any()) {
                    return new AlbumsResponse {
                        Status = "success",
                        Data = albums
                    };
                }
                else {
                    throw HttpError.NotFound("Albums not found.");
                }
            }
        }

        public object GetJson(GetAlbum request) {
            using (var db = dbFactory.Open()) {
                var albums = db.SelectByIds<Album>(new[] { request.Id }).Where(a => a.CategoryId == request.CategoryId);
                if (albums.Any()) {
                    return new AlbumsResponse {
                        Status = "success",
                        Data = albums
                    };
                }
                else {
                    throw HttpError.NotFound("Album not found.");
                }
            }
        }

        public object GetJson(GetTracks request) {
            using (var db = dbFactory.Open()) {
                // TODO: Also do a join on the album's categoryId to make sure all Ids match.
                var tracks = db.Select<Track>().Where(t => t.AlbumId == request.AlbumId);
                if (tracks.Any()) {
                    return new TracksResponse {
                        Status = "success",
                        Data = tracks
                    };
                }
                else {
                    throw HttpError.NotFound("Tracks not found.");
                }
            }
        }

        public object GetJson(GetTrack request) {
            using (var db = dbFactory.Open()) {
                // TODO: Also do a join on the album's categoryId to make sure all Ids match.
                var tracks = db.SelectByIds<Track>(new[] { request.Id }).Where(t => t.AlbumId == request.AlbumId);
                if (tracks.Any()) {
                    return new TracksResponse {
                        Status = "success",
                        Data = tracks
                    };
                }
                else {
                    throw HttpError.NotFound("Track not found.");
                }
            }
        }
    }
}
