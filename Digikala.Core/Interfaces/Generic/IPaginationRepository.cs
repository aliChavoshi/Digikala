using Digikala.DTOs.FormGenericDto.Public;

namespace Digikala.Core.Interfaces.Generic
{
    public interface IPaginationRepository
    {
        PaginationDto CreatePagination(int take = 30, int resultCount = 1, int pageId = 1);
    }
}