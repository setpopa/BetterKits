﻿using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterKits
{
    public class CommandKits : IRocketCommand
    {
        public string Help
        {
            get { return "Shows you available kits"; }
        }

        public string Name
        {
            get { return "kits"; }
        }

        public string Syntax
        {
            get { return ""; }
        }

        public bool RunFromConsole
        {
            get { return false; }
        }
        public List<string> Aliases => new List<string>()
        {
            "oldkits"
        };

        public AllowedCaller AllowedCaller
        {
            get { return Rocket.API.AllowedCaller.Player; }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "kits.kits" };
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            List<string> availableKits = new List<string>();
            List<Kit> kits = Kits.Instance.Configuration.Instance.Kits;
            foreach(Kit kit in kits)
            {
                if(caller.HasPermission("kit." + kit.Name.ToLower()))
                {
                    availableKits.Add(kit.Name);
                }
            }

            UnturnedChat.Say(caller, Kits.Instance.Translations.Instance.Translate("command_kits", String.Join(", ",availableKits.ToArray())));
        }
    }
}
