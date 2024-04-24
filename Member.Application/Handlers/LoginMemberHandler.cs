using MediatR;
using Member.Application.Commands;
using Member.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Member.Application.Handlers
{
    public class LoginMemberHandler : IRequestHandler<LoginMemberCommand, bool>
    {
        private readonly EncryptServiceBase _encryptService;
        private readonly IMediator _mediator;

        public LoginMemberHandler(EncryptServiceBase encryptService, IMediator mediator)
        {
            _encryptService = encryptService;
            _mediator = mediator;
        }

        public async Task<bool> Handle(LoginMemberCommand request, CancellationToken cancellationToken)
        {
            var getMemberByNameQuery = new GetMemberByNameQuery(request.Name);
           
            var matchingMember = await _mediator.Send(getMemberByNameQuery);

            if (matchingMember == null) return false;

            var decryptedPassword = matchingMember.Password;

            return decryptedPassword == request.Password;
        }
    }
}
