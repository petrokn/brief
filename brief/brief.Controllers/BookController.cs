﻿namespace brief.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Models;
    using Providers;

    public class BookController : ApiController
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            if (bookService == null)
            {
                throw new ArgumentNullException(nameof(bookService));
            }

            _bookService = bookService;
        }

        [HttpPost]
        public async Task<BookModel> Create([FromBody] BookModel book)
        {
            return await _bookService.CreateBook(book);
        }

        public BookModel Get(BookModel book)
        {
            return null;
        }
    }
}