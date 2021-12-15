using Microsoft.EntityFrameworkCore;
using SOProject.DomainModels2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOProject.Repository
{
    public interface IAnswerRepository
    {

        void InsertAnswer(Answer a);
        void UpdateAnswerDetails(Answer a);
        void UpdateAnswerVotesCount(int aid, int uid, int value);
        void DeleteAnswer(int aid);
        Answer GetAnswersByQuestionID(int qid);
        Answer GetAnswersByAnswerID(int aid);

    }

    public class AnswerRepository : IAnswerRepository
    {
        SOProjectDbContext db;
        QuestionRepository qr;
        IVoteRepository vr;

        public AnswerRepository(DbContextOptions<SOProjectDbContext> options)
        {
            db = new SOProjectDbContext(options);
            qr = new QuestionRepository(options);
            vr = new VoteRepository(options);
        }

        public void DeleteAnswer(int aid)
        {
            Answer an = db.Answers.Where(temp => temp.AnswerID == aid).FirstOrDefault();
            if (an != null)
            {
                db.Answers.Remove(an);
                db.SaveChanges();
            }
        }

        public Answer GetAnswersByAnswerID(int aid)
        {
            Answer an = db.Answers.Where(temp => temp.AnswerID == aid).FirstOrDefault();
            return an;
        }

        public Answer GetAnswersByQuestionID(int qid)
        {
            Answer an = db.Answers.Where(temp => temp.QuestionID == qid).FirstOrDefault();
            return an;
        }

        public void InsertAnswer(Answer a)
        {
            db.Answers.Add(a);
            db.SaveChanges();
        }

        public void UpdateAnswerDetails(Answer a)
        {
            Answer an = db.Answers.Where(temp => temp.AnswerID == a.AnswerID).Include(temp => temp.Question).FirstOrDefault();
            if(an != null)
            {
                an.AnswerText = a.AnswerText;
                db.SaveChanges();
            }
        }

        public void UpdateAnswerVotesCount(int aid, int uid, int value)
        {
            Answer an = db.Answers.Where(temp => temp.AnswerID == aid).FirstOrDefault();
            if (an != null)
            {
                an.VotesCount += value;
                db.SaveChanges();
                qr.UpdateQuestionVotesCount(an.QuestionID, value);
                vr.UpdateVote(aid, uid, value);
            }
        }
    }
}
