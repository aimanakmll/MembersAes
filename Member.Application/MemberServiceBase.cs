using System.Collections.Generic;

namespace Member.Application
{
    //This interface is use for Bussiness Rule / USE CASE
    public abstract class MemberServiceBase
    {
        public abstract List<Domain.Member> GetAllMembers();
        public abstract Domain.Member AddMember(Domain.Member member);
        public abstract Domain.Member GetMemberByName(string name);

    }
}
