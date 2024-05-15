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
            Color = ColorFromColorcode("#C10000");
            HasKillButton = false;
        }
        public override void HudManagerStart(HudManager __instance)
        {

            foreach (Vent vent in ShipStatus.Instance.AllVents)
            {
                vent.Right = null;
                vent.Center = null;
                vent.Left = null;
            }
        }
    }
}
