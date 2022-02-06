namespace LabManAPI.Contracts.Requests
{
    public class UserChangePasswordInfo
    {
        public string CurrentPassword { get; set; }

        public string Password { get; set; }

        public string RepeatedPassword { get; set; }

    }
}
