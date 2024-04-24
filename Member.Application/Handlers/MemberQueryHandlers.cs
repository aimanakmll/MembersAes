using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Member.Application;
using Member.Domain;
using Member.Application.Queries;
namespace Member.Application.Handlers
{
    public class GetAllMembersQueryHandler : IRequestHandler<GetAllMembersQuery, List<Domain.Member>>
    {
        private readonly MemberServiceBase _memberService;

        public GetAllMembersQueryHandler(MemberServiceBase memberService)
        {
            _memberService = memberService;
        }

        public async Task<List<Domain.Member>> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_memberService.GetAllMembers());
        }
    }

    public class GetMemberByNameQueryHandler : IRequestHandler<GetMemberByNameQuery, Domain.Member>
    {
        private readonly MemberServiceBase _memberService;

        public GetMemberByNameQueryHandler(MemberServiceBase memberService)
        {
            _memberService = memberService;
        }

        public async Task<Domain.Member> Handle(GetMemberByNameQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_memberService.GetMemberByName(request.Name));
        }
    }

}
