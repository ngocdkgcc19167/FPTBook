﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FPTBook.Models
{
    public class OrderDetail
    {
       
        [Key]
        [Column(Order = 0)]
        public int orderID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int bookID { get; set; }
        public double price { get; set; }
        [Required]
        [Range(0, 1000, ErrorMessage = "Please in input positive number")]
        public int quantity { get; set; }
        public double amount { get; set; }
        public virtual Order Order { get; set; }
        public virtual Book Book { get; set; }

    }
}