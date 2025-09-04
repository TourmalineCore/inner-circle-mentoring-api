using Microsoft.EntityFrameworkCore;

namespace Application.Commands;

public class HardDeleteOneOnOneCommandParams
{
    public long OneOnOneId { get; set; }
}

public class HardDeleteOneOnOneCommand
{
    private readonly AppDbContext _context;

    public HardDeleteOneOnOneCommand(
        AppDbContext context
    )
    {
        _context = context;
    }

    public async Task<bool> ExecuteAsync(HardDeleteOneOnOneCommandParams commandParams, long tenantId)
    {
        var oneOnOne = await _context
            .OneOnOnes
            .Where(x => x.TenantId == tenantId)
            .SingleAsync(x => x.Id == commandParams.OneOnOneId);

        _context
            .OneOnOnes
            .Remove(oneOnOne);

        await _context.SaveChangesAsync();

        return true;
    }
}
