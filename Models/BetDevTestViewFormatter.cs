using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using BetDevTestClassLibrary.Model;


namespace BetgDevTestWebsite.Models
{

    public class BetDevTestViewFormatter : IBetDevTestViewFormatter
    {
        public BetDevTestViewModel FormatViewModel(IRestResponse response1, BetDevTestViewModel betViewModel, string modelName)
        {
            using (var textReader = new StringReader(response1.Content.ToString()))
            {
                using (var reader = new JsonTextReader(textReader))
                {
                    if (modelName == "Products")
                    {
                        betViewModel.Products = new JsonSerializer().Deserialize(reader, typeof(List<Product>)) as List<Product>;

                    }
                    else if (modelName == "Carts")
                    {
                        betViewModel.Carts = new JsonSerializer().Deserialize(reader, typeof(List<Cart>)) as List<Cart>;
                    }
                    else if (modelName == "Cart")
                    {
                        betViewModel.Cart = new JsonSerializer().Deserialize(reader, typeof(Cart)) as Cart;
                    }
                    else if (modelName == "CartProductViews")
                    {
                        betViewModel.CartProductsViews = new JsonSerializer().Deserialize(reader, typeof(List<CartProductsView>)) as List<CartProductsView>;
                    }
                    else if (modelName == "OrderHistories")
                    {
                        betViewModel.OrderHistory = new JsonSerializer().Deserialize(reader, typeof(List<OrderHistory>)) as List<OrderHistory>;
                    }
                    //else if (modelName == "OrderHistory")
                    //{
                    //    betViewModel.OrderHistory = new JsonSerializer().Deserialize(reader, typeof(OrderHistory)) as OrderHistory;
                    //}
                }
            }
            return betViewModel;
        }


    }
}
