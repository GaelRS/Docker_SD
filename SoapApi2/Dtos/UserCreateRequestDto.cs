using System.Runtime.Serialization;

namespace SoapApi2.Dtos;

[DataContract]
public class UserCreateRequestDto {
    [DataMember(Order = 1)]
    public string Email {get; set;} = null;
    [DataMember (Order =2)]
    public string FirstName {get; set;} = null;
    [DataMember (Order = 3)]
    public string LastName {get; set;} = null;

}