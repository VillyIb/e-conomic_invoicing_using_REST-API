namespace Eu.Iamia.Utils.UnitTests;

public class TestSubjectDto
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
}