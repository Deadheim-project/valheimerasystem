using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EraSystem.Drop
{
    class Drop
    {
        [HarmonyPatch(typeof(CharacterDrop), "Start")]
        public static class Start
        {
            public static void Postfix(ref CharacterDrop __instance)
            {
                if (!IsDisabled(__instance.m_character.gameObject.name)) return;

                __instance.m_dropsEnabled = false;
                __instance.m_drops.RemoveAll(x => x.m_chance > 0);
            }

            private static bool IsDisabled(string creatureName)
            {
                List<string> disabledCrafts = GetDisabled();

                foreach (string x in disabledCrafts)
                {
                    if (string.IsNullOrWhiteSpace(x)) continue;
                    if (creatureName.ToLower().Contains(x.ToLower())) return true;
                }
                return false;
            }

            private static List<string> GetDisabled()
            {
                if (Plugin.Era.Value == "bronze") return Plugin.BlockedCreatureDropAtIronAge.Value.Split(',').Concat(Plugin.BlockedCreatureDropAtSilverAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtBlackmetalAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtMistAge.Value.Split(',').Concat(Plugin.BlockedCreatureDropAtFireAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtIceAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtEndAge.Value.Split(','))).ToList();
                if (Plugin.Era.Value == "iron") return (Plugin.BlockedCreatureDropAtSilverAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtBlackmetalAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtMistAge.Value.Split(',').Concat(Plugin.BlockedCreatureDropAtFireAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtIceAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtEndAge.Value.Split(','))).ToList();
                if (Plugin.Era.Value == "silver") return (Plugin.BlockedCreatureDropAtBlackmetalAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtMistAge.Value.Split(',').Concat(Plugin.BlockedCreatureDropAtFireAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtIceAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtEndAge.Value.Split(','))).ToList();
                if (Plugin.Era.Value == "blackmetal") return (Plugin.BlockedCreatureDropAtMistAge.Value.Split(',').Concat(Plugin.BlockedCreatureDropAtFireAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtIceAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtEndAge.Value.Split(','))).ToList();
                if (Plugin.Era.Value == "mist") Plugin.BlockedCreatureDropAtFireAge.Value.Split(',').Concat(Plugin.BlockedCreatureDropAtIceAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtEndAge.Value.Split(','));
                if (Plugin.Era.Value == "fire") Plugin.BlockedCreatureDropAtIceAge.Value.Split(',').Concat(Plugin.BlockedCreatureDropAtEndAge.Value.Split(','));
                if (Plugin.Era.Value == "ice") Plugin.BlockedCreatureDropAtEndAge.Value.Split(',');
                if (Plugin.Era.Value == "end") return new List<string>();

                return Plugin.BlockedCreatureDropAtBronzeAge.Value.Split(',').Concat(Plugin.BlockedCreatureDropAtIronAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtSilverAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtBlackmetalAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtMistAge.Value.Split(',').Concat(Plugin.BlockedCreatureDropAtFireAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtIceAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtEndAge.Value.Split(','))).ToList();
            }
        }
    }
}
