using BepInEx;

namespace Adrenaline
{
    [BepInPlugin("com.kobrakon.adrenaline", "Adrenaline", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        void Awake()
        {
            new AdrenalinePatch().Enable();
        }
    }
}