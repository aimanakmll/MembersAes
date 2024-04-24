namespace Member.Application
{
    // Abstract class to define repository behaviors
    public abstract class MemberRepositoryBase
    {
        public abstract List<Domain.Member> GetAllMembers(); // Signature 1
        public abstract Domain.Member AddMember(Domain.Member member); // Signature 2
        public abstract Domain.Member GetMemberByName(string name);
    } 
}
