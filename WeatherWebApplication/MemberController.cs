using System;
using System.Collections.Generic;
using Member.Application;
using Member.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Member.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IEncryptRepository _encryptRepository;
        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Domain.Member>> Get()
        {
            try
            {
                var members = _memberService.GetAllMembers();

                // Decrypt passwords before returning
                foreach (var member in members)
                {
                    member.Password = (member.Password);
                }

                return Ok(members);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
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

        public class MemberRequestModel
        {
            public string Name { get; set; }
            public string Password { get; set; }
        }
    }
}
