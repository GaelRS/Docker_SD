using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApi.Dtos;
using RestApi.Exceptions;
using RestApi.Mappers;
using RestApi.Services;

namespace RestApi.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class GroupsController : ControllerBase {
    private readonly IGroupService _groupService;

    public GroupsController(IGroupService groupService) {
        _groupService = groupService;
    }

    // localhost:port/groups/28728723
    [HttpGet("{id}")]
    [Authorize(Policy = "Read")]
    public async Task<ActionResult<GroupResponse>> GetGroupById(string id, CancellationToken cancellationToken) {
        var group = await _groupService.GetGroupByIdAsync(id, cancellationToken);

        if (group is null) {
            return NotFound();
        }

        return Ok(group.ToDto());
    }

    [HttpGet]
    [Authorize(Policy = "Read")]

    public async Task<ActionResult<IList<GroupResponse>>> GetGroupByName(
        [FromQuery] string name, 
        [FromQuery] int page, 
        [FromQuery] int pageS, 
        [FromQuery] string orderBy, 
        CancellationToken cancellationToken) {
        
        var groups = await _groupService.GetGroupByNameAsync(name, page, pageS, orderBy, cancellationToken);
        return Ok(groups.Select(group => group.ToDto()).ToList());
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Write")]
    public async Task<IActionResult> DeleteGroupById(string id, CancellationToken cancellationToken) {
        try {
            await _groupService.DeleteGroupByIdAsync(id, cancellationToken);
            return NoContent();
        } catch(GroupNotFoundException) {
            return NotFound();
        }
    }

    [HttpPost]
    [Authorize(Policy = "Write")]
    public async Task<ActionResult<GroupResponse>> CreatedGroup(
        [FromBody] CreatedGroupRequest groupRequest, 
        CancellationToken cancellationToken) {
        
        try {
            var group = await _groupService.CreateGroupAsync(groupRequest.Name, groupRequest.Users, cancellationToken);
            return CreatedAtAction(nameof(GetGroupById), new {id = group.Id}, group.ToDto());
        } catch(UserNotFoundException) {
            return NotFound(NewValidationProblemDetails("One or more validation errors occurred", HttpStatusCode.NotFound, new Dictionary<string, string[]> {
                {"Users",["User not found"]}
            }));
        } catch(InvalidGroupRequestFormatException) {
            return BadRequest(NewValidationProblemDetails("One or more validation errors occurred", HttpStatusCode.BadRequest, new Dictionary<string, string[]> {
                {"Groups",["Users array is empty"]}
            }));
        } catch(GroupAlreadyExistsException) {
            return Conflict(NewValidationProblemDetails("One or more validation errors occurred", HttpStatusCode.Conflict, new Dictionary<string, string[]> {
                {"Groups",["Group with same name already exists"]}
            }));
        }
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Write")]
    public async Task<IActionResult> UpdateGroup(
        string id, 
        [FromBody] UpdateGroupRequest groupRequest, 
        CancellationToken cancellationToken) {
        
        try {
            await _groupService.UpdateGroupAsync(id, groupRequest.Name, groupRequest.Users, cancellationToken);
            return NoContent();
        } catch(UserNotFoundException) {
            return NotFound(NewValidationProblemDetails("One or more validation errors occurred", HttpStatusCode.NotFound, new Dictionary<string, string[]> {
                {"Users",["User not found"]}
            }));
        } catch(GroupNotFoundException) {
            return NotFound();
        } catch(InvalidGroupRequestFormatException) {
            return BadRequest(NewValidationProblemDetails("One or more validation errors occurred", HttpStatusCode.BadRequest, new Dictionary<string, string[]> {
                {"Groups",["Users array is empty"]}
            }));
        } catch(GroupAlreadyExistsException) {
            return Conflict(NewValidationProblemDetails("One or more validation errors occurred", HttpStatusCode.Conflict, new Dictionary<string, string[]> {
                {"Groups",["Group with same name already exists"]}
            }));
        }
    }

    private static ValidationProblemDetails NewValidationProblemDetails(
        string title, 
        HttpStatusCode statusCode, 
        Dictionary<string, string[]> errors) {
        
        return new ValidationProblemDetails {
            Title = title,
            Status = (int)statusCode,
            Errors = errors
        };
    }
}
