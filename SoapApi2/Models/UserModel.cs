namespace SoapApi2.Models;

public class UserModel {
    internal Guid id;

    public Guid Id {get; set;}
    public string Email {get; set;} = null;
    public string FirstName {get; set;} = null;
    public string LastName {get; set;} = null;
    public DateTime BirthDate {get; set;}
}