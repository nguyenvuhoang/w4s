namespace Test.Models;

public readonly record struct UserId(Guid Value)
{
    public static UserId New() => new(Guid.NewGuid());
}

public readonly record struct Money(decimal Value, string Currency = "VND")
{
    public static Money Zero => new(0);

    public static Money operator +(Money a, Money b)
        => new(a.Value + b.Value, a.Currency);

    public static Money operator -(Money a, Money b)
        => new(a.Value - b.Value, a.Currency);
}

public enum SpendingCategory
{
    Housing,
    Food,
    Transportation,
    Utilities,
    Entertainment,
    Education,
    Healthcare,
    Debt,
    Other
}

public enum SpendingFrequency
{
    Daily,
    Weekly,
    Monthly,
    Yearly
}

public sealed record SpendingItem
{
    public SpendingCategory Category { get; }
    public Money Amount { get; }
    public SpendingFrequency Frequency { get; }

    public SpendingItem(
        SpendingCategory category,
        Money amount,
        SpendingFrequency frequency)
    {
        if (amount.Value <= 0)
            throw new Exception("Spending amount must be greater than zero.");

        Category = category;
        Amount = amount;
        Frequency = frequency;
    }

    public decimal MonthlyEquivalent()
        => Frequency switch
        {
            SpendingFrequency.Daily => Amount.Value * 30,
            SpendingFrequency.Weekly => Amount.Value * 4,
            SpendingFrequency.Monthly => Amount.Value,
            SpendingFrequency.Yearly => Amount.Value / 12,
            _ => Amount.Value
        };
}

public sealed class Loan
{
    public string Name { get; }
    public Money RemainingAmount { get; }
    public decimal InterestRate { get; } // 0.15 = 15%

    public Loan(
        string name,
        Money remainingAmount,
        decimal interestRate)
    {
        if (remainingAmount.Value < 0)
            throw new Exception("Loan amount must be >= 0");

        if (interestRate < 0)
            throw new Exception("Interest rate must be >= 0");

        Name = name;
        RemainingAmount = remainingAmount;
        InterestRate = interestRate;
    }
}

public sealed class FinancialProfile
{
    public UserId UserId { get; }
    public Money MonthlyIncome { get; }

    private readonly List<SpendingItem> _spendings = new();
    public IReadOnlyCollection<SpendingItem> Spendings => _spendings.AsReadOnly();

    private readonly List<Loan> _loans = new();
    public IReadOnlyCollection<Loan> Loans => _loans.AsReadOnly();

    // =========================
    // Derived values (Domain logic)
    // =========================

    public decimal TotalMonthlyExpense =>
        _spendings.Sum(x => x.MonthlyEquivalent());

    public decimal SavingCapacity =>
        MonthlyIncome.Value - TotalMonthlyExpense;

    public bool IsDeficit =>
        SavingCapacity < 0;

    // =========================
    // Constructor
    // =========================

    public FinancialProfile(
        UserId userId,
        Money monthlyIncome,
        IEnumerable<SpendingItem>? spendings = null,
        IEnumerable<Loan>? loans = null)
    {
        UserId = userId;
        MonthlyIncome = monthlyIncome;

        if (spendings != null)
            _spendings.AddRange(spendings);

        if (loans != null)
            _loans.AddRange(loans);
    }

    // =========================
    // Behavior (rich domain)
    // =========================

    public decimal CalculateSavingRate()
        => MonthlyIncome.Value == 0
            ? 0
            : SavingCapacity / MonthlyIncome.Value;

    public SpendingItem? HighestSpending()
        => _spendings
            .OrderByDescending(x => x.MonthlyEquivalent())
            .FirstOrDefault();
}
