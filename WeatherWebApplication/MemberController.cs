using System;
using System.Collections.Generic;
using MediatR;
using Member.Application;
using Member.Application.Commands;
using Member.Application.Queries;
using Member.Application.Services;
using Member.Domain;
using Member.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Member.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly MemberServiceBase _memberService;
        private readonly EncryptServiceBase _encryptRepository;
        private readonly MemberRepositoryBase _memberRepository;
        private readonly RSAEncryptService _rSAEncryptService;
        private readonly IMediator _mediator;

        //public MembersController(MemberServiceBase memberService)
        //{
        //    _memberService = memberService;

        //}
        public MembersController(MemberServiceBase memberService, EncryptServiceBase encryptService, MemberRepositoryBase memberRepository, RSAEncryptService rSAEncyptService, IMediator  mediator)
        {
            _memberService = memberService;
            _encryptRepository = encryptService;
            _memberRepository = memberRepository;
            _rSAEncryptService = rSAEncyptService;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Domain.Member>>> Get([FromServices] IMediator mediator)
        {
            try
            {
                var query = new GetAllMembersQuery();
                var members = await mediator.Send(query);

                return Ok(members);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        //[HttpPost("Insert Data")]
        //public ActionResult<Domain.Member> Post([FromBody] MemberRequestModel inputModel)
        //{
        //    try
        //    {
        //        Domain.Member member = new Domain.Member
        //        {
        //            Name = inputModel.Name,
        //            Password = inputModel.Password
        //        };

        //        Domain.Member addedMember = _memberService.AddMember(member);

        //        // Don't return the password
        //        addedMember.Password = null;

        //        return CreatedAtAction("Get", new { id = addedMember.Id }, addedMember);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}


        [HttpPost("Insert Data")]
        public ActionResult<Domain.Member> Post([FromBody] MemberRequestModel inputModel)
        {
            try
            {
                Domain.Member member = new Domain.Member
                {
                    Name = inputModel.Name,
                    Password = inputModel.Password
                };

                Domain.Member addedMember = _memberService.AddMember(member);

                // Don't return the password
                addedMember.Password = null;

                return CreatedAtAction("Get", new { id = addedMember.Id }, addedMember);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login using AES")]
        public ActionResult Login([FromBody] MemberLoginRequestModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.Name) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest("Username and password must be provided.");
            }

            try
            {
                var encryptService = _encryptRepository;

                // Fetch the member by name
                Domain.Member matchingMember = _memberService.GetMemberByName(loginModel.Name);

                if (matchingMember == null)
                {
                    return Unauthorized("Invalid username or password.");
                }

                string decryptedPassword;
                try
                {
                    decryptedPassword = matchingMember.Password;
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error decrypting the password. Original encrypted password: {matchingMember.Password}");
                }

                if (decryptedPassword == loginModel.Password)
                {
                        var members = _memberService.GetAllMembers();

                        // Decrypt passwords before returning
                        foreach (var member in members)
                        {
                            member.Password = (member.Password);
                        }

                        return Ok(new { message = "Login successful via aes : ", members });
                       
                }
                else
                {
                    return Unauthorized("Invalid username or password.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




        [HttpPost("encrypt")]
        public ActionResult EncryptPassword([FromBody] EncryptionRequestModel encryptionRequest)
        {
            try
            {
                string encryptedPassword;
                string decryptedPassword;

                if (encryptionRequest.EncryptionType.Equals("AES", StringComparison.OrdinalIgnoreCase))
                {
                    encryptedPassword = _encryptRepository.EncryptPassword(encryptionRequest.Password);
                    decryptedPassword = _encryptRepository.DecryptPassword(encryptedPassword);
                }
                else if (encryptionRequest.EncryptionType.Equals("RSA", StringComparison.OrdinalIgnoreCase))
                {
                    encryptedPassword = _rSAEncryptService.Encrypt(encryptionRequest.Password);
                    decryptedPassword = _rSAEncryptService.Decrypt(encryptedPassword);
                }
                else
                {
                    return BadRequest("Invalid encryption type. Choose 'AES' or 'RSA'.");
                }

                return Ok(new
                {
                    Name = encryptionRequest.Name,
                    OriginalPassword = encryptionRequest.Password,
                    EncryptionType = encryptionRequest.EncryptionType,
                    EncryptedPassword = encryptedPassword,
                    DecryptedPassword = decryptedPassword
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error encrypting/decrypting password: {ex.Message}");
            }
        }


        //HTTP POST to Insert the data
        public class MemberRequestModel
        {
            public string Name { get; set; }
            public string Password { get; set; }
        }

        //HTTP POST to login using the user's details and display all of the information inside the Members table
        public class MemberLoginRequestModel
        {
            public string Name { get; set; }
            public string Password { get; set; }
        }

        public class EncryptionRequestModel
        {
            public string Name { get; set; }
            public string Password { get; set; }
            public string EncryptionType { get; set; }
        }
    }
}
