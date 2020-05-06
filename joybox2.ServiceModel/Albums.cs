using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace joybox2.ServiceModel {

    [Route("/categories/{categoryId}/albums")]
    public class GetAlbums : IReturn<AlbumsResponse> {
        public int CategoryId { get; set; }
    }

    [Route("/categories/{categoryId}/albums/{Id}")]
    public class GetAlbum : IReturn<AlbumsResponse> {
        public int CategoryId { get; set; }
        public int Id { get; set; }
    }

    public class AlbumsResponse {
        public string Status { get; set; }
        public IEnumerable<Album> Data { get; set; }

    }

    public class Album {
        [AutoIncrement]
        public int Id { get; set; } // 'Id' is PrimaryKey by convention

        [Required, Unique]
        public string Name { get; set; }

        [Unique]
        public Guid Thumbnail { get; set; }

        [Reference]
        public int CategoryId { get; set; }
    }

}