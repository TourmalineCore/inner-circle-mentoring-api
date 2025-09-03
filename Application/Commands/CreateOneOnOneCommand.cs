using Core.Entities;

namespace Application.Commands;

public class CreateOneOnOneCommandParams
{
    public long MentorEmployeeId { get; set; }

    public long MenteeEmployeeId { get; set; }

    public DateOnly Date { get; set; }

    public string Note { get; set; }
}

public class CreateOneOnOneCommand
{
    private readonly AppDbContext _context;

    public CreateOneOnOneCommand(
        AppDbContext context
    )
    {
        _context = context;
    }

    public async Task<long> ExecuteAsync(CreateOneOnOneCommandParams commandParams, long tenantId)
    {
        var oneOnOne = new OneOnOne
        {
            TenantId = tenantId,
            MentorEmployeeId = commandParams.MentorEmployeeId,
            MenteeEmployeeId = commandParams.MenteeEmployeeId,
            Date = commandParams.Date,
            Note = commandParams.Note,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _context
            .OneOnOnes
            .AddAsync(oneOnOne);

        await _context.SaveChangesAsync();
        
        return oneOnOne.Id;
    }
}
