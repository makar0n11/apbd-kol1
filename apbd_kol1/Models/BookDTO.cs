namespace apbd_kol1.Models;

public class BookDTO
{
    public AuthorDTO authorDto;
    public int Id { get; set; }
    public string Title { get; set; }
   // List<AuthorDTO> Authors {get; set;}
    
}
public class AuthorDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
}