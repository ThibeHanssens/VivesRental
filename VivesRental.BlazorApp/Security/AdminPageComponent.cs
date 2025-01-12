using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace VivesRental.BlazorApp.Security;

// **AdminPageComponent**: Basisklasse voor pagina's die alleen toegankelijk zijn voor geautoriseerde gebruikers.
[Authorize]
public class AdminPageComponent : ComponentBase
{
    // Deze klasse kan worden uitgebreid door componenten die autorisatie vereisen.
}
