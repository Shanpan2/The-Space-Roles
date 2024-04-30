using HarmonyLib;
using Hazel;
using static UnityEngine.GraphicsBuffer;

namespace TheSpaceRoles
{
    public static class CheckedMurderPlayer
    {
        public static void Murder(int id1, int id2, DeathReason reason)
        {

            Helper.GetPlayerControlFromId(id1).MurderPlayer(Helper.GetPlayerControlFromId(id2), MurderResultFlags.Succeeded);
            /*

            DataBase.AllPlayerDeathReasons.Add(id2, reason);
            if (id2 == PlayerControl.LocalPlayer.PlayerId)
            {
                DataBase.buttons.Do(x => x.Death());

                Logger.Info($"Death, reason:{reason}");
            }*/
            if (id1== PlayerControl.LocalPlayer.PlayerId)
            {
                DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.Killed());
            }
            if (id2== PlayerControl.LocalPlayer.PlayerId)
            {
                DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.WasKilled());
            }
        }
        public static void RpcMurder(PlayerControl source, PlayerControl target, DeathReason reason, bool DoCustomRpcMurder = true)
        {


            Murder(source.PlayerId, target.PlayerId, reason);
            MessageWriter writer = Rpc.SendRpc(Rpcs.CheckedMurderPlayer);
            writer.Write((int)source.PlayerId);
            writer.Write((int)target.PlayerId);
            writer.Write((int)reason);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
    }
    public static class UnCheckedMurderPlayer
    {
        public static void Murder(int id1, int id2, DeathReason reason)
        {
            KillAnimationPatch.AnimCancel = true;
            Helper.GetPlayerControlFromId(id1).MurderPlayer(Helper.GetPlayerControlFromId(id2),MurderResultFlags.Succeeded);
            /*
            DataBase.AllPlayerDeathReasons.Add(id2, reason);
            if (id2 == PlayerControl.LocalPlayer.PlayerId)
            {
                DataBase.buttons.Do(x => x.Death());

                Logger.Info($"Death, reason:{reason}");
            }*/
            if (id1 == PlayerControl.LocalPlayer.PlayerId)
            {
                DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.Killed());
            }
            if (id2 == PlayerControl.LocalPlayer.PlayerId)
            {
                DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.WasKilled());
            }
        }
        public static void RpcMurder(PlayerControl source, PlayerControl target, DeathReason reason, bool DoCustomRpcMurder = true)
        {


            Murder(source.PlayerId, target.PlayerId, reason);
            MessageWriter writer = Rpc.SendRpc(Rpcs.UnCheckedMurderPlayer);
            writer.Write((int)source.PlayerId);
            writer.Write((int)target.PlayerId);
            writer.Write((int)reason);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
    }
}
