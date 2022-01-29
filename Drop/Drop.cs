using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EraSystem.Drop
{
    public class Drop
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
                List<string> disabledCrafts = GetCreatureDisabled();

                foreach (string x in disabledCrafts)
                {
                    if (string.IsNullOrWhiteSpace(x)) continue;
                    if (creatureName.ToLower().Contains(x.ToLower())) return true;
                }
                return false;
            }

            private static List<string> GetCreatureDisabled()
            {
                if (Plugin.Era.Value == "bronze") return Plugin.BlockedCreatureDropAtIronAge.Value.Split(',').Concat(Plugin.BlockedCreatureDropAtSilverAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtBlackmetalAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtMistAge.Value.Split(',').Concat(Plugin.BlockedCreatureDropAtFireAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtIceAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtEndAge.Value.Split(','))).ToList();
                if (Plugin.Era.Value == "iron") return (Plugin.BlockedCreatureDropAtSilverAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtBlackmetalAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtMistAge.Value.Split(',').Concat(Plugin.BlockedCreatureDropAtFireAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtIceAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtEndAge.Value.Split(','))).ToList();
                if (Plugin.Era.Value == "silver") return (Plugin.BlockedCreatureDropAtBlackmetalAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtMistAge.Value.Split(',').Concat(Plugin.BlockedCreatureDropAtFireAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtIceAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtEndAge.Value.Split(','))).ToList();
                if (Plugin.Era.Value == "blackmetal") return (Plugin.BlockedCreatureDropAtMistAge.Value.Split(',').Concat(Plugin.BlockedCreatureDropAtFireAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtIceAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtEndAge.Value.Split(','))).ToList();
                if (Plugin.Era.Value == "mist") Plugin.BlockedCreatureDropAtFireAge.Value.Split(',').Concat(Plugin.BlockedCreatureDropAtIceAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtEndAge.Value.Split(','));
                if (Plugin.Era.Value == "fire") Plugin.BlockedCreatureDropAtIceAge.Value.Split(',').Concat(Plugin.BlockedCreatureDropAtEndAge.Value.Split(','));
                if (Plugin.Era.Value == "ice") Plugin.BlockedCreatureDropAtEndAge.Value.Split(',');
                if (Plugin.Era.Value == "end") return new List<string>();

                return Plugin.BlockedCreatureDropAtStoneAge.Value.Split(',').Concat(Plugin.BlockedCreatureDropAtBronzeAge.Value.Split(',').Concat(Plugin.BlockedCreatureDropAtIronAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtSilverAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtBlackmetalAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtMistAge.Value.Split(',').Concat(Plugin.BlockedCreatureDropAtFireAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtIceAge.Value.Split(',')).Concat(Plugin.BlockedCreatureDropAtEndAge.Value.Split(',')))).ToList();
            }
        }


        [HarmonyPatch(typeof(DropOnDestroyed), "Awake")]
        public static class AwakeDropOnDestroyed
        {
            private static void Postfix(DropOnDestroyed __instance)
            {
                if (IsDisabled(__instance.gameObject.name))
                {
                    __instance.m_dropWhenDestroyed.m_drops = new List<DropTable.DropData>();
                }
            }
        }

        [HarmonyPatch(typeof(MineRock), "Start")]
        public static class AwakeMineRock
        {
            private static void Postfix(MineRock __instance)
            {
                if (IsDisabled(__instance.gameObject.name))
                {
                    __instance.m_dropItems.m_drops = new List<DropTable.DropData>();
                }
            }
        }

        [HarmonyPatch(typeof(MineRock5), "Start")]
        public static class AwakeMineRock5
        {
            private static void Postfix(MineRock5 __instance)
            {
                if (IsDisabled(__instance.gameObject.name))
                {
                    __instance.m_dropItems.m_drops = new List<DropTable.DropData>();
                }
            }
        }

        [HarmonyPatch(typeof(TreeLog), "Awake")]
        public static class TreeLogStart
        {
            private static void Postfix(TreeLog __instance)
            {
                if (IsDisabled(__instance.gameObject.name))
                {
                    __instance.m_dropWhenDestroyed.m_drops = new List<DropTable.DropData>();
                }
            }
        }

        [HarmonyPatch(typeof(Pickable), "Drop")]
        public static class PickableAwake
        {
            private static bool Prefix(Pickable __instance)
            {
                if (IsDisabled(__instance.gameObject.name)) return false;

                return true;             
            }
        }


        private static bool IsDisabled(string recipeName)
        {
            List<string> disabledCrafts = GetDisabledCrafts();

            foreach (string x in disabledCrafts)
            {
                if (string.IsNullOrWhiteSpace(x)) continue;
                if (recipeName.ToLower().Contains(x.ToLower())) return true;
            }
            return false;
        }

        private static List<string> GetDisabledCrafts()
        {
            if (Plugin.Era.Value == "bronze") return Plugin.BlockedRockAndTreeDropAtIronAge.Value.Split(',').Concat(Plugin.BlockedRockAndTreeDropAtSilverAge.Value.Split(',')).Concat(Plugin.BlockedRockAndTreeDropAtBlackmetalAge.Value.Split(',')).Concat(Plugin.BlockedRockAndTreeDropAtMistAge.Value.Split(',').Concat(Plugin.BlockedRockAndTreeDropAtFireAge.Value.Split(',')).Concat(Plugin.BlockedRockAndTreeDropAtIceAge.Value.Split(',')).Concat(Plugin.BlockedRockAndTreeDropAtEndAge.Value.Split(','))).ToList();
            if (Plugin.Era.Value == "iron") return (Plugin.BlockedRockAndTreeDropAtSilverAge.Value.Split(',')).Concat(Plugin.BlockedRockAndTreeDropAtBlackmetalAge.Value.Split(',')).Concat(Plugin.BlockedRockAndTreeDropAtMistAge.Value.Split(',').Concat(Plugin.BlockedRockAndTreeDropAtFireAge.Value.Split(',')).Concat(Plugin.BlockedRockAndTreeDropAtIceAge.Value.Split(',')).Concat(Plugin.BlockedRockAndTreeDropAtEndAge.Value.Split(','))).ToList();
            if (Plugin.Era.Value == "silver") return (Plugin.BlockedRockAndTreeDropAtBlackmetalAge.Value.Split(',')).Concat(Plugin.BlockedRockAndTreeDropAtMistAge.Value.Split(',').Concat(Plugin.BlockedRockAndTreeDropAtFireAge.Value.Split(',')).Concat(Plugin.BlockedRockAndTreeDropAtIceAge.Value.Split(',')).Concat(Plugin.BlockedRockAndTreeDropAtEndAge.Value.Split(','))).ToList();
            if (Plugin.Era.Value == "blackmetal") return (Plugin.BlockedRockAndTreeDropAtMistAge.Value.Split(',').Concat(Plugin.BlockedRockAndTreeDropAtFireAge.Value.Split(',')).Concat(Plugin.BlockedRockAndTreeDropAtIceAge.Value.Split(',')).Concat(Plugin.BlockedRockAndTreeDropAtEndAge.Value.Split(','))).ToList();
            if (Plugin.Era.Value == "mist") Plugin.BlockedRockAndTreeDropAtFireAge.Value.Split(',').Concat(Plugin.BlockedRockAndTreeDropAtIceAge.Value.Split(',')).Concat(Plugin.BlockedRockAndTreeDropAtEndAge.Value.Split(','));
            if (Plugin.Era.Value == "fire") Plugin.BlockedRockAndTreeDropAtIceAge.Value.Split(',').Concat(Plugin.BlockedRockAndTreeDropAtEndAge.Value.Split(','));
            if (Plugin.Era.Value == "ice") Plugin.BlockedRockAndTreeDropAtEndAge.Value.Split(',');
            if (Plugin.Era.Value == "end") return new List<string>();

            return Plugin.BlockedRockAndTreeDropAtStoneAge.Value.Split(',').Concat(Plugin.BlockedRockAndTreeDropAtBronzeAge.Value.Split(',').Concat(Plugin.BlockedRockAndTreeDropAtIronAge.Value.Split(',')).Concat(Plugin.BlockedRockAndTreeDropAtSilverAge.Value.Split(',')).Concat(Plugin.BlockedRockAndTreeDropAtBlackmetalAge.Value.Split(',')).Concat(Plugin.BlockedRockAndTreeDropAtMistAge.Value.Split(',').Concat(Plugin.BlockedRockAndTreeDropAtFireAge.Value.Split(',')).Concat(Plugin.BlockedRockAndTreeDropAtIceAge.Value.Split(',')).Concat(Plugin.BlockedRockAndTreeDropAtEndAge.Value.Split(',')))).ToList();
        }
    }
}
