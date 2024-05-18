namespace Domain.Entities.Responses;

public class GetAllDbUsersResponse
{
    public List<GetDbUserResponse> DbUsers { get; set; } = new();
}