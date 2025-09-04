using Api.Requests;
using Api.Responses;
using Application.Commands;
using Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TourmalineCore.AspNetCore.JwtAuthentication.Core.Filters;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("one-on-ones")]
public class OneOnOnesController : ControllerBase
{
    /// <summary>
    ///     Get mentee's one on ones
    /// </summary>
    [RequiresPermission(UserClaimsProvider.CanViewOneOnOnes)]
    [HttpGet]
    public async Task<MenteeOneOnOnesResponse> GetMenteeOneOnOnesAsync(
        [FromServices] MenteeOneOnOnesQuery menteeOneOnOnesQuery,
        [FromQuery] long menteeEmployeeId
    )
    {
        var queryParams = new MenteeOneOnOnesQueryParams
        {
            MenteeEmployeeId = menteeEmployeeId
        };

        var menteeOneOnOnes = await menteeOneOnOnesQuery.GetAsync(queryParams, User.GetTenantId());

        return new MenteeOneOnOnesResponse
        {
            OneOnOnes = menteeOneOnOnes
                .Select(x => new OneOnOneDto
                {
                    Id = x.Id,
                    Date = x.Date,
                    Note = x.Note,
                    Mentor = new EmployeeDto
                    {
                        EmployeeId = x.MentorEmployeeId,
                        // ToDo load from employees-api
                        FullName = "Mega Mentor"
                    }
                })
                .ToList(),
            Mentee = new EmployeeDto
            {
                EmployeeId = menteeEmployeeId,
                // ToDo load from employees-api
                FullName = "N/A"
            }
        };
    }

    /// <summary>
    ///     Create one on one
    /// </summary>
    /// <param name="createOneOnOneRequest"></param>
    [RequiresPermission(UserClaimsProvider.CanManageOneOnOnes)]
    [HttpPost]
    public async Task<CreateOneOnOneResponse> CreateOneOnOneAsync(
        [FromServices] CreateOneOnOneCommand createOneOnOneCommand,
        [Required][FromBody] CreateOneOnOneRequest createOneOnOneRequest
    )
    {
        var commandParams = new CreateOneOnOneCommandParams
        {
            MentorEmployeeId = User.GetEmployeeId(),
            MenteeEmployeeId = createOneOnOneRequest.MenteeEmployeeId,
            Date = createOneOnOneRequest.Date,
            Note = createOneOnOneRequest.Note
        };

        var newOneOnOneId = await createOneOnOneCommand.ExecuteAsync(commandParams, User.GetTenantId());

        return new CreateOneOnOneResponse()
        {
            NewOneOnOneId = newOneOnOneId
        };
    }

    /// <summary>
    ///     Deletes specific one on one
    /// </summary>
    /// <param name="oneOnOneId"></param>
    [RequiresPermission(UserClaimsProvider.AUTO_TESTS_ONLY_IsOneOnOnesHardDeleteAllowed)]
    [HttpDelete("{oneOnOneId}/hard-delete")]
    public async Task<object> HardDeleteOneOnOneAsync(
        [FromServices] HardDeleteOneOnOneCommand hardDeleteOneOnOneCommand,
        [Required][FromRoute] long oneOnOneId
    )
    {
        var commandParams = new HardDeleteOneOnOneCommandParams
        {
            OneOnOneId = oneOnOneId
        };

        return new
        {
            isDeleted = await hardDeleteOneOnOneCommand.ExecuteAsync(commandParams, User.GetTenantId())
        };
    }
}
