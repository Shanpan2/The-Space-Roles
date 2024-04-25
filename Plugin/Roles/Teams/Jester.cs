namespace TheSpaceRoles
{
    public class JesterTeam : CustomTeam
    {
        public JesterTeam()
        {
            Team = Teams.Jester;
            Color = Helper.ColorFromColorcode("#ea618e");
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
