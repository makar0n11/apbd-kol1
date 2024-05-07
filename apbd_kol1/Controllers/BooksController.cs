using apbd_kol1.Models;
using ExampleTest1.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace apbd_kol1.Controllers;

[Route("api/books/")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBooksRepository _booksRepository;
    public BooksController(IBooksRepository booksRepository)
    {
        _booksRepository = booksRepository;
    }
    [HttpGet("{int:id}/authors")]
    public async Task<IActionResult> GetAuthor(int id)
    {
        if (!await _booksRepository.DoesBookExist(id))
            return NotFound($"Book with given ID - {id} doesn't exist");
        
        var book = await _booksRepository.GetBook(id);
            
        return Ok(book);
    }
    [HttpPost]
    public async Task<IActionResult> AddAnimal(BookADD bookDtAdd)
    {
        
        await _booksRepository.AddBook(bookDtAdd);

        return Created();
    }
    
    
}