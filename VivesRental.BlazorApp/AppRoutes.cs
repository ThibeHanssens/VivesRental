namespace VivesRental.BlazorApp
{
    public static class AppRoutes
    {
        public static class Home
        {
            public const string Index = "/";
        }

        public static class Account
        {
            public const string SignIn = "/account/sign-in";
        }

        public static class Dashboard
        {
            public const string Index = "/dashboard";
        }

        public static class Products
        {
            private const string Base = "/products";

            public const string Index = $"{Base}";
            public const string Create = $"{Base}/create";
            public const string Edit = $"{Base}/edit/{{id:guid}}";

            public static string EditUrl(Guid id)
            {
                return Edit.Replace("{id:guid}", id.ToString());
            }
        }

        public static class Articles
        {
            private const string Base = "/articles";

            public const string Index = $"{Base}";
            public const string Create = $"{Base}/create";
            public const string Edit = $"{Base}/edit/{{id:guid}}";

            public static string EditUrl(Guid id)
            {
                return Edit.Replace("{id:guid}", id.ToString());
            }
        }

        public static class Customers
        {
            private const string Base = "/customers";

            public const string Index = $"{Base}";
            public const string Create = $"{Base}/create";
            public const string Edit = $"{Base}/edit/{{id:guid}}";

            public static string EditUrl(Guid id)
            {
                return Edit.Replace("{id:guid}", id.ToString());
            }
        }

        public static class Orders
        {
            private const string Base = "/orders";

            public const string Index = $"{Base}";
            public const string Details = $"{Base}/details/{{id:guid}}";
            public const string Return = $"{Base}/return/{{id:guid}}";

            public static string DetailsUrl(Guid id)
            {
                return Details.Replace("{id:guid}", id.ToString());
            }

            public static string ReturnUrl(Guid id)
            {
                return Return.Replace("{id:guid}", id.ToString());
            }
        }

        public static class OrderLines
        {
            private const string Base = "/orderlines";

            public const string Rent = $"{Base}/rent";
            public const string Return = $"{Base}/return/{{id:guid}}";

            public static string ReturnUrl(Guid id)
            {
                return Return.Replace("{id:guid}", id.ToString());
            }
        }

        public static class Reservations
        {
            private const string Base = "/reservations";

            public const string Index = $"{Base}";
            public const string Create = $"{Base}/create";
            public const string Details = $"{Base}/details/{{id:guid}}";

            public static string DetailsUrl(Guid id)
            {
                return Details.Replace("{id:guid}", id.ToString());
            }
        }
    }
}
