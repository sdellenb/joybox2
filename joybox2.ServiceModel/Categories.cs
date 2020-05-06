using System.Collections.Generic;
using ServiceStack;

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
        public int Id { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
    }
}
