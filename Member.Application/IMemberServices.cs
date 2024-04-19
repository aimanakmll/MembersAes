using System.Collections.Generic;

namespace Member.Application
{
    //This interface is use for Bussiness Rule / USE CASE
    public interface IMemberService
    {
        List<Domain.Member> GetAllMembers();

        Domain.Member AddMember(Domain.Member member);
    }
}
