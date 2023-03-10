using HarmonyLib;
using GorillaLocomotion;
using System.Collections;

namespace stats.Patches
{
    [HarmonyPatch(typeof(Player), "Awake")]
    public class InitPatch
    {
        public static void Postfix(Player __instance)
        {
            __instance.StartCoroutine(Delay());
        }

        public static IEnumerator Delay()
        {
            yield return null;

            Plugin.Instance.Init();
            yield break;
        }
    }
}
