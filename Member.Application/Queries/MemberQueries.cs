// Queries/MemberQueries.cs
using MediatR;
using System.Collections.Generic;
using Member.Domain;
using Member.Application;

namespace Member.Application.Queries 
{
    public class GetAllMembersQuery : IRequest<List<Domain.Member>> { }

    public class GetMemberByNameQuery : IRequest<Domain.Member>
    {
        public string Name { get; set; }

        public GetMemberByNameQuery(string name)
        {
            Name = name;
        }
    }
}
