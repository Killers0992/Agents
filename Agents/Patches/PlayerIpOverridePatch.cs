namespace Agents.Patches
{
    [HarmonyPatch(typeof(PlayerIpOverride))]
    public static class PlayerIpOverridePatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(PlayerIpOverride.Start))]
        public static bool OnStart(PlayerIpOverride __instance) => !BaseAgent.IdToAgent.ContainsKey(__instance.connectionToClient.connectionId);
    }
}
