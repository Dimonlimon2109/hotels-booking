using HotelsBooking.BLL.Interfaces;

namespace HotelsBooking.API.Adapters
{
    public class WebHostAdapter : IRootPath
    {
        private readonly IWebHostEnvironment _environment;
        public WebHostAdapter(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public string RootPath => _environment.WebRootPath;
    }
}
