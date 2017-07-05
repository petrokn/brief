﻿namespace brief.Library
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Controllers.Models.BaseEntities;
    using Controllers.Models;
    using Controllers.Providers;
    using Entities;
    using Helpers;
    using Repositories;

    public class SeriesService : BaseImageService, ISeriesService
    {
        private readonly ISeriesRepository _seriesRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IEditionRepository _editionRepository;
        private readonly ICoverRepository _coverRepository;
        private readonly IMapper _mapper;

        public SeriesService(ISeriesRepository seriesRepository, 
                             IBookRepository bookRepository,
                             IEditionRepository editionRepository,
                             ICoverRepository coverRepository,
                             IMapper mapper)
        {
            Guard.AssertNotNull(seriesRepository);
            Guard.AssertNotNull(bookRepository);
            Guard.AssertNotNull(editionRepository);
            Guard.AssertNotNull(coverRepository);
            Guard.AssertNotNull(mapper);

            _seriesRepository = seriesRepository;
            _editionRepository = editionRepository;
            _coverRepository = coverRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<ResponseMessage<(Guid bookId, Guid seriesId)>> AddBookToSeries(Guid bookId, Guid seriesId)
        {
            var response = new ResponseMessage<(Guid bookId, Guid seriesId)>();

            return response;
        }

        public async Task<ResponseMessage<(Guid bookId, Guid seriesId)>> RemoveBookFromSeries(Guid bookId, Guid seriesId)
        {
            var response = new ResponseMessage<(Guid bookId, Guid seriesId)>();

            if (await _seriesRepository.RemoveBookFromSeries(bookId, seriesId) == 0)
            {
                response.RawData = $"Linked record with {seriesId} and {bookId} wasn't found.";
                return response;
            }

            response.Payload = (bookId, seriesId);
            return response;
        }

        public async Task<BaseResponseMessage> CreateSeries(SeriesModel series)
        {
            var newSeries = _mapper.Map<Series>(series);

            var response = new BaseResponseMessage();

            if (!await _seriesRepository.CheckSeriesForUniqueness(newSeries))
            {
                response.RawData = $"Series {newSeries.Name} already existing with similar data.";
                return response;
            }

            var createdSeriesId = await _seriesRepository.CreateSerires(newSeries);

            response.Id = createdSeriesId;
            return response;
        }

        public async Task<BaseResponseMessage> UpdateSeries(SeriesModel series)
        {
            var newSeries = _mapper.Map<Series>(series);

            var response = new BaseResponseMessage();

            var seriesToUpdate = await _seriesRepository.GetSeries(newSeries.Id);

            if (seriesToUpdate == null)
            {
                response.RawData = $"Series with {newSeries.Id} wasn't found.";
                return response;
            }
            
            if (seriesToUpdate.Equals(newSeries))
            {
                response.RawData = $"Series {newSeries.Name} already existing with similar data.";
                return response;
            }

            await _seriesRepository.UpdateSerires(seriesToUpdate);

            response.Id = newSeries.Id;
            return response;
        }

        public async Task<BaseResponseMessage> RemoveSeries(Guid id, bool removeBooks)
        {
            var response = new BaseResponseMessage();

            var seriesToRemove = await _seriesRepository.GetSeries(id);

            if (seriesToRemove == null)
            {
                response.RawData = $"Series with {id} wasn't found.";
                return response;
            }

            if (removeBooks)
            {
                var editionsToRemove = await _editionRepository.GetEditionsByBookOrPublisher(id);

                if (editionsToRemove != null)
                {
                    editionsToRemove.ForEach(async e =>
                    {
                        var covers = await _coverRepository.GetCoversByEdition(e.Id);

                        if (covers != null)
                        {
                            covers.ForEach(c => TryCleanUp(c.LinkTo));

                            await _coverRepository.RemoveCovers(covers);
                        }
                    });

                    await _editionRepository.RemoveEditions(editionsToRemove);
                }

                if (seriesToRemove.BooksInSeries != null)
                {
                    await _bookRepository.RemoveBooks(seriesToRemove.BooksInSeries);
                }
            }
            
            await _seriesRepository.RemoveSerires(seriesToRemove);

            response.Id = id;
            return response;
        }
    }
}
