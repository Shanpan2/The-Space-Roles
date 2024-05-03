using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSpaceRoles
{
    public static class RoleOptionOptions
    {
        public static void Check(Teams teams, Roles roles)
        {

            CustomOptionsHolder.RoleOptions.Do(x => x.@object.active = false);
            int i = 0;
            foreach (var ro in CustomOptionsHolder.RoleOptions.Where(x => x.role == roles && x.team == teams))
            {
                ro.@object.SetActive(true);
                ro.Check(i + 4);
                i++;
            }
        }
    }
}
