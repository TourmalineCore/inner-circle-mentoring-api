namespace Api.Responses;

public class MenteeOneOnOnesResponse
{
    public List<OneOnOneDto> OneOnOnes { get; set; }

    public EmployeeDto Mentee { get; set; }
}

public class OneOnOneDto
{
    public long Id { get; set; }

    public DateOnly Date { get; set; }

    public string Note { get; set; }

    public EmployeeDto Mentor { get; set; }
}

public class EmployeeDto
{
    public long EmployeeId { get; set; }

    public string FullName { get; set; }
}
