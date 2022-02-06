namespace LabManAPI.Contracts
{
    public static class ApiRoutes
    {
        public const string Base = "api";

        public static class Item
        {
            public const string GetAll = Base + "/" + "items";

            public const string Create = Base + "/" + "item";

            public const string Get = Base + "/" + "items/{itemId}";

            public const string Update = Base + "/" + "items/{itemId}";

            public const string Delete = Base + "/" + "items/{itemId}";
        }

        public static class Identity
        {
            public const string Login = Base + "/identity/login";

            public const string Register = Base + "/identity/register";

            public const string GetAll = Base + "/identity/users";

            public const string ChangePassword = Base + "/identity/user/password";

            public const string ChangeEmail = Base + "/identity/user/email";

            public const string ConfirmChangeEmail = Base + "/identity/user/email/confirm";
        }

        public static class Reservation
        {
            public const string GetAll = Base + "/" + "reservations";

            public const string Create = Base + "/" + "reservation";

            public const string Get = Base + "/" + "reservations/{reservationId}";

            public const string Update = Base + "/" + "reservations/{reservationId}";

            public const string Delete = Base + "/" + "reservations/{reservationId}";

            public const string GetFromDate = Base + "/" + "reservations/from/date";

            public const string GetFromUser = Base + "/" + "reservations/from/identity";
        }

        public static class Fault
        {
            public const string GetAll = Base + "/" + "faults";

            public const string Create = Base + "/" + "fault";

            public const string Get = Base + "/" + "faults/{faultId}";

            public const string Update = Base + "/" + "faults/{faultId}";

            public const string Delete = Base + "/" + "faults/{faultId}";

            public const string GetFromUser = Base + "/" + "faults/from/identity";

            public const string GetFromItem = Base + "/" + "faults/from/item";

        }
    }
}