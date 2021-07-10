﻿namespace Digikala.DTOs.FormGenericDto.AdminPanel.ForCreateOrEdit
{
    public class CardFooterCreateEditBtnForm
    {
        public string ActionSubmit { get; set; }
        public string ActionReturn { get; set; }
        public string LabelSubmit { get; set; } = "ثبت اطلاعات";
        public string LabelReturn { get; set; } = "بازگشت";
        public bool IsExistSubmit { get; set; } = true;
        public bool IsExistReturn { get; set; } = true;
    }
}