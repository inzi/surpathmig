using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Abp.Auditing;
using Abp.Authorization.Users;
using inzibackend.Surpath;

namespace inzibackend.Surpath.Purchase
{
    public class PreAuthDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public bool IsExternalLogin { get; set; }
        public string ExternalLoginAuthSchema { get; set; }
        public string ReturnUrl { get; set; }
        public string SingleSignIn { get; set; }
        public AuthNetSubmit AuthNetSubmit { get; set; } = new AuthNetSubmit();
        public AuthNetCaptureResultDto AuthNetCaptureResultDto { get; set; } = new AuthNetCaptureResultDto();

        public string Middlename { get; set; }
        public string Address { get; set; }
        public string SuiteApt { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string TransactionId { get; set; }

    }
}
