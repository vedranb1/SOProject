using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SOProject.DomainModels2;
using SOProject.Repository;
using StackOverflowProject.ServiceLayer;
using StackOverflowProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SOProject.ServiceLayer
{
    public interface IAnswerService
    {
        void InsertAnswer(NewAnswerViewModel navm);
        void UpdateAnswerDetails(EditAnswerViewModel eavm);
        void UpdateAnswerVotesCount(int aid, int uid, int value);
        void DeleteAnswer(int aid);
        AnswerViewModel GetAnswersByQuestionID(int qid);
        AnswerViewModel GetAnswersByAnswerID(int aid);

    }

    public class AnswerService : IAnswerService
    {
        IAnswerRepository ar;
        public AnswerService(DbContextOptions<SOProjectDbContext> options)
        {
            ar = new AnswerRepository(options);
        }

        public void DeleteAnswer(int aid)
        {
            ar.DeleteAnswer(aid);
        }

        public AnswerViewModel GetAnswersByAnswerID(int aid)
        {
            Answer a = ar.GetAnswersByAnswerID(aid);
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<Answer, AnswerViewModel>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            AnswerViewModel avm = mapper.Map<Answer, AnswerViewModel>(a);
            return avm;
        }

        public AnswerViewModel GetAnswersByQuestionID(int qid)
        {
            Answer a = ar.GetAnswersByQuestionID(qid);
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<Answer, AnswerViewModel>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            AnswerViewModel avm = mapper.Map<Answer, AnswerViewModel>(a);
            return avm;
        }

        public void InsertAnswer(NewAnswerViewModel navm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<NewAnswerViewModel, Answer>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Answer a = mapper.Map<NewAnswerViewModel, Answer>(navm);
            ar.InsertAnswer(a);
        }

        public void UpdateAnswerDetails(EditAnswerViewModel eavm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<EditAnswerViewModel, Answer>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Answer a = mapper.Map<EditAnswerViewModel, Answer>(eavm);
            ar.UpdateAnswerDetails(a);
        }

        public void UpdateAnswerVotesCount(int aid, int uid, int value)
        {
            ar.UpdateAnswerVotesCount(aid, uid, value);
        }
    }
}
