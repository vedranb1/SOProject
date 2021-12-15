using System.Web.Http;
using SOProject.ServiceLayer;

namespace StackOverflowProject.ApiControllers
{
    public class QuestionsController : ApiController
    {
        IAnswerService asr;

        public QuestionsController(IAnswerService asr)
        {
            this.asr = asr;
        }

        // GET: Questions
        public void Post(int AnswerID, int UserID, int value)
        {
            this.asr.UpdateAnswerVotesCount(AnswerID, UserID, value);
        }
    }
}