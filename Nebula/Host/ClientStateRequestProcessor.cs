using NebulaAPI;
using NebulaAPI.GameState;
using NebulaAPI.Networking;
using NebulaAPI.Packets;
using PersonalLogistics.ModPlayer;
using PersonalLogistics.Nebula.Packets;
using PersonalLogistics.SerDe;
using PersonalLogistics.Util;

namespace PersonalLogistics.Nebula.Host
{
    [RegisterPacketProcessor]
    public class ClientStateRequestProcessor : BasePacketProcessor<ClientStateRequest>
    {
        /// <summary>
        /// Handled by host, triggers sending of the stored state to a client
        /// </summary>
        public override void ProcessPacket(ClientStateRequest packet, INebulaConnection conn)
        {
            if (IsClient)
                return;
            var remotePlayerId = ClientStateRequest.DecodePlayerId(packet);
            var plogPlayer = PlayerStateContainer.GetPlayer(remotePlayerId, true);
            if (plogPlayer is PlogRemotePlayer remotePlayer)
            {
                var remoteUserBytes = SerDeManager.ExportRemoteUserData(remotePlayer);
                Log.Debug($"Sending client state back to client {remoteUserBytes.Length} bytes");
                NebulaModAPI.MultiplayerSession.Network.SendPacket(new ClientState(remotePlayerId, remoteUserBytes));
            }
            else
            {
                Log.Warn("Invalid state got a local player back while running as host. Assuming player has dupe id");
                conn.SendPacket(new RegenerateUserIdRequest(remotePlayerId));
            }
        }
    }
}