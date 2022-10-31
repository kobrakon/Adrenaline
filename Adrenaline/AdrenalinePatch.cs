using EFT;
using System.Reflection;
using Aki.Reflection.Patching;
using System;
using System.Linq;

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
                if (__instance.ActiveHealthController.BodyPartEffects.Effects[0].Any(v => v.Key == "PainKiller"))
                {
                    (typeof(GClass1905).GetMethod("FindActiveEffect", BindingFlags.Instance | BindingFlags.Public).MakeGenericMethod(typeof(GClass1905).GetNestedType("PainKiller", BindingFlags.Instance | BindingFlags.NonPublic)).Invoke(__instance.ActiveHealthController, new object[] { EBodyPart.Head }) as GClass1905.GClass1903).AddWorkTime(30f, true);
                    return;
                }
                MethodInfo method = typeof(GClass1905).GetMethod("method_13", BindingFlags.Instance | BindingFlags.NonPublic);
                method.MakeGenericMethod(typeof(GClass1905).GetNestedType("PainKiller", BindingFlags.Instance | BindingFlags.NonPublic)).Invoke(__instance.ActiveHealthController, new object[]{ EBodyPart.Head, 0f, 30f, 5f, 1f, null });;
            }
        }
    }
}