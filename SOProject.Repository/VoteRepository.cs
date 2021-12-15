using Microsoft.EntityFrameworkCore;
using SOProject.DomainModels2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOProject.Repository
{
    public interface IVoteRepository
    {

        void UpdateVote(int aid, int uid, int value);

    }

    public class VoteRepository : IVoteRepository
    {
        SOProjectDbContext db;

        public VoteRepository(DbContextOptions<SOProjectDbContext> options)
        {
            db = new SOProjectDbContext(options);
        }

        public void UpdateVote(int aid, int uid, int value)
        {
            int updateValue;
            if(value > 0)
            {
                updateValue = 1;
            }
            else if(value < 0)
            {
                updateValue = -1;
            }
            else
            {
                updateValue = 0;
            }

            Vote v = db.Votes.Where(temp => temp.AnswerID == aid && temp.UserID == uid).FirstOrDefault();
            if(v != null)
            {
                v.VoteValue = updateValue;
            }
            else
            {
                Vote newVote = new Vote()
                {
                    AnswerID = aid,
                    UserID = uid,
                    VoteValue = updateValue
                };
                db.Votes.Add(newVote);
            }
            db.SaveChanges();
        }
    }
}
