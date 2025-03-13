namespace BankingProductsData
{
    public class GemimiResponse
    {
        public Candidate[] candidates { get; set; }
        public Promptfeedback promptFeedback { get; set; }
    }

    public class Promptfeedback
    {
        public Safetyrating[] safetyRatings { get; set; }
    }

    public class Safetyrating
    {
        public string category { get; set; }
        public string probability { get; set; }
    }

    public class Candidate
    {
        public ResponseContent content { get; set; }
        public string finishReason { get; set; }
        public int index { get; set; }
        public Safetyrating1[] safetyRatings { get; set; }
    }

    public class ResponseContent
    {
        public ResponsePart[] parts { get; set; }
        public string role { get; set; }
    }

    public class ResponsePart
    {
        public string text { get; set; }
    }

    public class Safetyrating1
    {
        public string category { get; set; }
        public string probability { get; set; }
    }

}
