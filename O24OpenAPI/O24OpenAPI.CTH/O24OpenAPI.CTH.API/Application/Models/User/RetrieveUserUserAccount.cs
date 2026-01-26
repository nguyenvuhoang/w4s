using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.CTH.API.Application.Models.User
{
    public class RetrieveUserUserAccount : BaseO24OpenAPIModel
    {
        public string ContractNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrencyCode { get; set; }
    }
}
