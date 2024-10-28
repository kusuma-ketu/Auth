using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] User user)
    {
        var result = _authService.Register(user.Username, user.Password, user.Role);
        return Ok(result);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User user)
    {
        var token = _authService.Login(user.Username, user.Password);
        return Ok(new { Token = token });
    }
}
