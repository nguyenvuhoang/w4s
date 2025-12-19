using Newtonsoft.Json;
using O24OpenAPI.CMS.API.Application.Features.Requests;

namespace O24OpenAPI.CMS.API.Application.Models
{
    public class FormModel : BaseO24OpenAPIModel
    {
        /// <summary>
        ///
        /// </summary>
        public FormModel() { }
        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [JsonProperty("info")]
        public InfoForm Info { get; set; } = new InfoForm();
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [JsonProperty("list_layout")]
        public List<Dictionary<string, object>> ListLayout { get; set; } = [];
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [JsonProperty("form_id")] public string FormId { get; set; }
        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        [JsonProperty("app")] public string App { get; set; } = string.Empty;
        /// <summary>
        ///  Master data
        /// </summary>
        [JsonProperty("master_data")]
        public BoRequestModel MasterData { get; set; } = new BoRequestModel();


    }

    public class InfoForm : BaseO24OpenAPIModel
    {
        /// <summary>
        ///
        /// </summary>
        public InfoForm() { }
        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        public InfoForm(string info)
        {

        }
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("title")] public string Title { get; set; } = string.Empty;
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("des")] public Dictionary<string, object> Des { get; set; } = [];
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("data")] public string Data { get; set; } = string.Empty;
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("learnapi")] public string Learnapi { get; set; } = string.Empty;
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("learnsql")] public string Learnsql { get; set; } = string.Empty;
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("last_update")] public string LastUpdate { get; set; } = string.Empty;
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("bodata")] public string Bodata { get; set; } = string.Empty;
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("openOne")] public string Openone { get; set; } = string.Empty;
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("url_input")] public string Url_Input { get; set; } = string.Empty;
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("lang_form")] public Dictionary<string, object> Lang_Form { get; set; } = [];
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("mode_form")] public ModeForm Mode_Form { get; set; } = new ModeForm();
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("form_code")] public string Form_Code { get; set; } = string.Empty;
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("ruleStrong")] public object Rulestrong { get; set; } = new object();
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("app")] public string App { get; set; } = string.Empty;


    }

    public class ModeForm
    {
        /// <summary>
        ///
        /// </summary>
        public ModeForm() { }
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("mode")] public string Mode { get; set; } = string.Empty;
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("col_text")] public string ColText { get; set; } = string.Empty;
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("col_input")] public string ColInput { get; set; } = string.Empty;
    }
}
