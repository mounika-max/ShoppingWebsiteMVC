﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ShoppingWebsiteMVC.Models
{
    public class CartContext:DbContext
    {
        public CartContext():base("name=DBconfig")
        {

        }
        public DbSet<Cart> Carts { get; set; }
    }
}