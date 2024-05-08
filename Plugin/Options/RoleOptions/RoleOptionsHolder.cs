using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheSpaceRoles
{
    public static class RoleOptionsHolder
    {
        public static List<RoleOptions> roleOptions = new();
        public static Roles selectedRoles = Roles.None;
        public static void RoleOptionsCreate(ref GameObject v,ref GameObject setting)
        {

            roleOptions = [];
            int i = 0;
            //一旦全部取得にしてるけど後で変えるかも?

            foreach (Roles role in Enum.GetValues(typeof(Roles)))
            {
                if (GetLink.CustomRoleLink.Any(x => x.Role == role))
                {
                    roleOptions.Add(new RoleOptions(role, i));

                    i++;
                }
            }
            ScrollerP scroller = new("RO_Scroller", ref v, ref setting, new(-5, 5, 0), new(-3, -5, 0), new(-3.45f, -0.5f, 0), roleOptions.Count * 0.36f,v.transform.position.y);
        }

    }
}
