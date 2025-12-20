using System;
using System.Collections.Generic;
using System.Text;
using Tesseract;

namespace O24OpenAPI.Kit.OCR.Options
{
    public sealed class TesseractOcrOptions
    {
        /// <summary>
        /// Trỏ tới folder chứa *.traineddata (ví dụ: ...\tessdata)
        /// </summary>
        public string TessdataPath { get; set; } =
            Path.Combine(AppContext.BaseDirectory, "tessdata");

        /// <summary>Ví dụ: "vie+eng"</summary>
        //public string DefaultLanguage { get; set; } = "vie+eng";

        //public EngineMode EngineMode { get; set; } = EngineMode.Default;

        //public PageSegMode PageSegMode { get; set; } = PageSegMode.Auto;

        public string DefaultLanguage { get; set; } = "lao"; // test vie trước
        public EngineMode EngineMode { get; set; } = EngineMode.LstmOnly;
        public PageSegMode PageSegMode { get; set; } = PageSegMode.SparseText;


        /// <summary>
        /// Engine variables dạng key/value (tuỳ nhu cầu tối ưu OCR sau này)
        /// </summary>
        public Dictionary<string, string> EngineVariables { get; set; } = new();
    }
}
