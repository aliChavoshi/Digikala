using System.Collections.Generic;

namespace Digikala.DTOs.FormDto.AdminPanel
{
    public class FormBodyDto
    {
        public bool ExistOrder { get; set; } = false;
        public bool ExistSearch { get; set; } = true;
        public bool ExistDateSearch { get; set; } = false;
        public bool ExistIsActive { get; set; } = false;
        public bool ExistPagination { get; set; } = true;
        public Dictionary<int, string> InputsOrder { get; set; } 
        public Dictionary<string, string> InputsSearch { get; set; }
    }
}