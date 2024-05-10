namespace Agents
{
    public class MainClass
    {
        Harmony _harmony;

        [PluginEntryPoint("AI", "1.0.0", "AI system for player objects.", "Killers0992")]
        public void Entry()
        {
            Harmony.DEBUG = true;

            _harmony = new Harmony("com.killers0992.ai");
            _harmony.PatchAll();

            EventManager.RegisterAllEvents(this);
        }

        [PluginEvent]
        public void OnMapGenerated(MapGeneratedEvent ev)
        {
            GameObject lr = GameObject.Find("LightRooms");

            AddNavMeshAndBake(lr);

            lr = GameObject.Find("HeavyRooms");

            AddNavMeshAndBake(lr);

            lr = GameObject.Find("EntranceRooms");

            AddNavMeshAndBake(lr);

            lr = GameObject.Find("Outside");

            AddNavMeshAndBake(lr);
        }

        void AddNavMeshAndBake(GameObject go)
        {
            var lightZoneSurface = go.AddComponent<NavMeshSurface>();

            lightZoneSurface.layerMask = new LayerMask()
            {
                value = 305624887,
            };

            lightZoneSurface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            lightZoneSurface.voxelSize = 0.08f;

            lightZoneSurface.BuildNavMesh();
        }
    }
}
