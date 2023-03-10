using EFT;
using System;
using System.Linq;
using System.Reflection;
using Aki.Reflection.Patching;

namespace Adrenaline
{
    public class AdrenalinePatch : ModulePatch
    {
        static Type effectType = typeof(ActiveHealthControllerClass).GetNestedTypes().First(t => t.GetProperty("Strength") != null);
        static MethodInfo effectMethod = typeof(ActiveHealthControllerClass).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).First(m => 
            m.GetParameters().Length == 6
            && m.GetParameters()[0].Name == "bodyPart"
            && m.GetParameters()[5].Name == "initCallback"
            && m.IsGenericMethod
        );

        protected override MethodBase GetTargetMethod() => typeof(Player).GetMethod("ReceiveDamage", BindingFlags.Instance | BindingFlags.NonPublic);

        [PatchPostfix]
        static void Postfix(ref Player __instance, EDamageType type)
        {
            if (type == EDamageType.Bullet || type == EDamageType.Explosion || type == EDamageType.Sniper || type == EDamageType.Landmine || type == EDamageType.GrenadeFragment)
            {
                try
                {
                    if (__instance.ActiveHealthController.BodyPartEffects.Effects[0].Any(v => v.Key == "PainKiller"))
                    {
                        var pk = typeof(ActiveHealthControllerClass).GetMethod("FindActiveEffect", BindingFlags.Instance | BindingFlags.Public).MakeGenericMethod(typeof(ActiveHealthControllerClass).GetNestedType("PainKiller", BindingFlags.Instance | BindingFlags.NonPublic)).Invoke(__instance.ActiveHealthController, new object[] { EBodyPart.Head });
                        if ((int)effectType.GetProperty("TimeLeft").GetValue(pk) < 30) effectType.GetMethod("AddWorkTime").Invoke(pk, new object[] { 30f, false });
                        return;
                    }

                    MethodInfo method = typeof(ActiveHealthControllerClass).GetMethod("method_15", BindingFlags.Instance | BindingFlags.NonPublic);
                    effectMethod.MakeGenericMethod(typeof(ActiveHealthControllerClass).GetNestedType("PainKiller", BindingFlags.Instance | BindingFlags.NonPublic)).Invoke(__instance.ActiveHealthController, new object[]{ EBodyPart.Head, 0f, 30f, 5f, 1f, null });
                }
                catch (Exception) {} // sometimes it just kinda throws exceptions and idk why
            }
        }
    }
}