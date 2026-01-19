namespace O24OpenAPI.CMS.Domain.AggregateModels;

public partial class TransactionExecutions : BaseEntity
{
    public string? WorkflowId { get; set; } // ID của workflow
    public string? StoreProcedure { get; set; } // Tên thủ tục lưu trữ
    public string? Parameter { get; set; } // Tham số cho thủ tục
    public int ExecOrder { get; set; } // Thứ tự thực thi
    public bool IsEnable { get; set; } // Trạng thái kích hoạt
    public string? Condition { get; set; } // Điều kiện thực thi
}
