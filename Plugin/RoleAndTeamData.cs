﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheSpaceRoles
{
    /*all すべてのteam

        c crew
        i impostor
        m madmate
        j jackal
        je jester
        ar arsonist
        vu vulture

        */
    public enum Roles : int
    {
        //normal(これが当てはまることはおそらくないかと
        None = 0,
        //default
        Crewmate,
        Impostor,
        Jackal,
        Jester,
        Arsonist,
        Vulture,
        lawyer,//弁護士
        Prosecutor,//検察官?
        Pursuer,//追跡者?
        Thief,//泥棒?

        //all or other(ここから　
        Mayor,//all
        Engineer,//c
        Sheriff,//c 
        Deputy,//c,m?
        Lighter,//all
        Detective,//all
        TimeMaster,//all, iとかいらねえだろ
        Medic,//all,cだけだろこれぇ 
        Swapper,//all
        Seer,//all
        Hacker,//all
        Tracker,//all
        Snitch,//c,m,? 仕様変更入るけど
        Spy,//c,j?
        Portalmaker,//c ?
        Securityguard,//c
        Guesser,//all
        Medium,//c
        Trapper,//all

    }
    public enum Teams : int
    {
        Crewmate = 0,
        Madmate,
        Impostor,
        Jackal,
        //Oppotunist,
        Jester,
        Arsonist,
        Vulture,
        lawyer,//弁護士
        Prosecutor,//検察官?
        Pursuer,//追跡者?
        Thief//泥棒?
    }


    /// <summary>
    /// Linkだよ!!
    /// </summary>
    public static class RoleLink
    {

        public static List<RoleMaster> RolesMasterLink = new()
        {
            new Crewmate(),
            new Impostor(),
            new Sheriff(),
        };


        public static List<RoleMaster> RolesMasterNormalLink = new()
        {
            new Crewmate(),
            new Impostor(),
        };
        public static Dictionary<Teams, Color> ColorFromTeams= new()
        {
            {Teams.Crewmate,Palette.CrewmateBlue},
            {Teams.Impostor,Palette.ImpostorRed}
        };


        public static RoleMaster GetRoleMaster(Roles roles)
        {
            if (!RolesMasterLink.Any(x => x.Role == roles)) { Logger.Error($"{roles} is not contained in RoleMasterLink"); return null; }
            return RolesMasterLink.First(x => x.Role == roles);
        }
        public static RoleMaster GetRoleMasterNormal(Teams teams)
        {
            if (!RolesMasterNormalLink.Any(x => x.teamsSupported.Contains(teams))) { Logger.Error($"{teams} is not contained in RoleMasterNoramlLink"); return null; }

            return RolesMasterNormalLink.First(x => x.teamsSupported.Contains(teams));
        }


    }
}
