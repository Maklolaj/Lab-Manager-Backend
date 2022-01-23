using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LabManAPI.Contracts.Requests
{
    public class UserUpdateProfileInfo
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        public string RepeatedPassword { get; set; }

    }
}

