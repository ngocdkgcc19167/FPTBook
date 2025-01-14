﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FPTBook.DB;
using FPTBook.Models;

namespace FPTBook.Controllers
{
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Books
        public ActionResult Index()
        {
            var books = db.Books.Include(b => b.Category);
            return View(books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.categoryID = new SelectList(db.Categories, "categoryID", "categoryName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "bookID,bookName,description,Img,stock_quantity,price,categoryID")] Book book, HttpPostedFileBase file)
        {
           string pic = System.IO.Path.GetFileName(file.FileName);
           string checkImg = Path.GetExtension(file.FileName);
            if (checkImg.ToLower() == ".jpg" || checkImg.ToLower() == ".jpeg" || checkImg.ToLower() == ".png" && file != null && file.ContentLength > 0)
            {
                string path = Path.Combine(Server.MapPath("~/assets/img"), Path.GetFileName(file.FileName));
                file.SaveAs(path);
                book.Img = pic.ToString();
                if (ModelState.IsValid)
                {
                    db.Books.Add(book);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.CheckError = "Invavlid file";
            }

            ViewBag.categoryID = new SelectList(db.Categories, "categoryID", "categoryName", book.categoryID);
            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoryID = new SelectList(db.Categories, "categoryID", "categoryName", book.categoryID);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book book, HttpPostedFileBase file, int id)
        {
            if (ModelState.IsValid)
            {
                Book book1 = db.Books.Find(id);
                if (file != null && file.ContentLength > 0)
                {
                    string pic= "";
                    string file_name = book.Img;
                    string path1 = Server.MapPath("~/assets/img/");
                    string checkimg = Path.GetExtension(file.FileName);
                    if (checkimg.ToLower() == ".jpg" || checkimg.ToLower() == ".jpeg" || checkimg.ToLower() == ".png")
                    {
                        FileInfo file1 = new FileInfo(path1 + file_name);
                        if (file1.Exists)
                        {
                            file1.Delete();
                        }
                        pic = System.IO.Path.GetFileName(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/assets/img/"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        book1.Img = pic.ToString();
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.CheckError = "*Invavlid file";
                    }
                }
                else
                {
                    book1.bookName = book.bookName;
                    book1.stock_quantity = book.stock_quantity;
                    book1.price = book.price;
                    book1.description = book.description;
                    book1.categoryID = book.categoryID;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            ViewBag.categoryID = new SelectList(db.Categories, "categoryID", "categoryName", book.categoryID);
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
