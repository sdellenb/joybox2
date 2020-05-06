using ServiceStack;
using joybox2.ServiceModel;

namespace joybox2.ServiceInterface
{
    public class AdminServices : Service
    {
        public object Any(GetHealth request) => new GetHealthResponse
        {
            Status = "healthy",
            Version = "1.0.0"
        };
    }
}
