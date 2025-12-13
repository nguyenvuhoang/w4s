namespace O24OpenAPI.ControlHub.Utils;

public static class ParamAdmin
{
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static DateTime GetBusDate()
    {
        return DateTime.UtcNow;
    }
}
/// <summary>
/// CreditTenorType
/// </summary>
public static class CreditTenorType
{
    /// <summary>
    /// The Advance
    /// </summary>
    public const string Advance = "A";
    /// <summary>
    /// The Bullet
    /// </summary>
    public const string Bullet = "B";
    /// <summary>
    /// The Any
    /// </summary>
    public const string Any = "E";

    /// <summary>
    /// The Day
    /// </summary>
    public const string Day = "D";
    /// <summary>
    /// The Quarter
    /// </summary>
    public const string Quarter = "Q";
    /// <summary>
    ///  The week
    /// </summary>
    public const string Week = "W";
    /// <summary>
    /// The Month
    /// </summary>
    public const string Month = "M";
    /// <summary>
    /// The Year
    /// </summary>
    public const string Year = "Y";
    /// <summary>
    /// HalfYear = "H"
    /// </summary>
    public const string HalfYear = "H";

}
