using BookStore.Core.Models;
using BookStore.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookRepository _bookRepository;

        public BooksController(BookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookRepository.GetAll();
            return Ok(books);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var book = await _bookRepository.GetById(id);
            if (book == null)
                return NotFound($"Книга с Id {id} не найдена");

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookRequest request)
        {
            var result = Book.Create(request.Title, request.Author, request.Price);

            if (result.IsFailure)
                return BadRequest(result.Error);

            await _bookRepository.Create(result.Value);

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateBookRequest request)
        {
            var existingBook = await _bookRepository.GetById(id);
            if (existingBook == null)
                return NotFound($"Книга с Id {id} не найдена");

            var validationResult = Book.Create(request.Title, request.Author, request.Price);
            if (validationResult.IsFailure)
                return BadRequest(validationResult.Error);

            await _bookRepository.Update(id, request.Title, request.Author, request.Price);
            return Ok("Данные книги успешно обновлены");
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingBook = await _bookRepository.GetById(id);
            if (existingBook == null)
                return NotFound($"Книга с Id {id} не найдена");

            await _bookRepository.Delete(id);
            return NoContent();
        }

        public record CreateBookRequest(string Title, string Author, decimal Price);
    }
}
