using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using System.Linq;
using SDG.Unturned;

namespace BetterKits
{
    public class CreateKitCommand : IRocketCommand
    {
        private Kits pluginInstance => Kits.Instance;
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length < 2)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("CreateKitFormat"));
                return;
            }

            Kit kit = new Kit();

            kit.Name = command[0];
            if (pluginInstance.Configuration.Instance.Kits.Where(k => k.Name.ToLower() == command[0].ToLower()).FirstOrDefault() != null)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("CreateKitExists"));
                return;
            }

            int cooldown = 0;
            if (!int.TryParse(command[1], out cooldown))
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("CreateKitInvalidCooldown"));
                return;
            }

            int money = 0;
            int.TryParse(command.ElementAtOrDefault(3), out money);
                                   
            kit.Money = money;
            kit.Cooldown = cooldown;
            uint experience = 0;
            uint.TryParse(command.ElementAtOrDefault(2), out experience);
            kit.XP = experience;

            kit.Items = new List<KitItem>();

            var clothing = player.Player.clothing;

            if (clothing.backpack != 0)
                kit.Items.Add(new KitItem(clothing.backpack, 1));
            if (clothing.vest != 0)
                kit.Items.Add(new KitItem(clothing.vest, 1));
            if (clothing.shirt != 0)
                kit.Items.Add(new KitItem(clothing.shirt, 1));
            if (clothing.pants != 0)
                kit.Items.Add(new KitItem(clothing.pants, 1));
            if (clothing.mask != 0)
                kit.Items.Add(new KitItem(clothing.mask, 1));
            if (clothing.hat != 0)
                kit.Items.Add(new KitItem(clothing.hat, 1));
            if (clothing.glasses != 0)
                kit.Items.Add(new KitItem(clothing.glasses, 1));

            for (byte num = 0; num < PlayerInventory.PAGES - 2; num++)
            {
                for (byte num2 = 0; num2 < player.Inventory.getItemCount(num); num2++)
                {
                    var item = player.Inventory.getItem(num, num2);
                    if (item == null)
                        continue;

                    kit.Items.Add(new KitItem(item.item.id, 1));
                }
            }
            pluginInstance.AddToConfigAndCache(kit);           

            UnturnedChat.Say(caller, pluginInstance.Translate("CreateKitSuccess", kit.Name, kit.Cooldown, kit.Items.Count));
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "createkit";

        public string Help => "Creates a new kit of your inventory";

        public string Syntax => "<name> <cooldown> [experience] [money]";

        public List<string> Aliases => new List<string>()
        {
            "ckit"
        };

        public List<string> Permissions => new List<string>() { "kits.createkit" };
    }
}
