﻿using ShoppingWebsiteMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace ShoppingWebsiteMVC.Controllers
{
    public class AdminController : Controller
    {
        // GET: Adminproduct
        ShoppingContext db = new ShoppingContext();
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }
        //Get details of a product 
        [Authorize]
        public ActionResult Details(string ProductId)
        {
            if (ProductId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(ProductId);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        //Get create view of new product
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        //Post create new product
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,CategoryName,BrandName,Price,Units,Discount,SupplierName")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }
        //Edit Product details
        [Authorize]
        public ActionResult Edit(string ProductId)
        {
            if (ProductId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(ProductId);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        //Post edit product details
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,CategoryName,BrandName,Price,Units,Discount,SupplierName")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }
        //Get Delete product 
        [Authorize]
        public ActionResult Delete(string ProductId)
        {
            if (ProductId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(ProductId);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        //Post Delete product
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string ProductId)
        {
            Product product = db.Products.Find(ProductId);
            product.Units = 0;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult Settings()
        {
            return View();
        }
        //Get edit admin details
        [Authorize]
        public ActionResult AdminEdit()
        {
                string username = User.Identity.Name;
                User user = db.Users.FirstOrDefault(u => u.UserId.Equals(username));
                User model = new User();
                model.Firstname = user.Firstname;
                model.Lastname = user.Lastname;
                model.Address = user.Address;
                model.ContactNumber = user.ContactNumber;
                model.City = user.City;
                model.Country = user.Country;
                return View(model);
            
        }
        //Post admin edit details
        [HttpPost]
        [Authorize]
        public ActionResult AdminEdit(User usr)
        {
            
            string username = User.Identity.Name;
            User user = db.Users.FirstOrDefault(u => u.UserId.Equals(username));
            user.Firstname = usr.Firstname;
            user.Lastname = usr.Lastname;
            Session["Username"] = usr.Firstname + " " + usr.Lastname;
            user.Address = usr.Address;
            user.ContactNumber = usr.ContactNumber;
            user.City = usr.City;
            user.Country = usr.Country;
            user.Password = user.Password;
            user.ConfirmPassword = user.Password;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return View(usr);
            
        }
        //Dispose the database
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}