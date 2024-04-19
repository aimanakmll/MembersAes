using System.Collections.Generic;

namespace Member.Application
{
    //Implement Bussiness Rule / USE CASES
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository memberRepository;
        public MemberService(IMemberRepository memberRepository)
        {
            this.memberRepository = memberRepository;
        }
        List<Domain.Member> IMemberService.GetAllMembers()
        {
            return this.memberRepository.GetAllMembers();
        }

        public Domain.Member AddMember(Domain.Member member)
        {
            return this.memberRepository.AddMember(member);
        }
    }
}
