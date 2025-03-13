using System.Collections.Generic;

namespace BankingProductsData
{

    public class GeminiRequest
    {
        public List<RequestContent> contents { get; set; }
        public List<RequestSafetySettings> safety_settings { get; set; }
    }

    public class RequestContent
    {
        public List<RequestPart> parts { get; set; }
        public string role { get; set; }
    }

    public class RequestPart
    {
        public string text { get; set; }
    }

    public class RequestSafetySettings
    {
        public string category { get; set; }
        public string threshold { get; set; }
    }
}
