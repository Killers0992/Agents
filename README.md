# Agents

Plugin for SCP: Secret Laboratory which allows you to create fake player which can walk around facility.

# API

```cs
// Creates agent with specific name.
BaseAgent agent = BaseAgent.Create(string name);

// Spawns agent with specific name and role at position.
BaseAgent agent = BaseAgent.CreateAndSpawn(string name, RoleTypeId role, Vector3 position);
```
