namespace Test.Features
{
    public sealed class ZaloOptions
    {
        public string AppId { get; set; } = "";
        public string SecretKey { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public string TemplateId { get; set; } = "";
        public string Mode { get; set; } = "development";
    }
}
