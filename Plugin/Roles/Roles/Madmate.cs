using System.Linq;
using UnityEngine;
using UnityEngine.UIElements.UIR;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    public class Madmate : CustomRole
    {
        public static CustomButton VampireBitebutton;
        public PlayerControl BittenPlayerControl;
        public Madmate()
        {

            teamsSupported = [Teams.Madmate];
            Role = Roles.MadMate;
            Color = Palette.ImpostorRed;
            HasKillButton = false;
            CanUseVent = true;
        }
        public override void HudManagerStart(HudManager __instance)
        {

        }
    }
}
