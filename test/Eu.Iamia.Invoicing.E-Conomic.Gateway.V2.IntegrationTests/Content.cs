using System.Text.Json.Serialization;
using System.Text.Json;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.IntegrationTests;

    public class Content
    {
        public string message { get; set; }
        public string errorCode { get; set; }
        public string developerHint { get; set; }
        public string logId { get; set; }
        public int httpStatusCode { get; set; }
        public string[] errors { get; set; }
        public DateTime logTime { get; set; }
        public string schemaPath { get; set; }

    public static Content? FromJson(string json)
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Converters = { new JsonStringEnumConverter() },
        };

        var content = JsonSerializer.Deserialize<Content>(json, options);
        return content;
    }

    public override string ToString()
        => JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            WriteIndented = true
        });
}


