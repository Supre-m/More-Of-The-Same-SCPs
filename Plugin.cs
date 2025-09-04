using System;
using Exiled.API.Features;


namespace MOTSSCPs
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "More Of The Same SCPs";
        public override string Author => "Supre";
        public override string Prefix => "MOTSSCPs";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(9, 8, 1);



        public static Plugin Singleton;
        public EventHandlers EventHandlers;


        public override void OnEnabled()
        {
            Singleton = this;
            EventHandlers = new EventHandlers();


            Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.OnSpawning;
            Exiled.Events.Handlers.Server.ChoosingStartTeamQueue += EventHandlers.OnChoosingTeamQueue;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Singleton = null;
            Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.OnSpawning;
            Exiled.Events.Handlers.Server.ChoosingStartTeamQueue -= EventHandlers.OnChoosingTeamQueue;

            base.OnDisabled();
        }

    }
}

