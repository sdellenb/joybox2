using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace joybox2.ServiceModel {

    [Route("/categories")]
    public class GetCategories : IReturn<CategoriesResponse> { }

    [Route("/categories/{Id}")]
    public class GetCategory : IReturn<CategoriesResponse> {
        public int Id { get; set; }
    }

    public class CategoriesResponse {
        public string Status { get; set; }
        public IEnumerable<Category> Data { get; set; }
    }

    public class Category {
        [AutoIncrement]
        public int Id { get; set; } // 'Id' is PrimaryKey by convention

        [Required, Unique]
        public string Name { get; set; }

        public string Thumbnail { get; set; }
    }
}
