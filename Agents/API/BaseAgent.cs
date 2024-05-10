using Interactables;
using PlayerRoles.FirstPersonControl;

namespace Agents.API
{
    public class BaseAgent : MonoBehaviour
    {
        public static Dictionary<ReferenceHub, BaseAgent> HubToAgent = new Dictionary<ReferenceHub, BaseAgent>();
        public static Dictionary<int, BaseAgent> IdToAgent = new Dictionary<int, BaseAgent>();

        public static LayerMask InteractionRayLayer = new LayerMask()
        {
            value = 134374145,
        };

        public static List<BaseAgent> List => HubToAgent.Values.ToList();
        public static int Count => HubToAgent.Count;

        public static BaseAgent CreateAndSpawn(string name, RoleTypeId role, Vector3 position)
        {
            BaseAgent agent = Create(name);
            agent.Spawn(role, position);
            return agent;
        }

        public static BaseAgent Create(string name)
        {
            var playerPrefab = UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);
            var hub = playerPrefab.GetComponent<ReferenceHub>();

            var agent = playerPrefab.AddComponent<BaseAgent>();
            agent.DisplayName = name;
            agent.Initialize(hub);
            return agent;
        }

        public static void AssignFreeNetworkId(BaseAgent agent)
        {
            for (int x = 9000; x < int.MaxValue; x++)
            {
                if (IdToAgent.ContainsKey(x))
                    continue;

                IdToAgent.Add(x, agent);
                agent.NetworkId = x;
                break;
            }
        }

        public int NetworkId { get; internal set; }
        public string DisplayName { get; private set; }

        public ReferenceHub Hub { get; private set; }
        public NavMeshAgent Agent { get; private set; }

        public ObstacleAvoidanceType ObstacleAvoidance
        {
            get => Agent.obstacleAvoidanceType;
            set { Agent.obstacleAvoidanceType = value; }
        }

        public InteractionCoordinator InterCoordinator => Hub.interCoordinator;

        public Vector3? SpawnLocation { get; set; }

        public FakeConnection Connection { get; private set; }

        public IFpcRole FpcRole => Hub.roleManager.CurrentRole is IFpcRole role ? role : null;

        public void Initialize(ReferenceHub hub)
        {
            AssignFreeNetworkId(this);

            HubToAgent.Add(hub, this);

            Hub = hub;

            Connection = new FakeConnection(NetworkId);

            string targetName = "Agent";

            hub.nicknameSync._firstNickname = targetName;
            hub.nicknameSync.Network_myNickSync = targetName;
            hub.nicknameSync.NickSet = true;

            NetworkServer.AddPlayerForConnection(Connection, gameObject);
        }

        void InitializeAgent()
        {
            if (Agent != null) return;

            Agent = gameObject.AddComponent<NavMeshAgent>();
            Agent.baseOffset = 0.98f;

            Agent.updateRotation = true;
            Agent.angularSpeed = 360;
            Agent.acceleration = 600;

            Agent.radius = 0.1f;
            Agent.areaMask = 1;
            ObstacleAvoidance = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
        }

        public void Spawn(RoleTypeId role, Vector3 position)
        {
            try
            {
                Hub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Role;
                Hub.nicknameSync.ViewRange = 0f;
            }
            catch (Exception)
            {
            }

            try
            {
                SpawnLocation = position;
                Hub.roleManager.ServerSetRole(role, RoleChangeReason.RemoteAdmin);
            }
            catch (Exception)
            {
            }
        }

        public bool IsEnemy(ReferenceHub hub)
        {
            if (hub.characterClassManager._godMode)
                return false;

            if (hub.roleManager.CurrentRole.RoleTypeId == RoleTypeId.Tutorial)
                return false;

            return HitboxIdentity.IsEnemy(Hub, hub);
        }

        internal void InternalOnSpawned() => InitializeAgent();
        internal void InternalOnRotationUpdate() => FpcRole.FpcModule.MouseLook.CurrentHorizontal = transform.eulerAngles.y;

        void DoRaycast()
        {
            if (Physics.Raycast(new Ray(Hub.PlayerCameraReference.position, Hub.transform.forward), out RaycastHit hit, 300f, InteractionRayLayer))
                OnHitSomething(hit);
        }

        void OnHitSomething(RaycastHit hit)
        {
            if (!hit.collider.TryGetComponent<InteractableCollider>(out InteractableCollider col))
                return;

            if (col.Target is not IInteractable inter)
                return;

            if (!InteractionCoordinator.GetSafeRule(inter).ClientCanInteract(col, hit))
                return;

            if (col.Target is not NetworkBehaviour net)
                return;

            InterCoordinator.UserCode_CmdServerInteract__NetworkIdentity__Byte(net.netIdentity, col.ColliderId);
        }

        void Update()
        {
            if (Agent == null) return;

            if (!Agent.isOnNavMesh)
            {
                Agent.enabled = false;
                Agent.enabled = true;
                return;
            }

            if (!Hub.IsAlive())
            {
                if (!Agent.isStopped)
                    Agent.isStopped = true;
                return;
            }
            else if (Agent.isStopped) { }
            {
                Agent.isStopped = false;
            }

            DoRaycast();
        }

        void OnDestroy()
        {
            IdToAgent.Remove(NetworkId);
            HubToAgent.Remove(Hub);
        }
    }
}
