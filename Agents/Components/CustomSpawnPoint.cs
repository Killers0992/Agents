namespace Agents.Components
{
    public class CustomSpawnPoint : ISpawnpointHandler
    {
        public Vector3 Position { get; set; }
        public float Rotation { get; set; }

        public bool TryGetSpawnpoint(out Vector3 position, out float horizontalRot)
        {
            position = Position;
            horizontalRot = Rotation;
            return true;
        }
    }
}
