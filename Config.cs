using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;
using PlayerRoles;

namespace MOTSSCPs
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public bool Debug { get; set; } = false;

        [Description("If the plugin only activates if all SCPs are taken. False causes duplicate SCPs can spawn when the minimum number of players is reached.")]
        public bool ActivateOnlyIfFullOfSCP { get; set; } = false;

        [Description("Minimum of players needed for the plugin activate(If you have ACtivateOnlyIfFullOfSCP in true still affecting)")]
        public int MinimumOfPlayers { get; set; } = 20;

        [Description("How many players spawn a new SCP after the minimum")]
        public int PlayersForNewSCP { get; set; } = 5;

        [Description("Limit of each SCP role that can be in a game(0 is disabled spawn with this plugin, 1 can spawned if don't spawn byself).")]
        public Dictionary<RoleTypeId, int> RolesLimit { get; set; } = new Dictionary<RoleTypeId, int>()
        {  {RoleTypeId.Scp939, 3 }, { RoleTypeId.Scp106, 1 }, { RoleTypeId.Scp173, 2 },
           { RoleTypeId.Scp096, 1 }, { RoleTypeId.Scp049, 2 }, { RoleTypeId.Scp079, 2 }, { RoleTypeId.Scp3114, 0 }  };

    }
}
