using System;
using Digikala.Core.Interfaces.Generic;
using Digikala.DTOs.FormDto.Public;

namespace Digikala.Core.Services.Generic
{
    public class PaginationRepository : IPaginationRepository
    {
        public PaginationDto CreatePagination(int take = 30, int resultCount = 1, int pageId = 1)
        {
            if ((pageId - 1) * take >= resultCount)
            {
                pageId = 1;
            }
            var skip = (pageId - 1) * take;
            var pageCount = (int)Math.Ceiling(resultCount / (decimal)take);
            var startPage = pageId - 5;
            var endPage = pageId + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > pageCount)
            {
                endPage = pageCount;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }
            return new PaginationDto()
            {
                EndPage = endPage,
                PageCount = pageCount,
                Skip = skip,
                StartPage = startPage,
                CurrentPage = pageId,
                PageId = pageId
            };
        }
    }
}