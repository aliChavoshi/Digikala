using System.Collections.Generic;

namespace Digikala.DTOs.FormGenericDto.Public
{
    public class GetAllGenericByPaginationDto<T>
    {
        public IEnumerable<T> List { get; set; }
        public PaginationDto PaginationDto { get; set; }
    }
}