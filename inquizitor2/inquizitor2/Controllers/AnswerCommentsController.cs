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
    public class AnswerCommentsController : Controller
    {
        private dbqaEntities db = new dbqaEntities();


        // GET: AnswerComments/Create
        [Authorize]
        public ActionResult Create(int? id)
        {
            ViewBag.AnswerId = id;
            ViewBag.QuestionId = (from q in db.Questions
                                  from a in q.Answers
                                  where a.AnswerId == id
                                  select q).SingleOrDefault().QuestionId;
            return View();
        }

        // POST: AnswerComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ACommentId,AnswerId,Content,UserName,Date")] AnswerComment answerComment, int id, int qid)
        {
            if (ModelState.IsValid)
            {

                answerComment.AnswerId = id;
                answerComment.UserName = User.Identity.Name;
                answerComment.User = db.Users.Where(user => user.UserName == User.Identity.Name).SingleOrDefault();
                answerComment.Answer = db.Answers.Where(a => a.AnswerId == id).SingleOrDefault();
                db.AnswerComments.Add(answerComment);
                db.SaveChanges();

                return RedirectToAction("Details", "Questions", new { id = qid });
            }

            return View(answerComment);
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
