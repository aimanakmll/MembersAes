using System;
using MediatR;
using Member.Domain;

namespace Member.Application.Command
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
