using AutoMapper.Execution;
using Hawi.Models;

namespace Hawi.Dtos
{
    public class ViewUser
    {
        public IEnumerable<User> users { get; set; }
        public ViewUser() 
        {
            users = new List<User>();
            users.Select(x => new 
            {
                x.UserId,x.Mobile,x.Name
            });
        }
        //public IEnumerable<UserDetails> userdetails { get; set; }

    }
}
