namespace Agents.Patches
{
    [HarmonyPatch(typeof(PlayerRoleManager))]
    public static class PlayerRoleManagerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerRoleManager.InitializeNewRole))]
        public static void OnFixedUpdate(PlayerRoleManager __instance, RoleTypeId targetId, RoleChangeReason reason, RoleSpawnFlags spawnFlags, NetworkReader data)
        {
            if (!BaseAgent.HubToAgent.TryGetValue(__instance._hub, out BaseAgent agent))
                return;

            agent.InternalOnSpawned();
        }
    }
}
