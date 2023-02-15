using EFT;
using System;
using System.Linq;
using System.Reflection;
using Aki.Reflection.Patching;

namespace Adrenaline
{
    public class AdrenalinePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(Player).GetMethod("ReceiveDamage", BindingFlags.Instance | BindingFlags.NonPublic);

        [PatchPostfix]
        static void PostFix(ref Player __instance, EDamageType type)
        {
            if (type == EDamageType.Bullet || type == EDamageType.Explosion || type == EDamageType.Sniper || type == EDamageType.Landmine || type == EDamageType.GrenadeFragment)
            {
                try
                {
                    if (__instance.ActiveHealthController.BodyPartEffects.Effects[0].Any(v => v.Key == "PainKiller"))
                    {
                        ActiveHealthControllerClass.GClass2091 pk = typeof(ActiveHealthControllerClass).GetMethod("FindActiveEffect", BindingFlags.Instance | BindingFlags.Public).MakeGenericMethod(typeof(ActiveHealthControllerClass).GetNestedType("PainKiller", BindingFlags.Instance | BindingFlags.NonPublic)).Invoke(__instance.ActiveHealthController, new object[] { EBodyPart.Head }) as ActiveHealthControllerClass.GClass2091;
                        if (pk.TimeLeft < 30) pk.AddWorkTime(30f, false);
                        return;
                    }
                    MethodInfo method = typeof(ActiveHealthControllerClass).GetMethod("method_15", BindingFlags.Instance | BindingFlags.NonPublic);
                    method.MakeGenericMethod(typeof(ActiveHealthControllerClass).GetNestedType("PainKiller", BindingFlags.Instance | BindingFlags.NonPublic)).Invoke(__instance.ActiveHealthController, new object[]{ EBodyPart.Head, 0f, 30f, 5f, 1f, null });
                }
                catch (Exception) {} // sometimes it just kinda throws exceptions and idk why
            }
        }
    }
}
