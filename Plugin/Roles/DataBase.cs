using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheSpaceRoles
{
    public static class DataBase
    {
        /// <summary>
        /// playerId,RoleMaster型で役職の型を入れれる
        /// </summary>
        public static Dictionary<int, CustomRole[]> AllPlayerRoles = [];//playerId,roles

        /// <summary>
        /// playerId,Teams型で陣営型を入れれる
        /// </summary>
        public static Dictionary<int, Teams> AllPlayerTeams = [];//playerId,Teams
        public static Dictionary<int, DeathReason> AllPlayerDeathReasons = [];

        public static List<CustomButton> buttons = [];
        public static PlayerControl[] AllPlayerControls()
        {
            return PlayerControl.AllPlayerControls.ToArray().Where(x => !x.isDummy).ToArray();
        }
        /// <summary>
        /// VoteAreaのすべてのプレイヤー
        /// </summary>
        /// <returns>nullの可能性あり</returns>
        public static PlayerVoteArea[] AllPlayerMeeting()
        {
            return MeetingHud.Instance.playerStates.ToArray();
        }
        /// <summary>
        /// RESET!!!!!!
        /// </summary>


        public static void ResetAndPrepare()
        {
            AllPlayerTeams.Clear();
            AllPlayerRoles.Clear();
            AllPlayerDeathReasons.Clear();
            buttons.Do(x => GameObject.Destroy(x.actionButton));
            buttons.Clear();

            HudManagerGame.IsGameStarting = false;

            HudManagerGame.OnGameStarted = true;
        }

        public static Dictionary<Teams, int> GetPlayerCountInTeam()
        {


            Dictionary<Teams, int> result = [];

            foreach (Teams team in Enum.GetValues(typeof(Teams))) 
            {
                result.Add(team,0);
            }
            foreach (var p in AllPlayerRoles)
            {
                if (p.Value.Any(x => !x.Dead))
                {
                    result[p.Value[0].Team.Team]++;
                }
            }
            return result;
        }
        public static int AlivingNotKillPlayer()
        {
            int i = 0;
            foreach(var team in GetPlayerCountInTeam().Where(x => new Teams[] { Teams.Impostor, Teams.Jackal }.Contains(x.Key)))
            {
                i += team.Value;
            }
            return i;
        }
        public static int AlivingKillPlayer()
        {
            int i = 0;
            foreach (var team in GetPlayerCountInTeam().Where(x => new Teams[] { Teams.Impostor, Teams.Jackal }.Contains(x.Key)))
            {
                i += team.Value;
            }
            return i;
        }
    }

}
