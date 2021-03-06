﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Digikala.DataAccessLayer.Entities.Identity;

namespace Digikala.DataAccessLayer.Entities
{
    public class BaseEntity : Commands
    {
        [Key]
        [Column(Order = 1)]
        public int Id { get; set; }

        #region Relations

        [Required]
        [Display(Name = "ثبت کننده")]
        public int CreatorUser { get; set; }

        [Display(Name = "ویرایش کننده")]
        public int? ModifierUser { get; set; }

        [ForeignKey(nameof(CreatorUser))]
        public User UserCreator { get; set; }

        [ForeignKey(nameof(ModifierUser))]
        public User UserModifier { get; set; }

        #endregion
    }
}