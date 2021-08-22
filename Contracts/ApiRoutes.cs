namespace LabManAPI.Contracts
{
    public static class ApiRoutes
    {
        public const string Base = "api";

        public static class Item
        {
            public const string GetAll = Base + "/" + "items";

            public const string Create = Base + "/" + "items";

            public const string Get = Base + "/" + "items/{itemId}";

            public const string Update = Base + "/" + "items/{itemId}";

            public const string Delete = Base + "/" + "items/{itemId}";
        }

         public static class Identity
        {
            public const string Login = Base + "/identity/login";

            public const string Register = Base + "/identity/register";

        }
    }
}