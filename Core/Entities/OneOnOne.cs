namespace Core.Entities;

public class OneOnOne
{
    public OneOnOne()
    {
    }

    public long Id { get; set; }

    public long TenantId { get; set; }

    public long MentorEmployeeId { get; set; }

    public long MenteeEmployeeId { get; set; }

    public DateOnly Date { get; set; }

    public string Note { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}
