using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;
using TaleWorlds.Localization;
using NetworkMessages.FromServer;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ViewModelCollection.Multiplayer;

namespace ChatCommands.Patches
{
    class MapScreenPath
    {
        public static List<EscapeMenuItemVM> Postfix(List<EscapeMenuItemVM> __result)
        {
            
            List<EscapeMenuItemVM> newButtons = new List<EscapeMenuItemVM>()
                {
                    new EscapeMenuItemVM(new TextObject("Test Add Button 1"), o => {
                    GameNetwork.BeginBroadcastModuleEvent();
                    GameNetwork.WriteMessage(new ServerMessage("Funciona esta mierda"));
                    GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None);
                    }, null, () => Tuple.Create(true, new TextObject("Que es esto")))
                };
            __result.AddRange(newButtons);
            return __result;
        }

    }
}

