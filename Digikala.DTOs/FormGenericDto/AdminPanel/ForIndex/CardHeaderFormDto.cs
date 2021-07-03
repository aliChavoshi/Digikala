namespace Digikala.DTOs.FormGenericDto.AdminPanel.ForIndex
{
    public class CardHeaderFormDto
    {
        public bool ExistCardTitle { get; set; } = true;
        public bool ExistBtn { get; set; } = true;
        public bool ModalButton { get; set; } = false;
        public string CardLabel { get; set; } = "جستجو پیشرفته";
        public string ActionName { get; set; } = "";
        public string NameMethodOnclick { get; set; } = "";
        public string ButtonName { get; set; } = "رکورد جدید";
    }
}