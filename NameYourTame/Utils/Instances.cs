using HarmonyLib;

namespace NameYourTame.Utility
{
    public class NameYourTame_Instances
    {
        public static Player _player;
        public static Character _playerCharacter;

        [HarmonyPatch(typeof(Player), "UpdateStats")]
        public static class SI_Player
        {
            public static void Postfix(ref Player __instance)
            {
                if (__instance != null)
                    _player = __instance;
            }
        }

        [HarmonyPatch(typeof(Character), "UpdateGroundContact")]
        public static class SI_CPlyaer
        {
            public static void Postfix(ref Character __instance)
            {
                try
                {
                    if (__instance != null)
                    {
                        if (__instance.m_nview.GetZDO().GetString("playerName") == NameYourTame_Instances._player.GetPlayerName() && __instance.m_nview.GetZDO().GetLong("playerID") == NameYourTame_Instances._player.GetPlayerID())
                        {
                            if (_playerCharacter != __instance)
                            {
                                _playerCharacter = __instance;
                            }
                        }
                    }
                }
                catch
                {

                }
            }
        }
    }
}
