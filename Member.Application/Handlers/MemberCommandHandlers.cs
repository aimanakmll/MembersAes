using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Member.Domain;
using Member.Application.Commands;

namespace Member.Application.Handlers
{
    public class AddMemberCommandHandler : IRequestHandler<AddMemberCommand, Domain.Member>
    {
        private readonly MemberServiceBase _memberService;

        public AddMemberCommandHandler(MemberServiceBase memberService)
        {
            _memberService = memberService;
        }

        public async Task<Domain.Member> Handle(AddMemberCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_memberService.AddMember(request.NewMember));
        }
    }
}
