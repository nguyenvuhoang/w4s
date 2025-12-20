using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace O24OpenAPI.Kit.OCR.Models
{
    public class OcrBase64Request
    {
        [JsonPropertyName("base64")]
        public string Base64 { get; set; } = default!;
        public string? Language { get; set; }
        public bool CollectWords { get; set; } = false;
        public string? DocumentType { get; set; } = null;
        public bool CleanText { get; set; } = true;
    }

}
