using System; 
using System.Collections.Generic; 
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class AuthService {
    private static List<User> users = new List<User>();
    private const string SecretKey = "supersecretkey123!";
    private readonly SymmetricSecurityKey _key;

    public AuthService() {
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
    }

    public string Register(string username, string password, string role) {
        var user = new User { Username = username, Password = password, Role = role};
        users.Add(user);
        return "User registered successfully";
    }

    public string Login(string username, string password) {
        var user = users.Find(u => u.Username == username && u.Password == password);
        if (user == null) return "Invalid credentials";

        var token = GenerateJwtToken(user);
        return token;
    }

    private string GenerateJwtToken(User user) {
        var claims = new[] {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}