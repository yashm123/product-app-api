using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;

namespace ProductApi.Controllers
{
    public class ValuesController : Controller
    {
        TrainingContext db = new TrainingContext();
         [Route("api/GetProducts")]
        public JsonResult GetProducts()
        {
            return Json(db.Products);
        }

      [Route("api/ValidateUser")]
    
     public JsonResult ValidateUser(string Email_ID, string Password)
     { 
           string msg = "";
            bool check = db.UserInfo.Any(a => a.EmailId == Email_ID && a.Password == Password);
            if (check)
            {
                if (Email_ID == "admin@admin.com")
                {
                    msg = "AdminPage";

                }
                else
                {
                    msg = "UserPage";
                }
            }
            else
            {
                msg = "InvalidUser";
            }
          //  Json(msg);
             //Context.Response.Write(js.Serialize(msg));
             return Json(msg); 
                 
     }

    [Route("api/AddUser")]
    public JsonResult AddUser(string Email_ID, string Password, string Name, string City, string Gender, long Mobile_Number)
    {
          string msg = "";
          if (db.UserInfo.Any(a => a.EmailId == Email_ID))
            {
                msg = "UserExists";  //user already exist
            }
            else
            {
                UserInfo user = new UserInfo() { EmailId = Email_ID, Password = Password, Name = Name, City = City, Gender = Gender, MobileNumber = Mobile_Number };
                db.UserInfo.Add(user);
                db.SaveChanges();
                msg = "Registered"; //you are succesfully registered
            }
                return Json(msg);
    }
    
        [Route("api/AddProduct")]
        public JsonResult AddProduct(int ProductId, string Name, int Price, string Description, int Quantity)
        {
            string msg = "";
            bool check = db.Products.Any(a => a.ProductId == ProductId);
            if (check)
            {
                msg = "Product Id Already Exists";
            }
            else
            {
                Products p = new Products() { ProductId = ProductId, Name = Name, Price = Price, Description = Description, Quantity = Quantity };
                db.Products.Add(p);
                db.SaveChanges();
                msg = "ProductAdded";
            }
            return Json(msg);
        }

        [Route("api/DeleteProduct")]
        public JsonResult DeleteProduct(int ProductId)
            {
            string msg = "";
            Products p = db.Products.Find(ProductId);
            db.Products.Remove(p);
            db.SaveChanges();
            msg = "Deleted";
            return Json(msg);
            }
            
        [Route("api/GetProductById")]
         public Products GetProductById(int ProductId)
         {
            return (db.Products.Find(ProductId));
         }
          [Route("api/UpdateProduct")]
        public JsonResult UpdateProduct(int ProductId, string Name, int Price, string Description, int Quantity)
        {
            string msg = "Updated";
            Products p = db.Products.Find(ProductId);
            p.Name = Name;
            p.Price = Price;
            p.Description = Description;
            p.Quantity = Quantity;
        //    db.Entry(p).State = System.Data.Entity.EntityState.Modified;
           db.Products.Update(p);
            db.SaveChanges();
            return Json(msg);
        }
           [Route("api/AddToCart")]
          public JsonResult AddToCart(string Email_ID, int Product_Id, int Quantity)
          {
               string msg = "";
               bool check = db.Cart.Any(a => a.EmailId == Email_ID && a.ProductId == Product_Id);
            if (check)
            {
                var obj = db.Cart.FirstOrDefault(c => c.EmailId== Email_ID && c.ProductId == Product_Id);
                obj.Quantity += Quantity;
               // db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                db.Cart.Update(obj);
                db.SaveChanges();
                msg = "Updated";
            }
            else
            {
                Cart c = new Cart() { EmailId = Email_ID, ProductId = Product_Id, Quantity = Quantity };
                db.Cart.Add(c);
                db.SaveChanges();
                msg = "Inserted";
            }
                  return Json(msg);
          }
         
          [Route("api/RemoveFromCart")]
            public JsonResult RemoveFromCart(string Email_ID, int Product_Id)
        {
            string msg = "";
            Cart obj = (Cart)(db.Cart.FirstOrDefault(c => c.EmailId == Email_ID && c.ProductId == Product_Id));
            db.Cart.Remove(obj);
            db.SaveChanges();
            msg = "Removed";
            return Json(msg);
        }
         [Route("api/GetUserCartDetails")]
        public IEnumerable<Cart> GetUserCartDetails(string Email_ID)
        {
            var cart = db.Cart.Where(c => c.EmailId == Email_ID);
             return cart;
        }

        [Route("api/EmptyCart")]
          public JsonResult EmptyCart(string Email_ID)
        {
            var cart = db.Cart.Where(c => c.EmailId == Email_ID);
            db.Cart.RemoveRange(cart);
            db.SaveChanges();
            return Json("Empty");
        }
         [Route("api/ProductsInCart")]
        public JsonResult ProductsInCart(string Email_ID)
        {
            var cart = from c in db.Cart
                       join p in db.Products
                       on c.ProductId equals p.ProductId
                       where c.EmailId == Email_ID
                       select new
                       {
                           c.ProductId,
                           p.Name,
                           p.Price,
                           c.Quantity
                       };
          return Json(cart);
        }
         [Route("api/BuyProducts")]
           public JsonResult BuyProducts(string Email_ID)
        {
       
            var cart = db.Cart.Where(x => x.EmailId == Email_ID);
            var products = db.Products;
            foreach (var item in cart)
            {
                foreach (var product in products)
                {
                    if(item.ProductId==product.ProductId)
                    {
                        product.Quantity = product.Quantity - item.Quantity.Value;
                        //db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                        db.Products.Update(product);
                    }
                }
            }
         
            db.Cart.RemoveRange(cart);
            db.SaveChanges();
           return Json("Saved");
        }
 	[Route("api/Msg")]
	public JsonResult Msg()
	{
		return Json("Welcome");
	}
     
    }
}
