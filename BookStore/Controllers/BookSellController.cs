using BookStore.Core.Models;
using BookStore.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookSellsController : ControllerBase
    {
        private readonly BookSellRepository _sellRepository;
        private readonly BookRepository _bookRepository;

        public BookSellsController(BookSellRepository sellRepository, BookRepository bookRepository)
        {
            _sellRepository = sellRepository;
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sales = await _sellRepository.GetAll();
            return Ok(sales);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request)
        {
            if (request.Items == null || !request.Items.Any())
                return BadRequest("Нельзя оформить пустую продажу. Добавьте хотя бы одну книгу.");

            var saleResult = BookSell.Create();
            var sale = saleResult.Value;

            foreach (var itemRequest in request.Items)
            {
                var book = await _bookRepository.GetById(itemRequest.BookId);
                if (book == null)
                    return NotFound($"Книга с Id {itemRequest.BookId} не найдена в каталоге. Продажа отменена.");

                var addItemResult = sale.AddItem(book, itemRequest.Quantity);

                if (addItemResult.IsFailure)
                    return BadRequest(addItemResult.Error);
            }

            await _sellRepository.Create(sale);

            return StatusCode(201, sale);
        }
    }

    public record CreateSaleRequest(List<SaleItemDto> Items);
    public record SaleItemDto(Guid BookId, int Quantity);
}
