using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.AI.Infrastructure.Configurations
{
    public class LLMProviderConfig : IConfig
    {
        public string ProviderName { get; set; } = "OpenAI";
        public OpenAIConfig OpenAI { get; set; } = new();
        public OllamaConfig Ollama { get; set; } = new();
    }
    public class OpenAIConfig
    {
        public string ApiKey { get; set; } = "";
        public string BaseUrl { get; set; } = "https://api.openai.com/v1/";
        public string ChatModel { get; set; } = "gpt-3.5-turbo";
        public string EmbedModel { get; set; } = "text-embedding-3-small";
    }
    public class OllamaConfig
    {
        public string BaseUrl { get; set; } = "http://localhost:11434/";
        public string ChatModel { get; set; } = "llama2-7b-chat";
        public string EmbedModel { get; set; } = "llama2-7b-embed";
    }
}
