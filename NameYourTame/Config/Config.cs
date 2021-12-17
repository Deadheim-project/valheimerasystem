using BepInEx;
using BepInEx.Configuration;
using BepInEx.Bootstrap;
using HarmonyLib;
using UnityEngine;

namespace NameYourTame.Config
{
    [BepInPlugin("NameYourTame.Config", "NameYourTame", "0.1.0")]
    public class NameYourTame_Config : BaseUnityPlugin
    {
        public static NameYourTame_Config Instance { get; private set; }

        public static bool EnableMod;

        public static bool EnableHasToCrouch;

        public static KeyCode ChangeName;

        public static bool foundMS;

        public void Awake()
        {
            Debug.Log("Loading NameYourTame...");

            NameYourTame_Config.Instance = this;

            EnableMod = true;
            EnableHasToCrouch = true;
            ChangeName = KeyCode.N;

            //Logs
            if (!EnableMod)
                Debug.LogWarning("[NameYourTame] Mod Disabled");
            else
            {
                Debug.Log("[NameYourTame] Mod Enabled");
                if (!EnableHasToCrouch)
                {
                    Debug.LogWarning("[NameYourTame] Has to Crouch Disabled");
                }
                else
                    Debug.Log("[NameYourTame] Has to Crouch Enabled");
            }

            Debug.Log("NameYourTame Loaded!");

            foreach (var plugin in Chainloader.PluginInfos)
            {
                var metadata = plugin.Value.Metadata;
                if (metadata.GUID.Contains("MoreSkills"))
                {
                    foundMS = true;
                    Debug.LogError("Found MoreSkills Mod, This Mod will be Deactivated.");
                    break;
                }
                else
                {
                    foundMS = false;
                }
            }
        }
    }
}

