namespace PageObjects.Models;

public class CreateUserRequest
{
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;

    public CreateUserRequest(string name, string username)
    {
        Name = name;
        Username = username;
    }
}
