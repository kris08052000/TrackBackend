using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TRACK_BACKEND.Data;
using TRACK_BACKEND.Models;
using TRACK_BACKEND.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly PasswordHasher _hasher;
    private readonly JwtService _jwt;

    public AuthController(AppDbContext db, PasswordHasher hasher, JwtService jwt)
    {
        _db = db;
        _hasher = hasher;
        _jwt = jwt;
    }

    // Signup
    [HttpPost("signup")]
    public IActionResult Signup([FromBody] UserDto dto)
    {
        if (_db.Users.Any(x => x.Email == dto.Email))
            return BadRequest("Email exists");

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = _hasher.Hash(dto.Password)
        };

        _db.Users.Add(user);
        _db.SaveChanges();

        return Ok("Signup success");
    }

    // Login
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserDto dto)
    {
        var user = _db.Users.FirstOrDefault(x => x.Email == dto.Email);
        if (user == null) return Unauthorized();

        if (!_hasher.Verify(dto.Password, user.PasswordHash))
            return Unauthorized();

        var token = _jwt.Generate(user.Id, user.Email);

        return Ok(new { token });
    }

    // Me
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        // Match the claim types you used in JWT
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        return Ok(new { id, email });
    }

    public class UserDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}