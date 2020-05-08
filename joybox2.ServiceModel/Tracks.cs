using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace joybox2.ServiceModel {

    [Route("/categories/{categoryId}/albums/{albumId}/tracks")]
    public class GetTracks : IReturn<TracksResponse> {
        public int CategoryId { get; set; }
        public int AlbumId { get; set; }
    }

    [Route("/categories/{categoryId}/albums/{albumId}/tracks/{id}")]
    public class GetTrack : IReturn<TracksResponse> {
        public int CategoryId { get; set; }
        public int AlbumId { get; set; }
        public int Id { get; set; }
    }

    public class TracksResponse {
        public string Status { get; set; }
        public IEnumerable<Track> Data { get; set; }

    }

    public class Track {
        [AutoIncrement]
        public int Id { get; set; } // 'Id' is PrimaryKey by convention

        [Required]
        public int TrackIndex { get; set; }

        [Required, Unique, StringLength(1024)]
        public string Path { get; set; }

        [Reference]
        public int AlbumId { get; set; }
    }

}
