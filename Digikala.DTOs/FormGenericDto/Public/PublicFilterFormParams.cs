namespace Digikala.DTOs.FormGenericDto.Public
{
    public class PublicFilterFormParams
    {
        public string OrderFrom { get; set; } = "0";
        public bool IsActive { get; set; }
        public string StartDate { get; set; } = "";
        public string EndDate { get; set; } = "";
        public Order Order { get; set; } = Order.Asc;
        public int SendTake { get; set; } = 30;
        public int SendPageId { get; set; } = 1;
    }

    public enum Order
    {
        Des = 0,
        Asc = 1
    }
}