using System.Collections.Generic;

namespace Test20211227.Models
{
    public class apiResponse
    {
        public string error { get; set; }
        public string msg { get; set; }
        public List<Country> data { get; set; }
    }
}
