using NebulaAPI.Networking;
using NebulaAPI.Packets;
using PersonalLogistics.ModPlayer;
using PersonalLogistics.Nebula.Packets;

namespace PersonalLogistics.Nebula.Client
{
    [RegisterPacketProcessor]
    public class RemoveFromNetworkResponseProcessor : BasePacketProcessor<RemoveFromNetworkResponse>
    {
        public override void ProcessPacket(RemoveFromNetworkResponse packet, INebulaConnection conn)
        {
            if (PlogPlayerRegistry.LocalPlayer().playerId.ToString() != packet.clientId)
                return;
            PlogPlayerRegistry.LocalPlayer().shippingManager.CompleteRemoteRequestRemove(packet);
        }
    }
}