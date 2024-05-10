using PlayerRoles.FirstPersonControl;

namespace Agents.Patches
{
    [HarmonyPatch(typeof(FpcMouseLook))]
    public static class FpcMouseLookPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(FpcMouseLook.UpdateRotation))]
        public static bool OnUpdateRotation(FpcMouseLook __instance)
        {
            if (!BaseAgent.HubToAgent.TryGetValue(__instance._hub, out BaseAgent agent))
                return true;

            agent.InternalOnRotationUpdate();
            return false;
        }
    }
}
