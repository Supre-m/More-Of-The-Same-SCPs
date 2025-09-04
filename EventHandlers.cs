using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Exiled.Events.EventArgs.Server;
using Exiled.API.Extensions;

namespace MOTSSCPs
{
    public class EventHandlers
    {
        private readonly Plugin _plugin;
        private Plugin plugin = Plugin.Singleton;
        private Config config = Plugin.Singleton.Config;
        private int SCPs;
        public bool ShouldSpawnScp(bool needallscpstaken)
        {
            if (needallscpstaken)
            {
                if (AreAllSCPTaken())
                {
                    int extraPlayers = Mathf.Max(0, Player.List.Count - config.MinimumOfPlayers);

                    int expectedScps = (extraPlayers / config.PlayersForNewSCP);
                    if (config.Debug)
                        Log.Debug($"expected SCPs {expectedScps}(Get True ShouldSpawnScp)");
                    return SCPs != 0;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                int extraPlayers = Mathf.Max(0, Player.List.Count - config.MinimumOfPlayers);

                int expectedScps = (extraPlayers / config.PlayersForNewSCP);

                if (config.Debug)
                    Log.Debug($"expected SCPs {expectedScps}");

                return SCPs != 0;
            }
        }
        public Dictionary<RoleTypeId, int> GetAvailablesSCPs()
        {
            RoleTypeId[] allScps =
            {
        RoleTypeId.Scp173,
        RoleTypeId.Scp096,
        RoleTypeId.Scp106,
        RoleTypeId.Scp079,
        RoleTypeId.Scp3114,
        RoleTypeId.Scp939,
        RoleTypeId.Scp049
    };

            Dictionary<RoleTypeId, int> ScpsInNotUse = new Dictionary<RoleTypeId, int>();

            foreach (var scprole in allScps)
            {
                if (config.RolesLimit[scprole] > Player.Get(scprole).Count())
                {
                    ScpsInNotUse.Add(scprole, (config.RolesLimit[scprole] - Player.Get(scprole).Count()));
                }
            }



            if (config.Debug)
            {
                string diccionary = string.Join("\n", ScpsInNotUse.Select(str => $"{str.Key}({str.Value})"));
                Log.Debug($"Scps in not use {diccionary}");
            }


            return ScpsInNotUse;
        }
        public bool AreAllSCPTaken()
        {

            if (config.RolesLimit[RoleTypeId.Scp3114] == 0)
            {
                RoleTypeId[] allScps =
{
        RoleTypeId.Scp173,
        RoleTypeId.Scp096,
        RoleTypeId.Scp106,
        RoleTypeId.Scp079,
        RoleTypeId.Scp939,
        RoleTypeId.Scp049
    };
                if (allScps.All(scp => Player.Get(scp).Any()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                RoleTypeId[] allScps =
{
        RoleTypeId.Scp173,
        RoleTypeId.Scp096,
        RoleTypeId.Scp106,
        RoleTypeId.Scp079,
        RoleTypeId.Scp3114,
        RoleTypeId.Scp939,
        RoleTypeId.Scp049
    };
                if (allScps.All(scp => Player.Get(scp).Any()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        public RoleTypeId GetRandomSCP()
        {
            var randomscp = GetAvailablesSCPs();
            if (randomscp.Count() == 0)
            {
                return RoleTypeId.ClassD;
            }
            var returningclass = randomscp.Keys.ToList()[UnityEngine.Random.Range(0, randomscp.Keys.Count())];
                return returningclass;
        }
        public void OnChoosingTeamQueue(ChoosingStartTeamQueueEventArgs ev)
        {
            SCPs = 0;
            int scps = (Player.List.Count - config.MinimumOfPlayers) / config.PlayersForNewSCP;
            SCPs = scps;
        }
        public void OnSpawning(ChangingRoleEventArgs ev)
        {
            if (ev.Player == null)
            {
                return;
            }
            if (ev.Reason == Exiled.API.Enums.SpawnReason.RoundStart || ev.Reason == Exiled.API.Enums.SpawnReason.LateJoin)
            {
                if (!config.ActivateOnlyIfFullOfSCP && config.MinimumOfPlayers <= Player.Count)
                {
                    if (ShouldSpawnScp(false))
                    {

                        if (RoleExtensions.GetTeam(ev.NewRole) != Team.SCPs)
                        {
                            var randomscp = GetRandomSCP();
                            if (randomscp.IsScp())
                            {
                                ev.NewRole = randomscp;
                                ev.Player.Position = randomscp.GetRandomSpawnLocation().Position;
                                SCPs--;
                                if (config.Debug)
                                {
                                    Log.Debug($"{ev.Player.Nickname} now is the {randomscp}");
                                }
                                ev.Reason = Exiled.API.Enums.SpawnReason.ForceClass;
                            }
                        }
                    }
                    /*
                      This is only changes SCPs not add news
                          else
                          {
                              if (ev.NewRole.GetTeam() == Team.SCPs)
                              {
                                  ev.NewRole = (GetRandomSCP());
                                  if (config.Debug)
                                  {
                                      Log.Debug($"{ev.Player.Nickname} now is the {ev.NewRole}(not using new scps)");
                                  }
                                  ev.Reason = Exiled.API.Enums.SpawnReason.ForceClass;
                              }
                          }
                    */
                }
                else if (config.ActivateOnlyIfFullOfSCP)
                {
                    if (ShouldSpawnScp(true))
                    {
                        if (RoleExtensions.GetTeam(ev.NewRole) != Team.SCPs)
                        {
                            var randomscp = GetRandomSCP();
                            if (randomscp != RoleTypeId.None)
                            {
                                SCPs--;
                                ev.NewRole = randomscp;
                                ev.Reason = Exiled.API.Enums.SpawnReason.ForceClass;
                            }
                        }
                    }
                }
            }         
        }
    }
}
