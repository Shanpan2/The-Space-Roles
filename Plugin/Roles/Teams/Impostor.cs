namespace TheSpaceRoles
{
    public class ImpostorTeam : CustomTeam
    {
        public ImpostorTeam()
        {
            Team = Teams.Impostor;
            Color = Palette.ImpostorRed;
            HasKillButton = true;
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
