using ServiceStack;

namespace joybox2.ServiceModel
{
    [Route("/health")]
    public class GetHealth : IReturn<GetHealthResponse> {}

    public class GetHealthResponse
    {
        public string Status { get; set; }
        public string Version { get; set; }
    }
}
