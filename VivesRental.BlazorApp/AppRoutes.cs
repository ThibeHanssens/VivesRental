namespace VivesRental.BlazorApp
{
    public static class AppRoutes
    {
        public static class Home
        {
            public const string Index = "/"; // Homepagina van de applicatie
        }

        public static class Account
        {
            public const string SignIn = "/account/sign-in"; // Inlogpagina voor medewerkers
        }

        public static class Dashboard
        {
            public const string Index = "/dashboard"; // Overzichtspagina (Springboard)
        }

        public static class Products
        {
            private const string Base = "/products";

            public const string Index = $"{Base}"; // Productenoverzicht
            public const string Create = $"{Base}/create"; // Nieuwe producten aanmaken
            public const string Edit = $"{Base}/edit/{{id:guid}}"; // Bestaande producten bewerken

            // Dynamisch gegenereerde URL voor bewerken
            public static string EditUrl(Guid id) => Edit.Replace("{id:guid}", id.ToString());
        }

        public static class Articles
        {
            private const string Base = "/articles";

            public const string Index = $"{Base}"; // Artikelenoverzicht
            public const string Create = $"{Base}/create"; // Nieuwe artikelen aanmaken
            public const string Edit = $"{Base}/edit/{{id:guid}}"; // Artikelen bewerken

            // Dynamisch gegenereerde URL voor bewerken
            public static string EditUrl(Guid id) => Edit.Replace("{id:guid}", id.ToString());
        }

        public static class Customers
        {
            private const string Base = "/customers";

            public const string Index = $"{Base}"; // Klantenoverzicht
            public const string Create = $"{Base}/create"; // Nieuwe klanten aanmaken
            public const string Edit = $"{Base}/edit/{{id:guid}}"; // Klanteninformatie bewerken

            // Dynamisch gegenereerde URL voor bewerken
            public static string EditUrl(Guid id) => Edit.Replace("{id:guid}", id.ToString());
        }

        public static class Orders
        {
            private const string Base = "/orders";

            public const string Index = $"{Base}"; // Bestellingenoverzicht
            public const string Details = $"{Base}/details/{{id:guid}}"; // Besteldetails bekijken
            public const string Return = $"{Base}/return/{{id:guid}}"; // Bestellingen terugbrengen

            // Dynamische URL's voor details en retourneren
            public static string DetailsUrl(Guid id) => Details.Replace("{id:guid}", id.ToString());
            public static string ReturnUrl(Guid id) => Return.Replace("{id:guid}", id.ToString());
        }

        public static class OrderLines
        {
            private const string Base = "/orderlines";

            public const string Rent = $"{Base}/rent"; // Artikelen verhuren via orderlijnen
            public const string Return = $"{Base}/return/{{id:guid}}"; // Artikelen retourneren

            // Dynamische URL voor retourneren
            public static string ReturnUrl(Guid id) => Return.Replace("{id:guid}", id.ToString());
        }

        public static class Reservations
        {
            private const string Base = "/reservations";

            public const string Index = $"{Base}"; // Overzicht van reserveringen
            public const string Create = $"{Base}/create"; // Nieuwe reserveringen aanmaken
            public const string Details = $"{Base}/details/{{id:guid}}"; // Details van een reservering bekijken

            // Dynamische URL voor details bekijken
            public static string DetailsUrl(Guid id) => Details.Replace("{id:guid}", id.ToString());
        }
    }
}
