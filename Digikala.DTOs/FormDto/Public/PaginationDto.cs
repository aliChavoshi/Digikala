namespace Digikala.DTOs.FormDto.Public
{
    public class PaginationDto
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int Skip { get; set; }
        public int PageId { get; set; } = 1;
    }
}