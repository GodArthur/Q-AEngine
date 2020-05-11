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

        // GET: QuestionComments
        public ActionResult Index()
        {
            var questionComments = db.QuestionComments.Include(q => q.Question).Include(q => q.User);
            return View(questionComments.ToList());
        }

        // GET: QuestionComments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionComment questionComment = db.QuestionComments.Find(id);
            if (questionComment == null)
            {
                return HttpNotFound();
            }
            return View(questionComment);
        }


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

        // GET: QuestionComments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionComment questionComment = db.QuestionComments.Find(id);
            if (questionComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuestionId = new SelectList(db.Questions, "QuestionId", "Title", questionComment.QuestionId);
            ViewBag.UserName = new SelectList(db.Users, "UserName", "Password", questionComment.UserName);
            return View(questionComment);
        }

        // POST: QuestionComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QCommentId,QuestionId,Content,UserName,Date")] QuestionComment questionComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(questionComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.QuestionId = new SelectList(db.Questions, "QuestionId", "Title", questionComment.QuestionId);
            ViewBag.UserName = new SelectList(db.Users, "UserName", "Password", questionComment.UserName);
            return View(questionComment);
        }

        // GET: QuestionComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionComment questionComment = db.QuestionComments.Find(id);
            if (questionComment == null)
            {
                return HttpNotFound();
            }
            return View(questionComment);
        }

        // POST: QuestionComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QuestionComment questionComment = db.QuestionComments.Find(id);
            db.QuestionComments.Remove(questionComment);
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
