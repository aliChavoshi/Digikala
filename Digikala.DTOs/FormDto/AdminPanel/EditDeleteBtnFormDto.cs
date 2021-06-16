using System.Collections.Generic;

namespace Digikala.DTOs.FormDto.AdminPanel
{
    public class EditDeleteBtnFormDto
    {
        public bool ModalEditButton { get; set; } = false;
        public bool ModalDeleteButton { get; set; } = false;
        public string EditMethodOnclick { get; set; } = "";
        public string DeleteMethodOnclick { get; set; } = "";
        public string ActionNameEdit { get; set; } = "Edit";
        public string ActionNameDelete { get; set; } = "Delete";
        public int Id { get; set; }
    }
}