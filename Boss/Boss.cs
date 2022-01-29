using HarmonyLib;

namespace EraSystem.Boss
{
    public class Boss
    {

        [HarmonyPatch(typeof(OfferingBowl), "Interact")]
        public static class Interact
        {
            private static bool Prefix(OfferingBowl __instance, Humanoid user)
            {
                if (Validate(__instance.m_bossPrefab.name)) return true;

                Player.m_localPlayer.Message(MessageHud.MessageType.Center, "You don't know enough", 0, null);
                return false;
            }
        }

        [HarmonyPatch(typeof(OfferingBowl), "UseItem")]
        public static class UseItem
        {
            private static bool Prefix(OfferingBowl __instance, Humanoid user, ItemDrop.ItemData item)
            {
                if (Validate(__instance.m_bossPrefab.name)) return true;

                Player.m_localPlayer.Message(MessageHud.MessageType.Center, "You don't know enough", 0, null);
                return false;
            }
        }

        public static bool Validate(string bossName)
        {
            if (Plugin.Era.Value == "stone")
            {
                if (!Plugin.BlockedBossesAtStoneAge.Value.Contains(bossName)) return true;
            }
            else if (Plugin.Era.Value == "bronze")
            {
                if (!Plugin.BlockedBossesAtBronzeAge.Value.Contains(bossName)) return true;
            }
            else if (Plugin.Era.Value == "iron")
            {
                if (!Plugin.BlockedBossesAtIronAge.Value.Contains(bossName)) return true;
            }
            else if (Plugin.Era.Value == "silver")
            {
                if (!Plugin.BlockedBossesAtSilverAge.Value.Contains(bossName)) return true;
            }
            else if (Plugin.Era.Value == "blackmetal")
            {
                if (!Plugin.BlockedBossesAtBlackmetalAge.Value.Contains(bossName)) return true;
            }
            else if (Plugin.Era.Value == "mist")
            {
                if (!Plugin.BlockedBossesAtMistAge.Value.Contains(bossName)) return true;
            }
            else if (Plugin.Era.Value == "fire")
            {
                if (!Plugin.BlockedBossesAtFireAge.Value.Contains(bossName)) return true;
            }
            else if (Plugin.Era.Value == "ice")
            {
                if (!Plugin.BlockedBossesAtIceAge.Value.Contains(bossName)) return true;
            }
            else if (Plugin.Era.Value == "end")
            {
                if (!Plugin.BlockedBossesAtEndAge.Value.Contains(bossName)) return true;
            }

            return false;
        }
    }
}