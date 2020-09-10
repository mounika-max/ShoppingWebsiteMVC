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
        UserContext dbo = new UserContext();
        [Authorize]
        public ActionResult Index()
        {
            return View(dbo.Products.ToList());
        }
        //Get details of a product 
        [Authorize]
        public ActionResult Details(string ProductId)
        {
            if (ProductId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = dbo.Products.Find(ProductId);
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
        public ActionResult Create([Bind(Include = "ProductId,ProductName,CategoryName,Price,Units,Discount,SupplierName")] Product product)
        {
            if (ModelState.IsValid)
            {
                dbo.Products.Add(product);
                dbo.SaveChanges();
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
            Product product = dbo.Products.Find(ProductId);
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
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,CategoryName,Price,Units,Discount,SupplierName")] Product product)
        {
            if (ModelState.IsValid)
            {
                dbo.Entry(product).State = EntityState.Modified;
                dbo.SaveChanges();
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
            Product product = dbo.Products.Find(ProductId);
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
            Product product = dbo.Products.Find(ProductId);
            dbo.Products.Remove(product);
            dbo.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbo.Dispose();
            }
            base.Dispose(disposing);
        }
        //Get edit admin details
        [Authorize]
        public ActionResult AdminEdit()
        {
            using (UserContext db = new UserContext())
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
        }
        //Post admin edit details
        [HttpPost]
        public ActionResult AdminEdit(User usr)
        {
            using (UserContext db = new UserContext())
            {
                if (ModelState.IsValid)
                {
                    string username = User.Identity.Name;
                    User user = db.Users.FirstOrDefault(u => u.UserId.Equals(username));
                    user.Firstname = usr.Firstname;
                    user.Lastname = usr.Lastname;
                    user.Address = usr.Address;
                    user.ContactNumber = usr.ContactNumber;
                    user.City = usr.City;
                    user.Country = usr.Country;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "User");

                }
                return View(usr);
            }
        }
    }
}