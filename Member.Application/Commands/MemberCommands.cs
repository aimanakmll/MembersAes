using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Member.Application.Commands
{
    public class AddMemberCommand : IRequest<Domain.Member>
    {
        public Domain.Member NewMember { get; set; }

        public AddMemberCommand(Domain.Member newMember)
        {
            NewMember = newMember;
        }
    }
}
