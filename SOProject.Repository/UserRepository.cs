using Microsoft.EntityFrameworkCore;
using SOProject.DomainModels2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SOProject.Repository
{
    public interface IUserRepository
    {
        void InsertUser(User u);
        void UpdateUserDetails(User u);
        void UpdateUserPassword(User u);
        void DeleteUser(int uid);
        List<User> GetUsers();
        List<User> GetUsersByEmail(string Email);
        List<User> GetUsersByEmailAndPassword(string Email, string Password);
        List<User> GetUsersByUserID(int uid);
        int GetLatestUserID();

    }

    public class UserRepository : IUserRepository
    {
        SOProjectDbContext db;

        public UserRepository(DbContextOptions<SOProjectDbContext> options)
        {
            db = new SOProjectDbContext(options);
        }


        public void DeleteUser(int uid)
        {
            User us = db.Users.Where(temp => temp.UserID == uid).FirstOrDefault();
            if(us != null)
            {
                db.Users.Remove(us);
                db.SaveChanges();
            }
            
        }

        public int GetLatestUserID()
        {
            int uid = db.Users.Select(temp => temp.UserID).Max();
            return uid;
        }

        public List<User> GetUsers()
        {
            List<User> us = db.Users.ToList();
            return us;
        }

        public List<User> GetUsersByEmail(string Email)
        {
            List<User> us = db.Users.Where(temp => temp.Email == Email).ToList();
            return us;
        }

        public List<User> GetUsersByEmailAndPassword(string Email, string Password)
        {
            List<User> us = db.Users.Where(temp => temp.Email == Email && temp.PasswordHash == Password).ToList();
            return us;
        }

        public List<User> GetUsersByUserID(int uid)
        {
            List<User> us = db.Users.Where(temp => temp.UserID == uid).ToList();
            return us;
        }

        public void InsertUser(User u)
        {
            db.Users.Add(u);
            db.SaveChanges();
        }

        public void UpdateUserDetails(User u)
        {
            User us = db.Users.Where(temp => temp.UserID == u.UserID).FirstOrDefault();
            if(us != null)
            {
                us.Username = u.Username;
                us.Mobile = u.Mobile;
                us.Email = u.Email;
                db.SaveChanges();
            }
        }

        public void UpdateUserPassword(User u)
        {
            User us = db.Users.Where(temp => temp.UserID == u.UserID).FirstOrDefault();
            if (us != null)
            {
                us.PasswordHash = u.PasswordHash;
                db.SaveChanges();
            }
        }
    }
}
