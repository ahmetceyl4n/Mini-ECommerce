using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.AppUser.Commands.CreateUser
{
    public class CreateUserCommandResponse
    {
        public bool Succeeded { get; internal set; }
        public string Message { get; internal set; }
    }
}
