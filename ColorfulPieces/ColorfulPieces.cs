using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Deadheim.ColorfulPieces
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class ColorfulPieces : BaseUnityPlugin
    {
        public const string PluginGUID = "redseiko.valheim.colorfulpieces";
        public const string PluginName = "ColorfulPieces";
        public const string PluginVersion = "1.1.0";

        private static readonly KeyboardShortcut _changeColorActionShortcut = new KeyboardShortcut(KeyCode.Y);
        private static readonly KeyboardShortcut _clearColorActionShortcut = new KeyboardShortcut(KeyCode.U);

        private static readonly int _pieceColorHashCode = "PieceColor".GetStableHashCode();
        private static readonly int _pieceEmissionColorFactorHashCode = "PieceEmissionColorFactor".GetStableHashCode();

        private class WearNTearData
        {
            public uint LastDataRevision { get; set; } = 0U;
            public List<Material> Materials { get; } = new List<Material>();
            public Color TargetColor { get; set; } = Color.clear;
            public float TargetEmissionColorFactor { get; set; } = 0f;

            public WearNTearData(WearNTear wearNTear)
            {
                Materials.AddRange(wearNTear.GetComponentsInChildren<MeshRenderer>(true).Select(r => r.material));
                Materials.AddRange(wearNTear.GetComponentsInChildren<SkinnedMeshRenderer>(true).Select(r => r.material));

                foreach (Material material in Materials)
                {
                    SaveMaterialColors(material);
                }
            }

            private static void SaveMaterialColors(Material material)
            {
                if (material.HasProperty("_Color"))
                {
                    material.SetColor("_SavedColor", material.GetColor("_Color"));
                }

                if (material.HasProperty("_EmissionColor"))
                {
                    material.SetColor("_SavedEmissionColor", material.GetColor("_EmissionColor"));
                }
            }
        }

        private static readonly Dictionary<WearNTear, WearNTearData> _wearNTearDataCache = new Dictionary<WearNTear, WearNTearData>();

        private static bool _isModEnabled = true;
        private static Color _targetPieceColor;
        private static string _targetPieceColorHex;
        private static float _targetPieceEmissionColorFactor = 0.4f;
        private static bool _showChangeRemoveColorPrompt = true;

        private static ManualLogSource _logger;

        private void Awake()
        {
            _targetPieceColor = Color.black;
            _targetPieceColorHex = ColorUtility.ToHtmlStringRGB(_targetPieceColor);
            _logger = Logger;
        }

        private void UpdateColorHexValue(object sender, EventArgs eventArgs)
        {
            Color color = _targetPieceColor;
            color.a = 1.0f; // Alpha transparency is unsupported.

            _targetPieceColorHex = $"#{ColorUtility.ToHtmlStringRGB(color)}";
            _targetPieceColor = color;
        }

        private void UpdateColorValue(object sender, EventArgs eventArgs)
        {
            if (ColorUtility.TryParseHtmlString(_targetPieceColorHex, out Color color))
            {
                color.a = 1.0f; 
                _targetPieceColor = color;
            }
        }

        public static void UpdateColorValue(string colorCode)
        {
            if (ColorUtility.TryParseHtmlString(colorCode, out Color color))
            {
                _targetPieceColorHex = colorCode;
                _targetPieceColor = color;
            }
        }

        private void Update()
        {
            if (!_isModEnabled || !Player.m_localPlayer || !Player.m_localPlayer.m_hovering)
            {
                return;
            }
            else if (_clearColorActionShortcut.IsDown())
            {
                ClearPieceColorAction(Player.m_localPlayer.m_hovering.GetComponentInParent<WearNTear>());
            }
            else if (_changeColorActionShortcut.IsDown())
            {
                ChangePieceColorAction(Player.m_localPlayer.m_hovering.GetComponentInParent<WearNTear>());
            }
        }

        private bool ClaimOwnership(WearNTear wearNTear)
        {
            if (!wearNTear
                || !wearNTear.m_nview
                || !wearNTear.m_nview.IsValid()
                || !PrivateArea.CheckAccess(wearNTear.transform.position, flash: true))
            {
                _logger.LogWarning("Piece does not have a valid ZNetView or is in a PrivateArea.");
                return false;
            }

            if (!wearNTear.m_nview.IsOwner())
            {
                wearNTear.m_nview.ClaimOwnership();
            }

            return true;
        }

        private void ChangePieceColorAction(WearNTear wearNTear)
        {
            if (!ClaimOwnership(wearNTear))
            {
                return;
            }

            wearNTear.m_nview.m_zdo.Set(_pieceColorHashCode, Utils.ColorToVec3(_targetPieceColor));
            wearNTear.m_nview.m_zdo.Set(_pieceEmissionColorFactorHashCode, _targetPieceEmissionColorFactor);

            if (_wearNTearDataCache.TryGetValue(wearNTear, out WearNTearData wearNTearData))
            {
                wearNTearData.TargetColor = _targetPieceColor;
                wearNTearData.TargetEmissionColorFactor = _targetPieceEmissionColorFactor;

                SetWearNTearColors(wearNTearData);
            }

            if (wearNTear.m_piece)
            {
                wearNTear.m_piece.m_placeEffect.Create(wearNTear.transform.position, wearNTear.transform.rotation);
            }
        }

        private void ClearPieceColorAction(WearNTear wearNTear)
        {
            if (!ClaimOwnership(wearNTear))
            {
                return;
            }

            if (wearNTear.m_nview.m_zdo.RemoveVec3(_pieceColorHashCode)
                || wearNTear.m_nview.m_zdo.RemoveFloat(_pieceEmissionColorFactorHashCode))
            {
                wearNTear.m_nview.m_zdo.IncreseDataRevision();
            }

            if (_wearNTearDataCache.TryGetValue(wearNTear, out WearNTearData wearNTearData))
            {
                wearNTearData.TargetColor = Color.clear;
                wearNTearData.TargetEmissionColorFactor = 0f;

                ClearWearNTearColors(wearNTearData);
            }

            if (wearNTear.m_piece)
            {
                wearNTear.m_piece.m_placeEffect.Create(wearNTear.transform.position, wearNTear.transform.rotation);
            }
        }

        [HarmonyPatch(typeof(WearNTear))]
        private class WearNTearPatch
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(WearNTear.Awake))]
            private static void WearNTearAwakePostfix(ref WearNTear __instance)
            {
                if (!_isModEnabled || !__instance)
                {
                    return;
                }

                _wearNTearDataCache[__instance] = new WearNTearData(__instance);
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(WearNTear.OnDestroy))]
            private static void WearNTearOnDestroyPrefix(ref WearNTear __instance)
            {
                _wearNTearDataCache.Remove(__instance);
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(WearNTear.UpdateWear))]
            private static void WearNTearUpdateWearPostfix(ref WearNTear __instance)
            {
                if (!_isModEnabled
                    || !__instance
                    || !__instance.m_nview
                    || __instance.m_nview.m_zdo == null
                    || __instance.m_nview.m_zdo.m_zdoMan == null
                    || __instance.m_nview.m_zdo.m_vec3 == null
                    || !_wearNTearDataCache.TryGetValue(__instance, out WearNTearData wearNTearData)
                    || wearNTearData.LastDataRevision >= __instance.m_nview.m_zdo.m_dataRevision)
                {
                    return;
                }

                if (__instance.m_nview.m_zdo.m_vec3.TryGetValue(_pieceColorHashCode, out Vector3 colorAsVector))
                {
                    wearNTearData.TargetColor = Utils.Vec3ToColor(colorAsVector);

                    if (__instance.m_nview.m_zdo.m_floats != null
                        && __instance.m_nview.m_zdo.m_floats.TryGetValue(_pieceEmissionColorFactorHashCode, out float factor))
                    {
                        wearNTearData.TargetEmissionColorFactor = factor;
                    }

                    SetWearNTearColors(wearNTearData);
                }
                else if (wearNTearData.TargetColor != Color.clear)
                {
                    wearNTearData.TargetColor = Color.clear;
                    wearNTearData.TargetEmissionColorFactor = 0f;

                    ClearWearNTearColors(wearNTearData);
                }

                wearNTearData.LastDataRevision = __instance.m_nview.m_zdo.m_dataRevision;
            }
        }

        [HarmonyPatch(typeof(Hud))]
        private class HudPatch
        {
            private static readonly string _hoverNameTextTemplate =
              "{0}{1}"
                  + "[<color={2}>{3}</color>] Mudar a cor para: <color=#{4}>#{4}</color> (f: <color=#{4}>{5}</color>)\n"
                  + "[<color={6}>{7}</color>] Limpar cor personalizada\n";

            [HarmonyPostfix]
            [HarmonyPatch(nameof(Hud.UpdateCrosshair))]
            private static void HudUpdateCrosshairPostfix(ref Hud __instance, ref Player player)
            {
                if (!_isModEnabled
                    || !_showChangeRemoveColorPrompt
                    || !__instance
                    || !player
                    || player != Player.m_localPlayer
                    || !player.m_hovering)
                {
                    return;
                }

                WearNTear wearNTear = player.m_hovering.GetComponentInParent<WearNTear>();

                if (!wearNTear || !wearNTear.m_nview || !wearNTear.m_nview.IsValid() || CustomSails.Plugin.instance.AllowInput())
                {
                    return;
                }

                __instance.m_hoverName.text =
                    string.Format(
                        _hoverNameTextTemplate,
                        __instance.m_hoverName.text,
                        __instance.m_hoverName.text.Length > 0 ? "\n" : string.Empty,
                        "#FFA726",
                        _changeColorActionShortcut,
                        ColorUtility.ToHtmlStringRGB(_targetPieceColor),
                        _targetPieceEmissionColorFactor.ToString("N2"),
                        "#EF5350",
                        _clearColorActionShortcut);
            }
        }

        private static void SetWearNTearColors(WearNTearData wearNTearData)
        {
            foreach (Material material in wearNTearData.Materials)
            {
                if (material.HasProperty("_EmissionColor"))
                {
                    material.SetColor("_EmissionColor", wearNTearData.TargetColor * wearNTearData.TargetEmissionColorFactor);
                }

                material.color = wearNTearData.TargetColor;
            }
        }

        private static void ClearWearNTearColors(WearNTearData wearNTearData)
        {
            foreach (Material material in wearNTearData.Materials)
            {
                if (material.HasProperty("_SavedEmissionColor"))
                {
                    material.SetColor("_EmissionColor", material.GetColor("_SavedEmissionColor"));
                }

                if (material.HasProperty("_SavedColor"))
                {
                    material.SetColor("_Color", material.GetColor("_SavedColor"));
                    material.color = material.GetColor("_SavedColor");
                }
            }
        }
    }

    internal static class ZDOExtensions
    {
        public static bool RemoveVec3(this ZDO zdo, int keyHashCode)
        {
            return zdo != null && zdo.m_vec3 != null && zdo.m_vec3.Remove(keyHashCode);
        }

        public static bool RemoveFloat(this ZDO zdo, int keyHashCode)
        {
            return zdo != null && zdo.m_floats != null && zdo.m_floats.Remove(keyHashCode);
        }
    }
}