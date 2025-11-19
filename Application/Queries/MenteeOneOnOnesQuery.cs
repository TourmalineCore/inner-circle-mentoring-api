using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries;

public class MenteeOneOnOnesQueryParams
{
    public long MenteeEmployeeId { get; set; }
}

public class MenteeOneOnOnesQuery
{
    private readonly AppDbContext _context;

    public MenteeOneOnOnesQuery(
        AppDbContext context
    )
    {
        _context = context;
    }
    
    public Task<List<OneOnOne>> GetAsync(MenteeOneOnOnesQueryParams queryParams, long tenantId)
    {
        return _context
            .OneOnOnes
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .Where(x => x.MenteeEmployeeId == queryParams.MenteeEmployeeId)
            .ToListAsync();
    }
}
