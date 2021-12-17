using HarmonyLib;
using System.Linq;


namespace EraSystem.Drop
{
    class Drop
    {

        [HarmonyPatch(typeof(Character), "Awake")]
        public static class Awake
        {
            public static void Prefix(ref Character __instance)
            {
                var drops = __instance.GetComponent<CharacterDrop>();

                if (!drops) return;

                if (Plugin.Era.Value == "stone")
                {
                    if (!Plugin.BlockedFactionDropAtStoneAge.Value.ToLower().Split(',').Contains(__instance.m_faction.ToString().ToLower())) return;
                }
                else if (Plugin.Era.Value == "bronze")
                {
                    if (!Plugin.BlockedFactionDropAtBronzeAge.Value.ToLower().Split(',').Contains(__instance.m_faction.ToString().ToLower())) return;

                }
                else if (Plugin.Era.Value == "iron")
                {
                    if (!Plugin.BlockedFactionDropAtIronAge.Value.ToLower().Split(',').Contains(__instance.m_faction.ToString().ToLower())) return;

                }
                else if (Plugin.Era.Value == "silver")
                {
                    if (!Plugin.BlockedFactionDropAtSilverAge.Value.ToLower().Split(',').Contains(__instance.m_faction.ToString().ToLower())) return;

                }
                else if (Plugin.Era.Value == "blackmetal")
                {
                    if (!Plugin.BlockedFactionDropAtBlackmetalAge.Value.ToLower().Split(',').Contains(__instance.m_faction.ToString().ToLower())) return;

                }
                else if (Plugin.Era.Value == "mist")
                {
                    if (!Plugin.BlockedFactionDropAtMistAge.Value.ToLower().Split(',').Contains(__instance.m_faction.ToString().ToLower())) return;

                }
                else if (Plugin.Era.Value == "fire")
                {
                    if (!Plugin.BlockedFactionDropAtFireAge.Value.ToLower().Split(',').Contains(__instance.m_faction.ToString().ToLower())) return;

                }
                else if (Plugin.Era.Value == "ice")
                {
                    if (!Plugin.BlockedFactionDropAtIceAge.Value.ToLower().Split(',').Contains(__instance.m_faction.ToString().ToLower())) return;

                }
                else if (Plugin.Era.Value == "end")
                {
                    if (!Plugin.BlockedFactionDropAtEndAge.Value.ToLower().Split(',').Contains(__instance.m_faction.ToString().ToLower())) return;
                }

                drops.m_dropsEnabled = false;
                drops.m_drops.RemoveAll(x => x.m_chance > 0);
            }
        }
    }
}
