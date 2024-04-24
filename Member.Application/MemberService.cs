using System.Collections.Generic;

namespace Member.Application
{
    //Implement Bussiness Rule / USE CASES
    public class MemberService : MemberServiceBase
    {
        private readonly MemberRepositoryBase memberRepository;
        public MemberService(MemberRepositoryBase memberRepository)
        {
            this.memberRepository = memberRepository;
        }

        public override List<Domain.Member> GetAllMembers()
        {
            return this.memberRepository.GetAllMembers();
        }

        public override Domain.Member AddMember(Domain.Member member)
        {
            return this.memberRepository.AddMember(member);
        }

        public override Domain.Member GetMemberByName(string name)
        {
            return this.memberRepository.GetMemberByName(name);
        }
    }
}
