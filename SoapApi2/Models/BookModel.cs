using SoapApi2.Dtos;

namespace SoapApi2.Models;

public class BookModel {
    internal Guid id;
    public Guid Id {get; set;}
    public string Title {get; set;} = null!;
    public string Author {get; set;} = null!;
    public string Publisher {get; set;} = null!;
    public DateTime PublishedDate {get; set;}
   
}