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
                    db.Insert(new Album {
                        Name = "Schwiizergoofe - 1",
                        Thumbnail = Guid.Parse("1d286810-e80d-46ce-b7c6-9cffe1223f82"),
                        CategoryId = musicCategoryId
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
    }
}
