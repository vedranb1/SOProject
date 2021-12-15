using System.Web.Http;
using SOProject.ServiceLayer;

namespace StackOverflowProject.ApiControllers
{
    public class AccountController : ApiController
    {

        IUserService us;

        public AccountController(IUserService us)
        {
            this.us = us;
        }

        public string Get(string Email)
        {
            if (this.us.GetUsersByEmail(Email) != null)
            {
                return "Found";
            }
            else
            {
                return "Not found";
            }
        }

    }
}
