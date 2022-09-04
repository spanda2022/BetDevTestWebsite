using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetDevTestClassLibrary.Model;

namespace BetgDevTestWebsite.Models
{
   
    public class BetDevTestViewModel 
    {

            public List<Product> Products { get; set; }
            public List<Cart> Carts { get; set; }

            public Cart Cart { get; set; }
            public List<CartProductsView> CartProductsViews { get; set; }
            public List<OrderHistory> OrderHistories { get; set; }

        public List<OrderHistory> OrderHistory { get; set; }

    }
  
    }

