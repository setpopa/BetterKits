using Rocket.API;
using Rocket.Unturned.Chat;
using System.Collections.Generic;

namespace BetterKits
{
    internal class RemoveKitCommand : IRocketCommand
    {
        private Kits pluginInstance => Kits.Instance;
        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 1) {
                UnturnedChat.Say(caller, pluginInstance.Translate("RemoveKitSyntaxError"));
                return;
            }
            if (pluginInstance.RemoveFromConfigAndCache(command[0]))
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("KitRemoved"));
                return;
            }
            else
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("command_kit_not_found"));
                return;
            }
        }
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "removekit";

        public string Help => "Remove kit by its name.";

        public string Syntax => "/rkit <kitname>";

        public List<string> Aliases => new List<string>() { "rkit" };

        public List<string> Permissions => new List<string>() { "kits.removekit" };
        
    }
}
