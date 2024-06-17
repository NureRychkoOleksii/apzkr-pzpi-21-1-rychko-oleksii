using Backend.Abstraction.Services;
using Backend.Core.Attributes;
using Backend.Core.DTOs.User;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;

    public UserController(IUserService userService, IJwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _userService.GetUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _userService.GetUserAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    [AdminRoleInterceptor]
    public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserDTO user)
    {
        try
        {
            await _userService.CreateUserAsync(user);
            return Ok();
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPut("{id}")]
    [AdminRoleInterceptor]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDTO user)
    {
        try
        {
            await _userService.UpdateUserAsync(id, user);
            return NoContent();
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("{id}")]
    [AdminRoleInterceptor]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginDTO loginDTO)
    {
        var user = await _userService.AuthenticateUserAsync(loginDTO.Username, loginDTO.Password);

        if (user == null)
        {
            return Unauthorized("Invalid username or password");
        }
        
        var token = _jwtService.GenerateToken(user.Id, user.Role);
        return Ok(token);
    }
    
    [HttpPost("signup")]
    public async Task<ActionResult> SignUp([FromBody] SignUpDTO signUpDto)
    {
        try
        {
            var result = await _userService.SignUpAsync(signUpDto);

            if (result)
            {
                return Ok("User registration successful");
            }

            return BadRequest("User with the same email already exists");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}