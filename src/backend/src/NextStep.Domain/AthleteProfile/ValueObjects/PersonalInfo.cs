using NextStep.Domain.Common;

namespace NextStep.Domain.AthleteProfile.ValueObjects;

public class PersonalInfo : ValueObject
{
    public string Name { get; private set; }
    public DateOnly BirthDate { get; private set; }

    private PersonalInfo()
    {
        Name = string.Empty;
    }

    private PersonalInfo(string name, DateOnly birthDate)
    {
        Name = name;
        BirthDate = birthDate;
    }

    public static PersonalInfo Create(string name, DateOnly birthDate)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        if (birthDate > DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("Birth date cannot be in the future", nameof(birthDate));

        return new PersonalInfo(name.Trim(), birthDate);
    }

    public int CalculateAge()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - BirthDate.Year;

        if (BirthDate > today.AddYears(-age))
            age--;

        return age;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
        yield return BirthDate;
    }
}
