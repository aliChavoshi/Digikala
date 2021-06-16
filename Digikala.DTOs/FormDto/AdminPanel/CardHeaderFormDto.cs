namespace Digikala.DTOs.FormDto.AdminPanel
{
    public class CardHeaderFormDto
    {
        public bool ModalButton { get; set; } = false;
        public string CardLabel { get; set; } = "جستجو پیشرفته";
        public string ActionName { get; set; } = "";
        public string NameMethodOnclick { get; set; } = "";
        public string ButtonName { get; set; } = "رکورد جدید";
    }
}