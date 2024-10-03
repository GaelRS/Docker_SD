namespace RestApi.Dtos;

public class CreatedGroupRequest {
    public string Name { get; set; }
    public Guid[] Users { get; set; }
}