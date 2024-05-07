using System.Data.SqlClient;
using apbd_kol1.Models;

namespace ExampleTest1.Repositories;

public class BooksRepository : IBooksRepository
{
    private readonly IConfiguration _configuration;
    public BooksRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<bool> DoesBookExist(int id)
    {
        var query = "SELECT 1 FROM BOOKS WHERE PK = @ID";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }

    public async Task<bool> DoesAuthorExist(int id)
    {
        var query = "SELECT 1 FROM AUTHORS WHERE PK = @ID";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }

    public async Task<BookDTO> GetBook(int id)
    {
        var query = @"SELECT 
							Books.PK AS BookID,
							Books.title AS BookTitle
							[AUTHORS]
							Authors.ID as AuthorID,
							FirstName,
							LastName,
						FROM BOOKS
						JOIN Books_author ON Books.PK = Books_authors.FK_Book
						JOIN Author ON Books_author_FK_Author = Author.PK
						WHERE Books.PK = @ID";
	    
	    await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
	    await using SqlCommand command = new SqlCommand();

	    command.Connection = connection;
	    command.CommandText = query;
	    command.Parameters.AddWithValue("@ID", id);
	    
	    await connection.OpenAsync();

	    var reader = await command.ExecuteReaderAsync();

	    var bookIdOrdinal = reader.GetOrdinal("BookID");
	    var bookTitleOrdinal = reader.GetOrdinal("BookTitle");
	    var authorIdOrdinal = reader.GetOrdinal("AuthorID");
	    var firstNameOrdinal = reader.GetOrdinal("FirstName");
	    var lastNameOrdinal = reader.GetOrdinal("LastName");
	    

	    BookDTO bookDto = null;
	    AuthorDTO authorDto = null;

	    while (await reader.ReadAsync())
	    {
		    {
			    bookDto = new BookDTO()
			    {
				    Id = reader.GetInt32(bookIdOrdinal),
				    Title = reader.GetString(bookTitleOrdinal),
				    authorDto = new AuthorDTO()
				    {
					    Id = reader.GetInt32(authorIdOrdinal),
					    FirstName = reader.GetString(firstNameOrdinal),
					    LastName = reader.GetString(lastNameOrdinal),
				    },
			    };
		    }
	    }

	    if (bookDto is null) throw new Exception();
        
        return bookDto;
    }
    public async Task<int> AddBook(BookADD bookDto)
    {
	    var insert = @"INSERT INTO Books VALUES(@PK, @Title);
					   SELECT @@IDENTITY AS PK; 
					   INSERT INTO Authors VALUES (@PK, @First_na,@Last_na)
					   SELECT @@IDENTITY AS PK;
					   INSERT INTO books_authors VALUES(@FK_book, @ FK_author)";
	    
	    await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
	    await using SqlCommand command = new SqlCommand();
	    
	    command.Connection = connection;
	    command.CommandText = insert;
	    
	    command.Parameters.AddWithValue("@Title", bookDto.Title);
	    command.Parameters.AddWithValue("@First_na", bookDto.authorDto.FirstName);
	    command.Parameters.AddWithValue("@Last_na", bookDto.authorDto.LastName);
	    
	    await connection.OpenAsync();
	    
	    var id = await command.ExecuteScalarAsync();

	    if (id is null) throw new Exception();
	    
	    return Convert.ToInt32(id);
    }
}