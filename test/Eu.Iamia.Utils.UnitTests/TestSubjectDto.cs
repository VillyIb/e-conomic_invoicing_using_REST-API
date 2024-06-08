namespace Eu.Iamia.Utils.UnitTests;

public class TestSubjectDto : IEquatable<TestSubjectDto>
{
    private string? _subject;

    public string? Subject
    {
        get => _subject;
        set
        {
            if (String.IsNullOrWhiteSpace(value)) throw new ArgumentException("Please provide a value.", nameof(Subject));
            _subject = value;
        }
    }

    public bool Equals(TestSubjectDto? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _subject == other._subject;
    }


    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TestSubjectDto)obj);
    }

    public override int GetHashCode()
    {
        return (_subject != null ? _subject.GetHashCode() : 0);
    }
}

public class DeepTestSubjectL3Dto
{
    public string? L4 { get; set; }
}

public class DeepTestSubjectL2Dto
{
    public DeepTestSubjectL3Dto? L3 { get; set; }
}
public class DeepTestSubjectL1Dto
{
    public DeepTestSubjectL2Dto? L2 { get; set; }
}

public class DeepTestSubjectL0Dto
{
    public DeepTestSubjectL1Dto? L1 { get; set; }
}