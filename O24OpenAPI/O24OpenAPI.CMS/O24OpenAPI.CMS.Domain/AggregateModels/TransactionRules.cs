namespace O24OpenAPI.CMS.Domain;

public class TransactionRules : BaseEntity
{
    public string WorkflowId { get; set; } // ID của workflow
    public string RuleName { get; set; } // Tên quy tắc
    public string Parameter { get; set; } // Tham số cho quy tắc
    public int RuleOrder { get; set; } // Thứ tự quy tắc
    public bool IsEnable { get; set; } // Trạng thái kích hoạt
    public string Spec { get; set; } // Đặc tả quy tắc
    public string Example { get; set; } // Ví dụ về quy tắc
    public string Caption { get; set; } // Tiêu đề quy tắc
    public string Condition { get; set; } // Điều kiện quy tắc
    public DateTime? UpdatedOnUtc { get; set; } // Thời gian cập nhật (UTC)
}
