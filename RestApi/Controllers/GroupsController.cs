using System.Net;
using Microsoft.AspNetCore.Mvc;
using RestApi.Dtos;
using RestApi.Exceptions;
using RestApi.Mappers;
using RestApi.Services;

namespace RestApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupsController : ControllerBase {
    private readonly IGroupService _groupService;

    public GroupsController (IGroupService groupService) {
        _groupService = groupService;
    }

    // localhost:port/groups/28728723
    [HttpGet("{id}")]
    public async Task<ActionResult<GroupResponse>> GetGroupById(string id, CancellationToken cancellationToken) {
        var group = await _groupService.GetGroupByIdAsync(id, cancellationToken);

        if (group is null) {
            return NotFound();
        }

        return Ok(group.ToDto());
    }
    
    /*[HttpGet]
    public async Task<ActionResult<IList<GroupResponse>>> GetGroupByName([FromQuery] string name, 
    [FromQuery] int page, [FromQuery] int pageS, [FromQuery] string orderBy, CancellationToken cancellationToken){
        
        var groups = await _groupService.GetGroupByNameAsync(name, page, pageS, orderBy, cancellationToken);

        return Ok(groups.Select(group => group.ToDto()).ToList());
    }*/

    //Nuevo método de GetGroupByName2, este funciona para hacer una busqueda exacta
    /*[HttpGet]
    public async Task<ActionResult<IList<GroupResponse>>> GetGroupByName2([FromQuery] string name, 
    [FromQuery] int page, [FromQuery] int pageS, [FromQuery] string orderBy, CancellationToken cancellationToken){
        
        var groups = await _groupService.GetGroupByNameAsync2(name, page, pageS, orderBy, cancellationToken);

        return Ok(groups.Select(group => group.ToDto()).ToList());
    }*/

    //Nuevo método de GetGroupByName2, este funciona para hacer una busqueda exacta sin paginación
    [HttpGet]
    public async Task<ActionResult<IList<GroupResponse>>> GetGroupByName2([FromQuery] string name, CancellationToken cancellationToken){
        
        var groups = await _groupService.GetGroupByNameAsync2(name, cancellationToken);

        return Ok(groups.Select(group => group.ToDto()).ToList());
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroupById(string id, CancellationToken cancellationToken){
      try{
        await _groupService.DeleteGroupByIdAsync(id, cancellationToken);
        return NoContent();
      }catch(GroupNotFoundException){
        return NotFound();
      }
        
    }

    [HttpPost]
    public async Task<ActionResult<GroupResponse>> CreatedGroup([FromBody] CreatedGroupRequest groupRequest, CancellationToken cancellationToken){
        try{

            var group = await _groupService.CreateGroupAsync(groupRequest.Name, groupRequest.Users, cancellationToken);
            return CreatedAtAction(nameof(GetGroupById), new {id = group.Id}, group.ToDto());

        }catch(InvalidGroupRequestFormatException){
            
            return BadRequest(NewValidationProblemDetails("One or more validation errors occurred", HttpStatusCode.BadRequest, new Dictionary<string, string[]>{
                {"Groups",["Users array is empty"]}
            }));

        }catch(GroupAlreadyExistsException){
            return Conflict(NewValidationProblemDetails("One or more validation errors occurred", HttpStatusCode.Conflict, new Dictionary<string, string[]>{
                {"Groups",["Group with same name already exists"]}
            }));
        }
    }

    private static ValidationProblemDetails NewValidationProblemDetails(string title, HttpStatusCode statusCode, Dictionary<string, string[]>errors){
        return new ValidationProblemDetails{
            Title = title,
            Status = (int)statusCode,
            Errors = errors
        };
    }
}