using UnityEngine;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    public abstract class CustomTeam
    {
        public Teams Team;
        public Color Color = new(0, 0, 0);
        public bool HasKillButton = false;
        public bool CanUseVent = true;
        public bool CanUseAdmin = true;
        public bool CanUseCamera = true;
        public bool CanUseVital = true;
        public bool CanUseDoorlog = true;
        public bool CanUseBinoculars = true;
        public bool CanRepairSabotage = true;
        public bool HasTask = true;
        public string ColoredTeamName => ColoredText(Color, Translation.GetString("team." + Team.ToString() + ".name"));
        public string RoleName => Translation.GetString("team." + Team.ToString() + ".name");
        public string ShortRoleName => Translation.GetString("team." + Team.ToString() + ".sname");
        public string ColoredShortRoleName => ColoredText(Color, Translation.GetString("team." + Team.ToString() + ".sname"));
        public string ColoredIntro => ColoredText(Color, Translation.GetString("intro.cosmetic", [Translation.GetString("team." + Team.ToString() + ".intro")]));
        public string Description => Translation.GetString("team." + Team.ToString() + ".description"); 
        public virtual void WinCheck() { }
    }
}
