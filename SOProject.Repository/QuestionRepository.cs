using Microsoft.EntityFrameworkCore;
using SOProject.DomainModels2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOProject.Repository
{
    public interface IQuestionRepository
    {

        void InsertQuestion(Question q);
        void UpdateQuestionDetails(Question q);
        void UpdateQuestionVotesCount(int qid, int value);
        void UpdateQuestionAnswersCount(int qid, int value);
        void UpdateQuestionViewsCount(int qid, int value);
        void DeleteQuestion(int qid);
        List<Question> GetQuestions();
        Question GetQuestionsByQuestionID(int qid);

    }

    public class QuestionRepository : IQuestionRepository
    {
        SOProjectDbContext db;

        public QuestionRepository(DbContextOptions<SOProjectDbContext> options)
        {
            db = new SOProjectDbContext(options);
        }

        public void DeleteQuestion(int qid)
        {
            Question qu = db.Questions.Where(temp => temp.QuestionID == qid).FirstOrDefault();
            if (qu != null)
            {
                db.Questions.Remove(qu);
                db.SaveChanges();
            }
        }

        public List<Question> GetQuestions()
        {
            List<Question> qu = db.Questions
                .Include(temp => temp.Category)
                .Include(temp => temp.User)
                .ToList();
            return qu;
        }

        public Question GetQuestionsByQuestionID(int qid)
        {
            Question qu = db.Questions
                .Where(temp => temp.QuestionID == qid)
                .Include(temp => temp.Answers)
                .ThenInclude(ans => ans.User)
                .ThenInclude(ans => ans.Votes)
                .Include(temp => temp.Category)
                .Include(temp => temp.User)
                .FirstOrDefault();
            return qu;
        }

        public void InsertQuestion(Question q)
        {
            db.Questions.Add(q);
            db.SaveChanges();
        }

        public void UpdateQuestionDetails(Question q)
        {
            Question qu = db.Questions.Where(temp => temp.QuestionID == q.QuestionID).FirstOrDefault();
            if(qu != null)
            {
                qu.QuestionName = q.QuestionName;
                qu.QuestionDateAndTime = q.QuestionDateAndTime;
                qu.CategoryID = q.CategoryID;
                db.SaveChanges();
            }
        }

        public void UpdateQuestionAnswersCount(int qid, int value)
        {
            Question qu = db.Questions.Where(temp => temp.QuestionID == qid).FirstOrDefault();
            if (qu != null)
            {
                qu.AnswerCount += value;
                db.SaveChanges();
            }
        }

        public void UpdateQuestionViewsCount(int qid, int value)
        {
            Question qu = db.Questions.Where(temp => temp.QuestionID == qid).FirstOrDefault();
            if (qu != null)
            {
                qu.ViewsCount = value;
                db.SaveChanges();
            }
        }

        public void UpdateQuestionVotesCount(int qid, int value)
        {
            Question qu = db.Questions.Where(temp => temp.QuestionID == qid).FirstOrDefault();
            if (qu != null)
            {
                qu.VotesCount += value;
                db.SaveChanges();
            }
        }
    }
}
