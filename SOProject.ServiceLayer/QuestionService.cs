using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SOProject.DomainModels2;
using SOProject.Repository;
using StackOverflowProject.ServiceLayer;
using StackOverflowProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOProject.ServiceLayer
{
    public interface IQuestionService
    {

        void InsertQuestion(NewQuestionViewModel nqvm);
        void UpdateQuestionDetails(EditQuestionViewModel eqvm);
        void UpdateQuestionVotesCount(int qid, int value);
        void UpdateQuestionAsnwerCount(int qid, int value);
        void UpdateQuestionViewsCount(int qid, int value);
        void DeleteQuestion(int qid);
        List<QuestionViewModel> GetQuestions();
        QuestionViewModel GetQuestionsByQuestionID(int qid, int UserID);

    }

    public class QuestionService : IQuestionService
    {
        IQuestionRepository qr;

        public QuestionService(DbContextOptions<SOProjectDbContext> options)
        {
            qr = new QuestionRepository(options);
        }

        public void DeleteQuestion(int qid)
        {
            qr.DeleteQuestion(qid);
        }

        public List<QuestionViewModel> GetQuestions()
        {
            List<Question> q = qr.GetQuestions();
            var config = new MapperConfiguration(cfg => { 
                cfg.CreateMap<Question, QuestionViewModel>();
                cfg.CreateMap<User, UserViewModel>();
                cfg.CreateMap<Category, CategoryViewModel>();
                cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            List<QuestionViewModel> qvm = mapper.Map<List<Question>, List<QuestionViewModel>>(q);
            return qvm;

        }

        public QuestionViewModel GetQuestionsByQuestionID(int qid, int UserID)
        {
            Question q = qr.GetQuestionsByQuestionID(qid);
            QuestionViewModel qvm = null;
            if (q != null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Question, QuestionViewModel>();
                    cfg.CreateMap<User, UserViewModel>();
                    cfg.CreateMap<Category, CategoryViewModel>();
                    cfg.CreateMap<Answer, AnswerViewModel>();
                    cfg.CreateMap<Vote, VoteViewModel>();
                    cfg.IgnoreUnmapped();
                });
                IMapper mapper = config.CreateMapper();
                qvm = mapper.Map<Question, QuestionViewModel>(q);

                foreach (var item in qvm.Answers)
                {
                    item.CurrentUserVoteType = 0;
                    VoteViewModel vote = item.Votes.Where(temp => temp.UserID == UserID).FirstOrDefault();
                    if (vote != null)
                    {
                        item.CurrentUserVoteType = vote.VoteValue;
                    }
                }
                
            }
            return qvm;
        }
        public void InsertQuestion(NewQuestionViewModel nqvm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<NewQuestionViewModel, Question>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Question q = mapper.Map<NewQuestionViewModel, Question>(nqvm);
            qr.InsertQuestion(q);
        }

        public void UpdateQuestionDetails(EditQuestionViewModel eqvm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<EditQuestionViewModel, Question>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Question q = mapper.Map<EditQuestionViewModel, Question>(eqvm);
            qr.UpdateQuestionDetails(q);
        }

        public void UpdateQuestionAsnwerCount(int qid, int value)
        {
            qr.UpdateQuestionAnswersCount(qid, value);
        }

        public void UpdateQuestionViewsCount(int qid, int value)
        {
            qr.UpdateQuestionViewsCount(qid, value);
        }

        public void UpdateQuestionVotesCount(int qid, int value)
        {
            qr.UpdateQuestionVotesCount(qid, value);
        }
    }
}
