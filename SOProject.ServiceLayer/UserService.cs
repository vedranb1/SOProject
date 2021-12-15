using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SOProject.DomainModels2;
using SOProject.Repository;
using StackOverflowProject.ServiceLayer;
using StackOverflowProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SOProject.ServiceLayer
{
    public interface IUserService
    {

        int InsertUser(RegisterViewModel uvm);
        void UpdateUserDetails(EditUserDetailsViewModel eudvm);
        void UpdateUserPassword(EditUserPasswordViewModel eupvm);
        void DeleteUser(int uid);
        List<UserViewModel> GetUsers();
        UserViewModel GetUsersByEmail(string Email);
        UserViewModel GetUsersByEmailAndPassword(string Email, string Password);
        UserViewModel GetUsersByUserID(int UserID);

    }

    public class UserService : IUserService
    {
        IUserRepository ur;

        public UserService(DbContextOptions<SOProjectDbContext> options)
        {
            ur = new UserRepository(options);
        }

        public void DeleteUser(int uid)
        {
            ur.DeleteUser(uid);
        }

        public List<UserViewModel> GetUsers()
        {
            List<User> u = ur.GetUsers();
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<User, UserViewModel>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            List<UserViewModel> uvm = mapper.Map<List<User>, List<UserViewModel>>(u);
            return uvm;

        }

        public UserViewModel GetUsersByEmail(string Email)
        {
            User u = ur.GetUsersByEmail(Email).FirstOrDefault();
            UserViewModel uvm = null;
            if (u != null)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<User, UserViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                uvm = mapper.Map<User, UserViewModel>(u);
                
            }
            return uvm;
        }

        public UserViewModel GetUsersByEmailAndPassword(string Email, string Password)
        {
            User u = ur.GetUsersByEmailAndPassword(Email, SHA256HashGenerator.GenerateHash(Password)).FirstOrDefault();
            UserViewModel uvm = null;
            if (u != null)
            {
                var config = new MapperConfiguration(cfg => 
                {
                    cfg.CreateMap<User, UserViewModel>()
                        .ForMember(destination => destination.Password, cfg => cfg.MapFrom(src => src.PasswordHash));
                    cfg.IgnoreUnmapped(); 
                });
                IMapper mapper = config.CreateMapper();
                uvm = mapper.Map<User, UserViewModel>(u);

            }
            return uvm;
        }

        public UserViewModel GetUsersByUserID(int UserID)
        {
            User u = ur.GetUsersByUserID(UserID).FirstOrDefault();
            UserViewModel uvm = null;
            if (u != null)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<User, UserViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                uvm = mapper.Map<User, UserViewModel>(u);

            }
            return uvm;
        }

        public int InsertUser(RegisterViewModel uvm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<RegisterViewModel, User>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            User u = mapper.Map<RegisterViewModel, User>(uvm);
            u.PasswordHash = SHA256HashGenerator.GenerateHash(uvm.Password);
            ur.InsertUser(u);
            int uid = ur.GetLatestUserID();
            return uid;
        }

        public void UpdateUserDetails(EditUserDetailsViewModel eudvm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<EditUserDetailsViewModel, User>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            User u = mapper.Map<EditUserDetailsViewModel, User>(eudvm);
            ur.UpdateUserDetails(u);
        }

        public void UpdateUserPassword(EditUserPasswordViewModel eupvm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<EditUserPasswordViewModel, User>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            User u = mapper.Map<EditUserPasswordViewModel, User>(eupvm);
            u.PasswordHash = SHA256HashGenerator.GenerateHash(eupvm.Password);
            ur.UpdateUserPassword(u);
        }
    }
}
