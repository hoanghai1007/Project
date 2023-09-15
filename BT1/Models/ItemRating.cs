﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BT1_project.Models
{
    [Table("Rating")]
    public class ItemRating
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Star { get; set; }
    }
}
