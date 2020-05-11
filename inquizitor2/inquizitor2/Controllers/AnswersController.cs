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
    public class AnswersController : Controller
    {
        private dbqaEntities db = new dbqaEntities();

        // GET: Answers/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            ViewBag.QuestionId = id;
            return View();
        }

        // POST: Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Content")] Answer answer, int id)
        {
            if (ModelState.IsValid)
            {
                answer.QuestionId = id;
                answer.UserName = User.Identity.Name;
                answer.User = db.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                answer.Question = db.Questions.Where(q => q.QuestionId == id).SingleOrDefault();
                db.Answers.Add(answer);
                db.SaveChanges();
                int qid = id;
                return RedirectToAction("Details", "Questions", new { id = qid });
            }

            return View(answer);
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
