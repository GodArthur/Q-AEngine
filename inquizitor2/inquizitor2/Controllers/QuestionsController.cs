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
    public class QuestionsController : Controller
    {
        private dbqaEntities db = new dbqaEntities();

        // GET: Questions
        public ActionResult Index()
        {
            var questions = db.Questions.OrderByDescending(q => q.Upvotes).Include(q => q.User);
            questions.ToList();
            //sortUp(questions);
            return View("Index", questions);
        }



        [Authorize]
        public ActionResult VoteQuestion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Question question = db.Questions.Find(id);

            if (question == null)
            {
                return HttpNotFound();
            }

            question.Upvotes += 1;
            db.SaveChanges();

            return Index();
        }


        // GET: Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.question = question;
            var qComments = db.QuestionComments.Where(comment => comment.QuestionId == id).Include(comment => comment.User)?.ToList();
            var answers = db.Answers.Where(a => a.QuestionId == id).OrderByDescending(a => a.Upvotes).Include(a => a.User).Include(a => a.AnswerComments)?.ToList();

            if (answers.Count < 1)
            {
                ViewBag.Answers = null;
            }
            else
            {
               // SortUp(answers);
                ViewBag.Answers = answers;
            }

            if (qComments.Count < 1)
            {
                ViewBag.QComments = null;
            }
            else
            {
                ViewBag.QComments = qComments;
            }

            
            //return View();
            return View("Details", question);
        }


     

        // GET: Questions/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.UserName = new SelectList(db.Users, "UserName", "Password");
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Content")] Question question)
        {
            if (ModelState.IsValid)
            {
                question.UserName = User.Identity.Name;
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.UserName = new SelectList(db.Users, "UserName", "Password", question.UserName);
            return View(question);
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserName = new SelectList(db.Users, "UserName", "Password", question.UserName);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuestionId,Title,Content,UserName,Date,Views,Upvotes")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserName = new SelectList(db.Users, "UserName", "Password", question.UserName);
            return View(question);
        }

        // GET: Questions/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
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
