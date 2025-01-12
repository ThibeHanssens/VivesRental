namespace Vives.Presentation.Authentication
{
    public interface IBearerTokenStore
    {
        // Haalt het huidige token op.
        string GetToken();

        // Stelt een nieuw token in.
        void SetToken(string token);

    }
}
