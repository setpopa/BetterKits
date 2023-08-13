using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace BetterKits
{ 
    public class Kits : RocketPlugin<KitsConfiguration>
    {
        public static Kits Instance = null;       

        public static Dictionary<string, DateTime> GlobalCooldown = new Dictionary<string,DateTime>();
        public static Dictionary<string, DateTime> InvididualCooldown = new Dictionary<string, DateTime>();

        protected override void Load()
        {
            Instance = this;
            if (IsDependencyLoaded("Uconomy"))
            {
                Logger.Log("Optional dependency Uconomy is present.");
            }
            else
            {
                Logger.Log("Optional dependency Uconomy is not present.");
            }
        }
        public void AddToConfigAndCache(Kit kit)
        {
            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
            XDocument config = XDocument.Load(System.IO.Directory.GetCurrentDirectory() + "\\Plugins\\Kits\\Kits.configuration.xml");
            Configuration.Instance.Kits.Add(kit);
            config.Element("KitsConfiguration").Element("Kits").Add(
                new XElement("Kit",
                    new XElement("Name",kit.Name),
                    new XElement("XP",kit.XP),
                    new XElement("Money",kit.Money),
                    new XElement("Vehicle", new XAttribute(xsi + "nil", true)),
                    new XElement("Items", !kit.Items.Any() ? (object)new XAttribute(xsi + "nil", true) : kit.Items.Select(i => new XElement("Item", new XAttribute("id", i.ItemId), new XAttribute("amount", i.Amount)))),
                    //new XElement("Items", kit.Items.Select(i => new XElement("Item", new XAttribute("id",i.ItemId),new XAttribute("amount", i.Amount)))),
                    new XElement("Cooldown", kit.Cooldown)
                ));
            config.Save(System.IO.Directory.GetCurrentDirectory() + "\\Plugins\\Kits\\Kits.configuration.xml");
        }
        public bool RemoveFromConfigAndCache(string kitName)
        {
            XDocument config = XDocument.Load(System.IO.Directory.GetCurrentDirectory() + "\\Plugins\\Kits\\Kits.configuration.xml");
            Kit kit = Kits.Instance.Configuration.Instance.Kits.Where(k => k.Name.ToLower() == kitName.ToLower()).FirstOrDefault();            
            if (kit != null)
            {
                Configuration.Instance.Kits.Remove(kit);
                config.Element("KitsConfiguration").Element("Kits").Elements("Kit").Where(x => x.Element("Name").Value.ToLower() == kitName.ToLower()).Remove();
                config.Save(System.IO.Directory.GetCurrentDirectory() + "\\Plugins\\Kits\\Kits.configuration.xml");
                return true;            
            }
            return false;         
        }
        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList(){
                    {"RemoveKitSyntaxError","/rkit <kitName>" },
                    {"KitRemoved","Kit was succefully removed" },
                    {"CreateKitInvalidMoney", "Money are in incorrect format. Use -(value) to make it cost of the kit." },
                    { "CreateKitFormat", "Format: /ckit <name> <cooldown> [experience] [money]" },
                    { "CreateKitExists", "The kit with such name already exists" },
                    { "CreateKitInvalidCooldown", "Cooldown is in incorrect format" },
                    { "CreateKitSuccess", "Successfully created kit {0} cooldown {1} with {2} items" },
                    {"command_kit_invalid_parameter","Invalid parameter, specify a kit with /kit <name>"},
                    {"command_kit_not_found","Kit not found"},
                    {"command_kit_no_permissions","You don't have permissions to use this kit"},
                    {"command_kit_cooldown_command","You have to wait {0} seconds to use this command again"},
                    {"command_kit_cooldown_kit","You have to wait {0} seconds to get this kit again"},
                    {"command_kit_failed_giving_item","Failed giving a item to {0} ({1},{2})"},
                    {"command_kit_success","You just received the kit {0}" },
                    {"command_kits","You have access to the following kits: {0}" },
                    {"command_kit_no_money","You can't afford the kit {2}. You need atleast {0} {1}." },
                    {"command_kit_money","You have received {0} {1} from the kit {2}." },
                    {"command_kit_xp","You have received {0} xp from the kit {1}." }
                };
            }
        }
    }
}
