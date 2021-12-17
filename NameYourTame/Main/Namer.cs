using HarmonyLib;
using NameYourTame.Utility;
using NameYourTame.Config;
using UnityEngine;
using NameYourTame.Utils;

namespace NameYourTame.Main
{
    class Namer
    {
        public static bool update;
        public static Character character;
        public static bool GUI;
        public static string NotChanged = "<b>[<color=yellow>" + NameYourTame_Config.ChangeName + "</color>]</b>" + " To Name Your Tame";

        [HarmonyPatch(typeof(Tameable), "Awake")]
        public static class CNTameAwake
        {
            public static void Postfix(ref Tameable __instance)
            {
                if (!NameYourTame_Config.foundMS)
                {
                    if (__instance != null)
                    {
                        if (__instance.m_nview.GetZDO().GetString("TameName") != "" && __instance.m_nview.GetZDO().GetString("TameName") != NotChanged && __instance.m_nview.GetZDO().GetString("TameName") != __instance.m_character.m_name && __instance.m_nview.GetZDO().GetString("TameName") != "Type in chat, /name TameName, to name your tame")
                        {
                            __instance.m_character.m_name = __instance.m_nview.GetZDO().GetString("TameName");
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Tameable), "GetHoverText")]
        public static class ShowTamingHoverText
        {
            public static void Postfix(ref string __result, ref Tameable __instance)
            {
                if (!NameYourTame_Config.foundMS)
                {
                    if (NameYourTame_Instances._player != null)
                    {
                        if (NameYourTame_Config.EnableMod)
                        {
                            if (__instance.m_character.IsTamed() && !__instance.m_character.IsPlayer())
                            {
                                if (__instance.m_nview.GetZDO().GetString("TameName", "") == "" || __instance.m_nview.GetZDO().GetString("TameName", "Type in chat, /name TameName, to name your tame") == "Type in chat, /name TameName, to name your tame")
                                {   
                                    __instance.m_nview.GetZDO().Set("TameName", NotChanged);
                                }

                                if (NameYourTame_Config.EnableHasToCrouch)
                                {
                                    if (NameYourTame_Instances._player.IsCrouching())
                                    {
                                        if (__instance.m_nview.GetZDO().GetString("TameName") == NotChanged)
                                        {
                                            NameReciever nameReciever = new NameReciever(ref __instance, __instance.m_character.m_name);
                                            if (Input.GetKey(KeyCode.N))
                                                if (!GUI)
                                                {
                                                    TextInput.instance.RequestText(nameReciever, "Name Your Tame", 32);
                                                    GUI = true;
                                                }
                                            __result += Localization.instance.Localize("\n<color=green>" + __instance.m_nview.GetZDO().GetString("TameName") + "</color>");
                                        }
                                    }
                                    else
                                    {
                                        if (__instance.m_nview.GetZDO().GetString("TameName") == NotChanged && !__result.Contains("Crouch"))
                                            __result += Localization.instance.Localize("\n<color=orange>Crouch to change the Tame name</color>");
                                    }
                                }
                                else
                                {
                                    if (__instance.m_nview.GetZDO().GetString("TameName") == NotChanged)
                                    {
                                        NameReciever nameReciever = new NameReciever(ref __instance, __instance.m_character.m_name);
                                        if (Input.GetKey(KeyCode.N))
                                            if (!GUI)
                                            {
                                                TextInput.instance.RequestText(nameReciever, "Name Your Tame", 32);
                                                GUI = true;
                                            }
                                        __result += Localization.instance.Localize("\n<color=green>" + __instance.m_nview.GetZDO().GetString("TameName") + "</color>");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(EnemyHud), "UpdateHuds")]
        public static class Test
        {
            public static void Postfix(ref EnemyHud __instance)
            {
                if (NameYourTame_Instances._player != null)
                {
                    if (__instance != null)
                    {
                        if (update)
                        {
                            EnemyHud.HudData hudData;
                            __instance.m_huds.TryGetValue(character, out hudData);
                            if (character.GetZDOID() == hudData.m_character.GetZDOID())
                                hudData.m_name.text = character.m_name;
                            update = false;
                        }
                    }
                }
            }
        }
    }
}
