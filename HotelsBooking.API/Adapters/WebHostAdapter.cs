using HotelsBooking.BLL.Interfaces;

namespace HotelsBooking.API.Adapters
{
    public class WebHostAdapter : IImagePath
    {
        private readonly IWebHostEnvironment _environment;
        public WebHostAdapter(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public string RootPath => _environment.WebRootPath;
    }
}
