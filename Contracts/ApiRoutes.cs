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

        }

        public static class Reservation
        {
            public const string GetAll = Base + "/" + "reservations";

            public const string Create = Base + "/" + "reservation";

            public const string Get = Base + "/" + "reservations/{reservationId}";

            public const string Update = Base + "/" + "reservations/{reservationId}";

            public const string Delete = Base + "/" + "reservations/{reservationId}";
        }
    }
}