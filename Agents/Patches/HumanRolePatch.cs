namespace Agents.Patches
{
    [HarmonyPatch(typeof(HumanRole))]
    public static class HumanRolePatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(HumanRole.SpawnpointHandler), MethodType.Getter)]
        public static bool OnSpawnPointHandler(HumanRole __instance, ref ISpawnpointHandler __result)
        {
            if (!__instance.TryGetOwner(out ReferenceHub ownerHub))
                return true;

            if (!BaseAgent.HubToAgent.TryGetValue(ownerHub, out BaseAgent agent))
                return true;

            if (!agent.SpawnLocation.HasValue) return true;

            __result = new CustomSpawnPoint()
            {
                Position = agent.SpawnLocation.Value,
            };
            agent.SpawnLocation = null;
            return false;
        }
    }
}
