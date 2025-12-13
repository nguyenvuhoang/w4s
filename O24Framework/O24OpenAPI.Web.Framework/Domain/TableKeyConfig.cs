using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Web.Framework.Domain
{
    public class TableKeyConfig : BaseEntity
    {
        public string SchemaName { get; set; } = default!;
        public string TableName { get; set; } = default!;
        public string KeyColumn { get; set; } = default!;
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
