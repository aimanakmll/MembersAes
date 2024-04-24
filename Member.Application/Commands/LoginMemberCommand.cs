using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Member.Application.Commands
{
    public class LoginMemberCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
