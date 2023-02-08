using NetworkMessages.FromServer;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ChatCommands.Commands
{
    class TeleportAll : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            bool isAdmin = false;
            bool isExists = AdminManager.Admins.TryGetValue(networkPeer.VirtualPlayer.Id.ToString(), out isAdmin);
            return isExists && isAdmin;
        }

        public string Command()
        {
            return "!tpall";
        }

        public string Description()
        {
            return "Teleporting all players.";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            foreach (NetworkCommunicator targetPeer in GameNetwork.NetworkPeers)
            {
                if (networkPeer.ControlledAgent != null && targetPeer.ControlledAgent != null)
                {
                    Vec3 targetPos = networkPeer.ControlledAgent.Position;
                    targetPos.x = targetPos.x + 1;
                    targetPeer.ControlledAgent.TeleportToPosition(targetPos);
                }
            }
            GameNetwork.BeginBroadcastModuleEvent();
            GameNetwork.WriteMessage(new ServerMessage("Players are teleported"));
            GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None);
            return true;
        }
    }
}
