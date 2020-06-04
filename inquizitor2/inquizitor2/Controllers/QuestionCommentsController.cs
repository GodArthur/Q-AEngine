using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using inquizitor2.Models;

namespace inquizitor2.Controllers
{
    public class QuestionCommentsController : Controller
    {
        private dbqaEntities db = new dbqaEntities();


        // GET: QuestionComments/Create
        [Authorize]
        public ActionResult Create(int? id)
        {
            ViewBag.QuestionId = id;
            return View();
        }

        // POST: QuestionComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Content")] QuestionComment questionComment, int id)
        {
            if (ModelState.IsValid)
            {
                questionComment.QuestionId = id;
                questionComment.UserName = User.Identity.Name;
                questionComment.User = db.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                questionComment.Question = db.Questions.Where(q => q.QuestionId == id).SingleOrDefault();
                db.QuestionComments.Add(questionComment);
                db.SaveChanges();
                int qid = id;
                return RedirectToAction("Details", "Questions", new { id = qid });
            }

            //ViewBag.QuestionId = new SelectList(db.Questions, "QuestionId", "Title", questionComment.QuestionId);
            //ViewBag.UserName = new SelectList(db.Users, "UserName", "Password", questionComment.UserName);
            return View(questionComment);
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
