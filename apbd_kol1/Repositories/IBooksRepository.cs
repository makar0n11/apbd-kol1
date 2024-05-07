using apbd_kol1.Models;

namespace ExampleTest1.Repositories;

public interface IBooksRepository
{
    Task<bool>  DoesBookExist(int id);
    Task<bool> DoesAuthorExist(int id);
    Task<int> AddBook(BookADD bookDtoAdd);
   
    Task<BookDTO> GetBook(int id);
 
}