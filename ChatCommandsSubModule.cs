using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using HarmonyLib;
using System.Reflection;
using ChatCommands.Patches;
using System.IO;
using Newtonsoft.Json;
using NetworkMessages.FromServer;
using TaleWorlds.MountAndBlade.ViewModelCollection.Multiplayer;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;
using System.Collections.Generic;

namespace ChatCommands
{



    public class ChatCommandsSubModule : MBSubModuleBase

    {

        public static ChatCommandsSubModule Instance { get; private set; }

        private void setup()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            string configPath = Path.Combine(basePath, "chatCommands.json");
            if (!File.Exists(configPath))
            {
                Config config = new Config();
                config.AdminPassword = Helpers.RandomString(6);
                ConfigManager.SetConfig(config);
                string json = JsonConvert.SerializeObject(config);
                File.WriteAllText(configPath, json);
            }
            else
            {
                string configString = File.ReadAllText(configPath);
                Config config = JsonConvert.DeserializeObject<Config>(configString);
                ConfigManager.SetConfig(config);
            }
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            this.setup();

            Debug.Print("** Dume pagame **", 0, Debug.DebugColor.Green);

            CommandManager cm = new CommandManager();
            Harmony.DEBUG = true;

            var harmony = new Harmony("mentalrob.chatcommands.bannerlord");
            //harmony.PatchAll(assembly);
            var original = typeof(ChatBox).GetMethod("ServerPrepareAndSendMessage", BindingFlags.NonPublic | BindingFlags.Instance);
            Debug.Print("Cargando Prefix");
            var prefix = typeof(PatchChatBox).GetMethod("Prefix");
            Debug.Print("Terminado de cargar Prefix");

            Debug.Print("Cargando Postfix");
            var postfix = typeof(MapScreenPath).GetMethod("Postfix");
            Debug.Print("Terminado de cargar PostFix");

            Debug.Print("Parcheado Harmony");
            harmony.Patch(original, prefix: new HarmonyMethod(prefix),postfix:new HarmonyMethod(postfix));
            Debug.Print("Terminado de parchear Harmony");
        }

        protected override void OnSubModuleUnloaded()
        {
            Debug.Print("** CHAT COMMANDS BY MENTALROB UNLOADED **", 0, Debug.DebugColor.Green);
            // Game.OnGameCreated -= OnGameCreated;
        }


        public override void OnMultiplayerGameStart(Game game, object starterObject)
        {

            Debug.Print("** CHAT HANDLER ADDED **", 0, Debug.DebugColor.Green);
            game.AddGameHandler<ChatHandler>();
            // game.AddGameHandler<ManipulatedChatBox>();
        }
        public override void OnGameEnd(Game game)
        {
            game.RemoveGameHandler<ChatHandler>();
        }
        public void test()
        {
            GameNetwork.BeginBroadcastModuleEvent();
            GameNetwork.WriteMessage(new ServerMessage("Funciona"));
            GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None);
        }
    }
}
