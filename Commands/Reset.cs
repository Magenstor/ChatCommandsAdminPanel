using NetworkMessages.FromServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace ChatCommands.Commands
{
    class Reset : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            bool isAdmin = false;
            bool isExists = AdminManager.Admins.TryGetValue(networkPeer.VirtualPlayer.Id.ToString(), out isAdmin);
            return isExists && isAdmin;
        }

        public string Command()
        {
            return "!reset";
        }

        public string Description()
        {
            return "Reset the map";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            Mission.Current.ResetMission();
            GameNetwork.BeginModuleEventAsServer(networkPeer);
            GameNetwork.WriteMessage(new ServerMessage("Reset"));
            GameNetwork.EndModuleEventAsServer();
            return true;

        }
    }
}
