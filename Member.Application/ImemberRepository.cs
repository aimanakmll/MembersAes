using Member.Domain;

namespace Member.Application
{
    public interface IMemberRepository
    {
        List<Domain.Member> GetAllMembers();
        Domain.Member AddMember(Domain.Member member);
    }

    public interface IEncryptRepository
    {
        void AddEncrypt(string encryptedPassword);
    }
}
