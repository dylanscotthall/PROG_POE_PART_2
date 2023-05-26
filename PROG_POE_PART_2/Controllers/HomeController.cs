using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using NuGet.Protocol;
using PROG_POE_PART_2.Model;
using PROG_POE_PART_2.Models;
using System;
using System.Diagnostics;
using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace PROG_POE_PART_2.Controllers
{
    public class HomeController : Controller
    {
        ProgPoeContext db;
        bool showMore = false;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            db = new ProgPoeContext();
        }
        
        //returns view with a farmer and all their products
        public IActionResult IndexFarmer(int? id)
        {
            Farmer temp = db.Farmers.Where(f => f.FarmerId == id).FirstOrDefault();
            ViewBag.user = temp;
            ViewBag.products = db.Products.Where(p => p.FarmerId == id).ToList();
            return View();
        }
        //returns a list of farmers
        public IActionResult IndexEmployee(int? id)
        {
            Console.WriteLine(id.ToString());
            Employee temp = db.Employees.Where(e => e.EmployeeId == id).FirstOrDefault();
            ViewBag.user = temp;
            ViewBag.farmers = db.Farmers.ToList();
            ViewBag.products = db.Products.ToList();
            return View();
        }
        //returns sign in screen
        public IActionResult SignIn()
        {
            return View();
        }
        //allows a person to sign in
        [HttpPost]
        public IActionResult SignIn(SignInOrUp user)
        {
            Farmer farmer = db.Farmers.Where(f => f.FarmerUsername == user.Username).FirstOrDefault();
            if (farmer != null && farmer.FarmerPassword == user.Password)
            {
                return RedirectToAction("IndexFarmer", new { id = farmer.FarmerId });
            }
            Employee employee = db.Employees.Where(e => e.EmployeeUsername == user.Username).FirstOrDefault();
            if (employee != null && employee.EmployeePassword == user.Password)
            {
                return RedirectToAction("IndexEmployee", new { id = employee.EmployeeId });
            }
            ViewBag.error = true;
            return View();
        }
        //returns a view that allows a employee to create a new farmer
        public IActionResult CreateFarmer(int? employeeId, bool? error)
        {
            ViewBag.employeeId = employeeId;
            ViewBag.error = error;
            return View();
        }
        //processes the request to create a new farmer and adds it to the db
        [HttpPost]
        public async Task<IActionResult> CreateFarmer(int? employeeId, Farmer farmer)
        {
            try
            {
                await db.Farmers.AddAsync(farmer);
                await db.SaveChangesAsync();
            }
            catch(Exception e)
            {
                return RedirectToAction("CreateFarmer", new { employeeId = employeeId, error = true });
            }
            return RedirectToAction("IndexEmployee", new { id = employeeId });
        }
        //allows an employee to edit data of a farmer
        public IActionResult EditFarmer(int? farmerId, int? employeeId, bool? error)
        {
            ViewBag.Farmer = db.Farmers.Where(f => f.FarmerId == farmerId).FirstOrDefault();
            ViewBag.employeeId = employeeId;
            ViewBag.error = error;
            return View();
        }
        //processes the request to edit a farmer
        [HttpPost]
        public async Task<IActionResult> EditFarmer(int? employeeId,int? farmerId, Farmer farmer)
        {
            try
            {
                var result = await db.Farmers.Where(f => f.FarmerId == farmerId).FirstOrDefaultAsync();
                if (result != null)
                {
                    result.FarmerName = farmer.FarmerName;
                    result.FarmerSurname = farmer.FarmerSurname;
                    result.FarmName = farmer.FarmName;
                    result.FarmerUsername = farmer.FarmerUsername;
                    result.FarmerPassword = farmer.FarmerPassword;
                    await db.SaveChangesAsync();
                }
            }
            catch(Exception e)
            {
                return RedirectToAction("EditFarmer", new { farmerId = farmerId, employeeId = employeeId, error = true});
            }
            return RedirectToAction("IndexEmployee", new { id = employeeId });
        }
        //simple sort method to allows a employee to sort farmer data
        public IActionResult Sort(int? employeeId, int? farmerId, String? sort)
        {
            return RedirectToAction("ViewFarmer", new { farmerId = farmerId, employeeId = employeeId, sorted = sort});
        }
        //allows employee to view data of a farmer
        public IActionResult ViewFarmer(int? farmerId, int? employeeId, String? sorted)
        {
            Console.WriteLine(sorted);
            ViewBag.Farmer = db.Farmers.Where(f => f.FarmerId == farmerId).FirstOrDefault();
            var products = db.Products.Where(p => p.FarmerId == farmerId).ToList();
            if (sorted != null)
            {
                if (sorted == "date")
                {
                    products = products.OrderBy(item => item.DateAdded).ToList();
                }
                if(sorted == "type")
                {
                    products = products.OrderBy(item => item.ProductType).ToList();
                }
            }
            ViewBag.products = products;
            ViewBag.employeeId = employeeId;
            return View();
        }
        //processes the request to view a farmer and returns to the index page
        [HttpPost]
        public async Task<IActionResult> ViewFarmer(int? employeeId)
        {
            return RedirectToAction("IndexEmployee", new { id = employeeId });
        }
        //processes a request to delete a farmer
        public async Task<IActionResult> DeleteFarmer(int? farmerId, int? employeeId)
        {   
            var farmer = await db.Farmers.FindAsync(farmerId);
            db.Farmers.Remove(farmer);
            await db.SaveChangesAsync();
            return RedirectToAction("IndexEmployee", new { id = employeeId});
        }
        //returns a page allow a farmer to create new product
        public IActionResult CreateProduct(int? farmerId, bool? error)
        {
            ViewBag.farmerId = farmerId;
            ViewBag.error = error;
            return View();
        }
        //processes request for farmer to create new product and adds it to the database
        [HttpPost]
        public async Task<IActionResult> CreateProduct(int? farmerId, Product product)
        {
            product.DateAdded = DateTime.Now;
            try
            {
                await db.Products.AddAsync(product);
                await db.SaveChangesAsync();
            }
            catch(Exception e) {
                return RedirectToAction("CreateProduct", new { farmerId = farmerId , error = true});
            }
            return RedirectToAction("IndexFarmer", new { id = farmerId });
        }
        //allows a farmer to edit a product
        public IActionResult EditProduct(int? farmerId, int? productId, bool? error)
        {
            ViewBag.Product = db.Products.Where(p => p.ProductId == productId).FirstOrDefault();
            ViewBag.farmerId = farmerId;
            ViewBag.error = error;
            return View();
        }
        //processes the request to edit a product
        [HttpPost]
        public async Task<IActionResult> EditProduct(int? farmerId, int? productId, Product product)
        {
            try
            {
                var result = await db.Products.Where(p => p.ProductId == productId).FirstOrDefaultAsync();
                if (result != null)
                {
                    result.ProductName = product.ProductName;
                    result.ProductPrice = product.ProductPrice;
                    result.ProductStock = product.ProductStock;
                    result.ProductType = product.ProductType;
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("EditProduct", new { farmerId = farmerId, productId = productId, error = true});
            }
            return RedirectToAction("IndexFarmer", new { id = farmerId });
        }
        //allows a farmer to view a specific product
        public IActionResult ViewProduct(int? farmerId, int? productId)
        {
            ViewBag.Product = db.Products.Where(p => p.ProductId == productId).FirstOrDefault();
            ViewBag.farmerId = farmerId;
            return View();
        }
        //processes the request to return the viewed product back to the index page
        [HttpPost]
        public async Task<IActionResult> ViewProduct(int? farmerId)
        {
            return RedirectToAction("IndexFarmer", new { id = farmerId });
        }
        //allows a farmer to delete a product
        public async Task<IActionResult> DeleteProduct(int? farmerId, int? productId)
        {
            var product = await db.Products.FindAsync(productId);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("IndexFarmer", new { id = farmerId });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}