namespace TheSpaceRoles
{
    public class CrewmateTeam : CustomTeam
    {
        public CrewmateTeam()
        {
            Team = Teams.Crewmate;
            Color = Palette.CrewmateBlue;
            HasKillButton = false;
            CanRepairSabotage = true;
            CanUseAdmin = true;
            CanUseBinoculars = true;
            CanUseCamera = true;
            CanUseDoorlog = true;
            CanUseVent = false;
            CanUseVital = true;
            HasTask = true;
        }
    }
}
