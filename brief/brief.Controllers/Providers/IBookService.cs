﻿namespace brief.Controllers.Providers
{
    using System;
    using System.Threading.Tasks;
    using Models;

    public interface IBookService
    {
        Task<BookModel> CreateBook(BookModel book);
        Task<BookModel> UpdateBook(BookModel book);
        Task RemoveBook(Guid id);
    }
}
