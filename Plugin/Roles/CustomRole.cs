using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    public abstract class CustomRole
    {
        public int PlayerId;
        public string PlayerName;
        public Teams[] teamsSupported = Enum.GetValues(typeof(Teams)).Cast<Teams>().ToArray();
        public Teams Team;
        public Roles Role;
        public Color Color = new(0, 0, 0);
        public bool HasKillButton = false;
        public bool HasAbilityButton = false;
        public int[] AbilityButtonType = [];
        public bool? CanUseVent = null;
        public bool? CanUseAdmin = null;
        public bool? CanUseCamera = null;
        public bool? CanUseVital = null;
        public bool? CanUseDoorlog = null;
        public bool? CanUseBinoculars = null;
        public bool? CanRepairSabotage = null;
        public bool? HasTask = null;
        public void Init()
        {
            CanUseVent = CanUseVent == null ? GetLink.GetCustomTeam(Team).CanUseVent : CanUseVent;
            CanUseAdmin = CanUseAdmin == null ? GetLink.GetCustomTeam(Team).CanUseAdmin : CanUseAdmin;
            CanUseBinoculars = CanUseBinoculars == null ? GetLink.GetCustomTeam(Team).CanUseBinoculars : CanUseBinoculars;
            CanUseCamera = CanUseCamera == null ? GetLink.GetCustomTeam(Team).CanUseCamera : CanUseCamera;
            CanUseDoorlog = CanUseDoorlog == null ? GetLink.GetCustomTeam(Team).CanUseDoorlog : CanUseDoorlog;
            CanUseBinoculars = CanUseBinoculars == null ? GetLink.GetCustomTeam(Team).CanUseBinoculars : CanUseBinoculars;
            CanRepairSabotage = CanRepairSabotage == null ? GetLink.GetCustomTeam(Team).CanUseBinoculars : CanRepairSabotage;
            HasTask = HasTask == null ? GetLink.GetCustomTeam(Team).HasTask : HasTask;


        }
        public virtual void HudManagerStart(HudManager hudManager) { }
        public virtual void Killed() { }
        public virtual void WasKilled() { }
        public virtual void Update() { }
        public virtual void APUpdate() { }
        public string ColoredRoleName => ColoredText(Color, Translation.GetString("role." + Role.ToString() + ".name"));
        public string RoleName => Translation.GetString("role." + Role.ToString() + ".name");

        public string ColoredIntro => ColoredText(Color, Translation.GetString("intro.cosmetic", [Translation.GetString("role." + Role.ToString() + ".intro")]));
        public string Description()
        {
            string r = "";
            if (teamsSupported.Length == Enum.GetValues(typeof(Teams)).Length)
            {
                r += $"<b>{Translation.GetString("team.all.name")}</b>";
            }
            else
            {
                int i = 0;
                foreach (var item in teamsSupported)
                {

                    r += "<b>" + Translation.GetString("team." + item.ToString() + ".name") + "</b>";
                    i++;
                    if (i + 1 != teamsSupported.Length)
                    {
                        r += ",";
                    }
                }

            }
            r += "\n";
            return r.ToString();
        }


        /// <summary>
        /// プレイヤーid入れて初期化
        /// </summary>
        /// <param name="playerId">PlayerControl pc.playerId</param>
        public void ReSet(int playerId)
        {
            PlayerId = playerId;
            PlayerName = DataBase.AllPlayerControls().First(x => x.PlayerId == playerId).name.Replace("<color=.*>", string.Empty).Replace("</color>", string.Empty); ;
            Team = DataBase.AllPlayerTeams[playerId];
            Init();
        }
    }
    [HarmonyPatch(typeof(HudManager))]
    public static class HudManagerGame
    {
        public static bool OnGameStarted = false;
        public static bool IsGameStarting = false;
        [HarmonyPatch(nameof(HudManager.OnGameStart)), HarmonyPostfix]
        public static void ButtonCreate(HudManager __instance)
        {

            if (!OnGameStarted) return;
            IsGameStarting = true;
            OnGameStarted = false;

            ButtonCooldownEnabled = false;
            ButtonCooldown = 10f;
            DataBase.buttons.Clear();
            if (DataBase.AllPlayerRoles.ContainsKey(PlayerControl.LocalPlayer.PlayerId))
            {
                //var k = DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Select(x => x.Role.ToString()).ToArray();
                //Logger.Info(string.Join(",", k));

                DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.HudManagerStart(__instance));
            }



        }
        public static float ButtonCooldown;
        public static bool ButtonCooldownEnabled;
        [HarmonyPatch(nameof(HudManager.Update)), HarmonyPostfix]
        public static void Update()
        {
            if (!IsGameStarting) return;
            if (DataBase.AllPlayerRoles.ContainsKey(PlayerControl.LocalPlayer.PlayerId))
            {

                DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.Update());
                DataBase.AllPlayerRoles.Do(y => y.Value.Do(x => x.APUpdate()));
            }
        }

    }

}