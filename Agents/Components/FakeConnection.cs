﻿namespace Agents.Components
{
    public class FakeConnection : NetworkConnectionToClient
    {
        public override void Send(ArraySegment<byte> segment, int channelId = 0)
        {
        }

        public override string address => "localhost";

        public FakeConnection(int networkConnectionId) : base(networkConnectionId)
        {
        }
    }
}
