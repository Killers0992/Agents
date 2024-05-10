namespace AI.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SpawnAgentCommand : ICommand
    {
        public string Command { get; } = "spawnagent";

        public string[] Aliases { get; } = new string[] { "spag" };

        public string Description { get; } = "Spawns agent.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Player.TryGet(sender, out Player plr))
            {
                response = $"Only player an execute this command!";
                return false;
            }
            
            if (arguments.Count == 0)
            {
                response = $"Syntax: spawnagent <role>";
                return false;
            }

            if (!Enum.TryParse<RoleTypeId>(arguments.At(0), true, out RoleTypeId role))
            {
                response = $"Invalid role with name {arguments.At(0)}";
                return false;
            }

            BaseAgent.CreateAndSpawn("Agent", role, plr.Position);
            response = "Spawned agent!";
            return true;
        }
    }
}
