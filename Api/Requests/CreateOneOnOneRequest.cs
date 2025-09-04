using System.ComponentModel.DataAnnotations;

namespace Api.Requests;

public class CreateOneOnOneRequest
{
    [Required]
    public long MenteeEmployeeId { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    [Required]
    public string Note { get; set; }
}