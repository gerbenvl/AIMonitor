using Newtonsoft.Json;
using System;

namespace AIMonitor
{
    public class AIInstanceSetting
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("applicationId")]
        public string ApplicationId { get; set; }

        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }
    }
}
