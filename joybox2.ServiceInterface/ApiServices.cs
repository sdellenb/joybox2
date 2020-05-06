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
                // Initialize with some hardcoded data.
                if (db.CreateTableIfNotExists<Category>()) {
                    db.Insert(new Category { Id = 1, Name = "audiobooks", Thumbnail = "/svg/audiobooks.svg" });
                    db.Insert(new Category { Id = 2, Name = "music", Thumbnail = "/svg/music.svg" });
                }
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
    }
}
