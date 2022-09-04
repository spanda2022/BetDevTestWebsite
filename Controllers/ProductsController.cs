using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using System.Net;
using BetDevTestClassLibrary.Model;
using BetgDevTestWebsite.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

using System.IO;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BetgDevTestWebsite.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private IBetDevTestViewFormatter betDevTestViewFormatter;
        int ipageNumber = 0;
       

        private IConfiguration _configuration { get; }
        public ProductsController(IConfiguration configuration, IBetDevTestViewFormatter _betDevTestViewFormatter)
        {
            _configuration = configuration;
            betDevTestViewFormatter = _betDevTestViewFormatter;
        }

        // GET: Products/ListProducts
    
     [Authorize]
        public IActionResult ListProducts(bool isFirstPage = true, int cntProductsToExtract = 2, int pg = 1, string pageType = "")
        {
            try
            {
                ipageNumber = pg;
                cntProductsToExtract = Int32.Parse( _configuration["cntProductsToExtract"]) ;

                if (pageType != "" && pg >= 1)
                {
                    if (pageType == "prev")
                    {
                        if (pg > 1)
                            pg = pg - 1;
                    }
                    else if (pageType == "next")
                        pg = pg + 1;
                    ipageNumber = pg;
                }
                else if (pageType == "")
                {
                    pg = 1;
                    
                }
                ViewBag.iPageNumber = pg.ToString();

                ViewBag.iPageNumber = ipageNumber.ToString();
                string clienturl = _configuration["BetDevTestAPI_URL"] + "/Products/ListProducts";                
                BetDevTestViewModel betViewModel = new BetDevTestViewModel();

                var client = new RestClient(clienturl);
                client.Timeout = -1;
                var request1 = new RestRequest(Method.GET);
                request1.AddHeader("Content-Type", "application/json");

                IRestResponse response1 = client.Execute(request1);
                if(!response1.IsSuccessful)
                    return RedirectToAction("Error","Home", new { errormsg = response1.Content.Replace('"',' ') });
                List<Product> products = new List<Product>();
                using (var textReader = new StringReader(response1.Content.ToString()))
                {
                    using (var reader = new JsonTextReader(textReader))
                    {
                        products = new JsonSerializer().Deserialize(reader, typeof(List<Product>)) as List<Product>;
                    }
                }
                return View("ListProducts", products.ToPagedList(pg, cntProductsToExtract));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error ListProducts");
            }
                        
        }

        // GET: Products/AddProduct
        public ActionResult AddProduct()
        {
            return View();
        }

        // POST: Products/AddProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        // public ActionResult AddProduct(IFormCollection collection)
        public ActionResult AddProduct(Product pNew)
        {
            try
            {
                if (ModelState.IsValid)
                {                    
                    pNew.IsActive = true;
                    string clienturl = _configuration["BetDevTestAPI_URL"] + "/Products/AddProduct?pName=" + pNew.Name.ToString() + "&pQty=" + pNew.Quantity + "&price=" + pNew.Price + "&imagePath=" + pNew.Image;

                    var client = new RestClient(clienturl);
                    client.Timeout = -1;
                    var request1 = new RestRequest(Method.POST);
                    request1.AddHeader("Content-Type", "application/json");

                    IRestResponse response1 = client.Execute(request1);

                    if (!response1.IsSuccessful)
                        return RedirectToAction("Error", "Home", new { errormsg = response1.Content });


                    return RedirectToAction("ListProducts", "Products");
                }
                return View();
            }
            catch
            {
                return View();                
            }
        }


        // POST: Products/DeleteProduct/5
        [HttpPost]

        public IActionResult DeleteProduct(int productId)
        {
            
            try
            {
                string clienturl = _configuration["BetDevTestAPI_URL"] + "/Products/DeleteProduct?productId=" + productId;
               
                var client = new RestClient(clienturl);
                client.Timeout = -1;
                var request1 = new RestRequest(Method.POST);
                request1.AddHeader("Content-Type", "application/json");

                IRestResponse response1 = client.Execute(request1);

                if (!response1.IsSuccessful)
                    return RedirectToAction("Error", "Home", new { errormsg = response1.Content });
                              
                return RedirectToAction("ListProducts", "Products");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { errormsg = ex.Message });
            }
        }
    }
}
