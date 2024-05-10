namespace Agents.Patches
{
    [HarmonyPatch(typeof(QueryProcessor))]
    public static class QueryProcessorPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(QueryProcessor.OnDestroy))]
        public static bool OnDestroy(QueryProcessor __instance) => !BaseAgent.HubToAgent.ContainsKey(__instance._hub);
    }
}
