using HarmonyLib;
using InnerNet;
using System;
using System.Linq;
using UnityEngine;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    [HarmonyPatch]
    public abstract class CustomRole
    {
        public int PlayerId;
        public string PlayerName;
        public PlayerControl PlayerControl;
        public Teams[] teamsSupported = Enum.GetValues(typeof(Teams)).Cast<Teams>().ToArray();
        public CustomTeam Team;
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
            CanUseVent = CanUseVent == null ? GetLink.GetCustomTeam(Team.Team).CanUseVent : CanUseVent;
            CanUseAdmin = CanUseAdmin == null ? GetLink.GetCustomTeam(Team.Team).CanUseAdmin : CanUseAdmin;
            CanUseBinoculars = CanUseBinoculars == null ? GetLink.GetCustomTeam(Team.Team).CanUseBinoculars : CanUseBinoculars;
            CanUseCamera = CanUseCamera == null ? GetLink.GetCustomTeam(Team.Team).CanUseCamera : CanUseCamera;
            CanUseDoorlog = CanUseDoorlog == null ? GetLink.GetCustomTeam(Team.Team).CanUseDoorlog : CanUseDoorlog;
            CanUseBinoculars = CanUseBinoculars == null ? GetLink.GetCustomTeam(Team.Team).CanUseBinoculars : CanUseBinoculars;
            CanRepairSabotage = CanRepairSabotage == null ? GetLink.GetCustomTeam(Team.Team).CanUseBinoculars : CanRepairSabotage;
            HasTask = HasTask == null ? GetLink.GetCustomTeam(Team.Team).HasTask : HasTask;

        }
        public void ResetStart()
        {
            ActionBool(HudManager.Instance.ImpostorVentButton, (bool)CanUseVent);
            ActionBool(HudManager.Instance.KillButton, (bool)HasKillButton);
            HudManager.Instance.ImpostorVentButton.gameObject.SetActive((bool)CanUseVent);
            HudManager.Instance.KillButton.gameObject.SetActive((bool)HasKillButton);
        }
        protected void ActionBool(ActionButton button,bool show_hide)
        {
            if(show_hide)
            {
                button.enabled = true;
                button.gameObject.SetActive(true);
            }
            else
                button.enabled = false;
            button.gameObject.SetActive(false);
            {
                button.Hide();
            }
        }
        public bool Dead = false;
        public bool Exiled = false;



        public virtual void HudManagerStart(HudManager hudManager) { }
        public virtual void MeetingEnd() { }
        public virtual void Killed() { }
        public virtual void WasKilled() { }
        public virtual void Die() { }
        public virtual void Update() { }
        public virtual void APUpdate() { }
        public string ColoredRoleName => ColoredText(Color, Translation.GetString("role." + Role.ToString() + ".name"));
        public string RoleName => Translation.GetString("role." + Role.ToString() + ".name");

        public string ColoredIntro => ColoredText(Color, Translation.GetString("intro.cosmetic", [Translation.GetString("role." + Role.ToString() + ".intro")]));
        public string RoleDescription()
        {
            string r = "";
            string f = "";
            if (teamsSupported.Length == Enum.GetValues(typeof(Teams)).Length)
            {
                f += $"<b>{Translation.GetString("team.all.name")}</b>";
            }
            else
            {
                int i = 0;
                foreach (var item in teamsSupported)
                {
                    f += "<b>" + GetLink.GetCustomTeam(item).ColoredTeamName + "</b>";



                    i++;
                    if (i != teamsSupported.Length)
                    {
                        f += ",";
                    }
                }

            }
            r += $"{Translation.GetString("canvisibleteam", [f])}\n";
            r += Description();

            return r;
        }
        public string Description()
        {
            return $"{Translation.GetString($"role.{Role}.description")}\n ";
        }


        /// <summary>
        /// プレイヤーid入れて初期化
        /// </summary>
        /// <param name="playerId">PlayerControl pc.playerId</param>
        public void ReSet(int playerId,Teams teams)
        {
            PlayerId = playerId;
            PlayerControl = DataBase.AllPlayerControls().First(x=>x.PlayerId==playerId);
            PlayerName = DataBase.AllPlayerControls().First(x => x.PlayerId == playerId).name.Replace("<color=.*>", string.Empty).Replace("</color>", string.Empty); ;
            Team = GetLink.GetCustomTeam(teams);
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
                DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.ResetStart());
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
    [HarmonyPatch(typeof(MeetingHud),nameof(MeetingHud.CheckForEndVoting))]
    public static class MeetingHudVote
    {
        public static void Postfix()
        {
            DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.MeetingEnd());

        }
    }
    [HarmonyPatch(typeof(ActionButton),nameof(ActionButton.SetEnabled))]
    public static class MeetingEndPlayerStart
    {
        public static void Postfix(ActionButton __instance)
        {
            if (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started)
            {

                if (DataBase.AllPlayerRoles != null && DataBase.AllPlayerRoles.ContainsKey(PlayerControl.LocalPlayer.PlayerId))
                {
                    DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.ResetStart());
                }
            }

        }
    }
    [HarmonyPatch(typeof(PlayerControl),nameof(PlayerControl.Exiled))]
    public static class PlayerControlExiledPatch
    {
        public static void Postfix(PlayerControl __instance) {

            DataBase.AllPlayerRoles[__instance.PlayerId].Do(x => x.Team.WasExiled());
            DataBase.AllPlayerRoles[__instance.PlayerId].Do(x => x.Exiled=true);
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Die))]
    public static class PlayerControlDiePatch
    {
        public static void Postfix(PlayerControl __instance)
        {

            DataBase.AllPlayerRoles[__instance.PlayerId].Do(x => x.Die());
            DataBase.AllPlayerRoles[__instance.PlayerId].Do(x => x.Dead=true);
            DataBase.AllPlayerRoles[__instance.PlayerId].Do(x => Logger.Info(x.PlayerId.ToString()));
            Logger.Info(__instance.PlayerId +"_"+__instance.Data.PlayerName);
        }
    }

}