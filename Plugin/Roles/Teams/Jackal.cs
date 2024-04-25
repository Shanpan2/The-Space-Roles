namespace TheSpaceRoles
{
    public class JackalTeam : CustomTeam
    {
        public JackalTeam()
        {
            Team = Teams.Jackal;
            Color = Helper.ColorFromColorcode("#09afff");
            HasKillButton = false;
            CanRepairSabotage = true;
            CanUseAdmin = true;
            CanUseBinoculars = true;
            CanUseCamera = true;
            CanUseDoorlog = true;
            CanUseVent = true;
            CanUseVital = true;
            HasTask = false;
        }
    }
}
