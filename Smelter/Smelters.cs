using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace EraSystem.Smelters
{
    class Smelters
    {
        [HarmonyPatch(typeof(Smelter), nameof(Smelter.Awake))]
        public static class OnAddOre
        {
            [HarmonyPriority(Priority.Last)]
            private static void Postfix(Smelter __instance)
            {
                if (!__instance) return;

                List<Smelter.ItemConversion> metalList = new List<Smelter.ItemConversion>();

                if (Plugin.Era.Value == "stone")
                {
                    metalList = __instance.m_conversion.Where(x => Plugin.BlockedMetalsAtStoneAge.Value.Split(',').Contains(x.m_from.name.ToLower())).ToList();
                }
                else if (Plugin.Era.Value == "bronze")
                {
                    metalList = __instance.m_conversion.Where(x => Plugin.BlockedMetalsAtBronzeAge.Value.Split(',').Contains(x.m_from.name.ToLower())).ToList();

                }
                else if (Plugin.Era.Value == "iron")
                {
                    metalList = __instance.m_conversion.Where(x => Plugin.BlockedMetalsAtIronAge.Value.Split(',').Contains(x.m_from.name.ToLower())).ToList();

                }
                else if (Plugin.Era.Value == "silver")
                {
                    metalList = __instance.m_conversion.Where(x => Plugin.BlockedMetalsAtSilverAge.Value.Split(',').Contains(x.m_from.name.ToLower())).ToList();

                }
                else if (Plugin.Era.Value == "blackmetal")
                {
                    metalList = __instance.m_conversion.Where(x => Plugin.BlockedMetalsAtBlackmetalAge.Value.Split(',').Contains(x.m_from.name.ToLower())).ToList();

                }
                else if (Plugin.Era.Value == "mist")
                {
                    metalList = __instance.m_conversion.Where(x => Plugin.BlockedMetalsAtMistAge.Value.Split(',').Contains(x.m_from.name.ToLower())).ToList();

                }
                else if (Plugin.Era.Value == "fire")
                {
                    metalList = __instance.m_conversion.Where(x => Plugin.BlockedMetalsAtFireAge.Value.Split(',').Contains(x.m_from.name.ToLower())).ToList();

                }
                else if (Plugin.Era.Value == "ice")
                {
                    metalList = __instance.m_conversion.Where(x => Plugin.BlockedMetalsAtIceAge.Value.Split(',').Contains(x.m_from.name.ToLower())).ToList();

                }
                else if (Plugin.Era.Value == "end")
                {
                    metalList = __instance.m_conversion.Where(x => Plugin.BlockedMetalsAtEndAge.Value.Split(',').Contains(x.m_from.name.ToLower())).ToList();
                }

                foreach(var x in metalList)
                {
                    __instance.m_conversion.Remove(x);
                }
            }
        }
    }
}
