using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using BetgDevTestWebsite.Models;
using Microsoft.Extensions.Configuration;
using BetDevTestClassLibrary.Model;
using Microsoft.AspNetCore.Authorization;

namespace BetgDevTestWebsite.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {

        private IBetDevTestViewFormatter betDevTestViewFormatter;
        
        private IConfiguration _configuration { get; }
        private string UserEmail;

        public ShoppingCartController(IConfiguration configuration, IBetDevTestViewFormatter _betDevTestViewFormatter)
        {
            _configuration = configuration;
            betDevTestViewFormatter = _betDevTestViewFormatter;
        }

        // GET: ShoppingCart/ShowAllOrderHistory
        public ActionResult ShowOrderedProducts(int cartId,string OrderNo,DateTime OrderDate,string UserEmail,decimal TotalPrice)
        {
            string clienturl = _configuration["BetDevTestAPI_URL"] + "/ShoppingCart/ShowCartProducts?cartId=" + cartId + "&cartNo=";
            BetDevTestViewModel betViewModel = new BetDevTestViewModel();

            var client = new RestClient(clienturl);
            client.Timeout = -1;
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("Content-Type", "application/json");

            IRestResponse response1 = client.Execute(request1);


            if (!response1.IsSuccessful)
                return RedirectToAction("Error", "Home", new { errormsg = response1.Content.Replace('"', ' ') });

            betViewModel = betDevTestViewFormatter.FormatViewModel(response1, betViewModel, "CartProductViews");
            ViewBag.OrderNo = OrderNo;           
            ViewBag.OrderDate = OrderDate.ToString();
            ViewBag.UserEmail = UserEmail;
            ViewBag.TotalPrice = TotalPrice;

            return View("ShowOrderedProducts", betViewModel);

        }
        // GET: ShoppingCart/ShowAllOrderHistory
        public ActionResult ShowAllOrderHistory()
        {
            
            UserEmail = HttpContext.User.Identity.Name;
            
            string clienturl = _configuration["BetDevTestAPI_URL"] + "/ShoppingCart/ShowAllOrderHistory?UserEmail="+UserEmail;
            BetDevTestViewModel betViewModel = new BetDevTestViewModel();

            var client = new RestClient(clienturl);
            client.Timeout = -1;
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("Content-Type", "application/json");

            IRestResponse response1 = client.Execute(request1);

            if (!response1.IsSuccessful)
                return RedirectToAction("Error", "Home", new { errormsg = response1.Content.Replace('"', ' ') });

            betViewModel = betDevTestViewFormatter.FormatViewModel(response1, betViewModel, "OrderHistories");         

            return View("ShowAllOrderHistory", betViewModel);

        }


        // GET: ShoppingCart/ShowSingleOrderDetails
        public ActionResult ShowSingleOrderDetails(int orderId)
        {
            UserEmail = HttpContext.User.Identity.Name; 

            string clienturl = _configuration["BetDevTestAPI_URL"] + "/ShoppingCart/ShowSingleOrderDetails?UserEmail="+UserEmail+"&orderId=" + orderId;
            BetDevTestViewModel betViewModel = new BetDevTestViewModel();

            var client = new RestClient(clienturl);
            client.Timeout = -1;
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("Content-Type", "application/json");

            IRestResponse response1 = client.Execute(request1);

            if (!response1.IsSuccessful)
                return RedirectToAction("Error", "Home", new { errormsg = response1.Content.Replace('"', ' ') });

            betViewModel = betDevTestViewFormatter.FormatViewModel(response1, betViewModel, "OrderHistory");
            return View("ShowAllOrderHistory", betViewModel);

        }

        // GET: ShoppingCart/ShowShoppingCart
        public ActionResult ShowShoppingCart()
        {
            UserEmail = HttpContext.User.Identity.Name;
            string clienturl = _configuration["BetDevTestAPI_URL"] + "/ShoppingCart/ShowShoppingCart?UserEmail="+UserEmail;
            BetDevTestViewModel betViewModel = new BetDevTestViewModel();

            var client = new RestClient(clienturl);
            client.Timeout = -1;
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("Content-Type", "application/json");

            IRestResponse response1 = client.Execute(request1);

            if (!response1.IsSuccessful)
                return RedirectToAction("Error", "Home", new { errormsg = response1.Content.Replace('"', ' ') });

            betViewModel = betDevTestViewFormatter.FormatViewModel(response1, betViewModel, "Cart");


            clienturl = _configuration["BetDevTestAPI_URL"] + "/ShoppingCart/ShowCartProducts?cartId=" + betViewModel.Cart.Id+"&cartNo="+betViewModel.Cart.CardNo;
            

            client = new RestClient(clienturl);
            client.Timeout = -1;
            request1 = new RestRequest(Method.GET);
            request1.AddHeader("Content-Type", "application/json");

            response1 = client.Execute(request1);

            if (!response1.IsSuccessful)
                return RedirectToAction("Error", "Home", new { errormsg = response1.Content.Replace('"', ' ') });

            betViewModel = betDevTestViewFormatter.FormatViewModel(response1, betViewModel, "CartProductViews");

            return View("ShowShoppingCart", betViewModel);
                        
        }

        [HttpGet]
        public ActionResult AddProductToCart(string productId, string productName)
        {
            ViewBag.productId = productId;
            ViewBag.productName = productName;
            return View();
        }

        // POST: ShoppingCart/AddProductToCart
            [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProductToCart(int productId, int Qty)
        {
            try
            {
                UserEmail = HttpContext.User.Identity.Name;
                string clienturl = _configuration["BetDevTestAPI_URL"] + "/ShoppingCart/AddProductToCart?UserEmail="+UserEmail+ "&productId="+productId+ "&Qty="+Qty;
                BetDevTestViewModel betViewModel = new BetDevTestViewModel();

                var client = new RestClient(clienturl);
                client.Timeout = -1;
                var request1 = new RestRequest(Method.POST);
                request1.AddHeader("Content-Type", "application/json");

                IRestResponse response1 = client.Execute(request1);
                if (!response1.IsSuccessful)
                    return RedirectToAction("Error", "Home", new { errormsg = response1.Content.Replace('"', ' ') });

                              
                return RedirectToAction("ShowShoppingCart", "ShoppingCart");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST: ShoppingCart/RemoveProductFromCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveProductFromCart( int productId, int Qty)
        {
            try
            {
                UserEmail = HttpContext.User.Identity.Name;

                string clienturl = _configuration["BetDevTestAPI_URL"] + "/ShoppingCart/RemoveProductFromCart?UserEmail=" + UserEmail + "&productId=" + productId + "&Qty=" + Qty;
                BetDevTestViewModel betViewModel = new BetDevTestViewModel();

                var client = new RestClient(clienturl);
                client.Timeout = -1;
                var request1 = new RestRequest(Method.POST);
                request1.AddHeader("Content-Type", "application/json");

                IRestResponse response1 = client.Execute(request1);
                if (!response1.IsSuccessful)
                    return RedirectToAction("Error", "Home", new { errormsg = response1.Content.Replace('"', ' ') });

                return RedirectToAction("ShowShoppingCart", "ShoppingCart");

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // POST: ShoppingCart/CheckOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut( int CartId)
        {
            try
            {
                UserEmail = HttpContext.User.Identity.Name;
                string clienturl = _configuration["BetDevTestAPI_URL"] + "/ShoppingCart/CheckOut?UserEmail=" + UserEmail + "&CartId=" + CartId;
                BetDevTestViewModel betViewModel = new BetDevTestViewModel();

                var client = new RestClient(clienturl);
                client.Timeout = -1;
                var request1 = new RestRequest(Method.POST);
                request1.AddHeader("Content-Type", "application/json");

                IRestResponse response1 = client.Execute(request1);
            
                return RedirectToAction("Error", "Home", new { errormsg = response1.Content.Replace('"', ' ') });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
