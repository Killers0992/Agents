namespace Agents.Patches
{
    [HarmonyPatch(typeof(PlayerAuthenticationManager))]
    public static class PlayerAuthenticationManagerPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(PlayerAuthenticationManager.FixedUpdate))]
        public static bool OnFixedUpdate(PlayerAuthenticationManager __instance) => !BaseAgent.HubToAgent.ContainsKey(__instance._hub);
    }
}
