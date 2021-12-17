using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EraSystem
{
    internal class ItemService
    { 
        public static void RemoveDisabledThings()
        {
            RemoveDisabledPieces();
            RemoveDisabledRecipes();
        }

        private static void RemoveDisabledPieces()
        {
            foreach (Piece piece in Resources.FindObjectsOfTypeAll(typeof(Piece)))
            {
                if (IsDisabled(piece.name))
                {
                    if (piece.m_resources.Any())
                    {
                        piece.m_resources[0].m_recover = false;
                        piece.m_resources[0].m_resItem = Plugin.dontCraftPrefab.GetComponent<ItemDrop>();
                    }
                }
            }
        }

        private static void RemoveDisabledRecipes()
        {
            var recipes = ObjectDB.instance.m_recipes;
            var dontCraftStone = Plugin.dontCraftPrefab;

            if (!recipes.Any()) return;

            foreach (Recipe recipe in recipes)
            {
                if (IsDisabled(recipe.name))
                {
                    if (recipe.m_resources.Any())
                    {
                        recipe.m_resources[0].m_resItem = dontCraftStone.GetComponent<ItemDrop>();
                    }
                }
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
            if (Plugin.Era.Value == "bronze") return Plugin.BlockedCraftsUntilIronAge.Value.Split(',').Concat(Plugin.BlockedCraftsUntilSilverAge.Value.Split(',')).Concat(Plugin.BlockedCraftsUntilBlackmetalAge.Value.Split(',')).Concat(Plugin.BlockedCraftsUntilMistAge.Value.Split(',').Concat(Plugin.BlockedCraftsUntilFireAge.Value.Split(',')).Concat(Plugin.BlockedCraftsUntilIceAge.Value.Split(',')).Concat(Plugin.BlockedCraftsUntilEndAge.Value.Split(','))).ToList();
            if (Plugin.Era.Value == "iron") return (Plugin.BlockedCraftsUntilSilverAge.Value.Split(',')).Concat(Plugin.BlockedCraftsUntilBlackmetalAge.Value.Split(',')).Concat(Plugin.BlockedCraftsUntilMistAge.Value.Split(',').Concat(Plugin.BlockedCraftsUntilFireAge.Value.Split(',')).Concat(Plugin.BlockedCraftsUntilIceAge.Value.Split(',')).Concat(Plugin.BlockedCraftsUntilEndAge.Value.Split(','))).ToList();
            if (Plugin.Era.Value == "silver") return (Plugin.BlockedCraftsUntilBlackmetalAge.Value.Split(',')).Concat(Plugin.BlockedCraftsUntilMistAge.Value.Split(',').Concat(Plugin.BlockedCraftsUntilFireAge.Value.Split(',')).Concat(Plugin.BlockedCraftsUntilIceAge.Value.Split(',')).Concat(Plugin.BlockedCraftsUntilEndAge.Value.Split(','))).ToList();
            if (Plugin.Era.Value == "blackmetal") return (Plugin.BlockedCraftsUntilMistAge.Value.Split(',').Concat(Plugin.BlockedCraftsUntilFireAge.Value.Split(',')).Concat(Plugin.BlockedCraftsUntilIceAge.Value.Split(',')).Concat(Plugin.BlockedCraftsUntilEndAge.Value.Split(','))).ToList();
            if (Plugin.Era.Value == "mist") Plugin.BlockedCraftsUntilFireAge.Value.Split(',').Concat(Plugin.BlockedCraftsUntilIceAge.Value.Split(',')).Concat(Plugin.BlockedCraftsUntilEndAge.Value.Split(','));
            if (Plugin.Era.Value == "fire") Plugin.BlockedCraftsUntilIceAge.Value.Split(',').Concat(Plugin.BlockedCraftsUntilEndAge.Value.Split(','));
            if (Plugin.Era.Value == "ice") Plugin.BlockedCraftsUntilEndAge.Value.Split(',');
            if (Plugin.Era.Value == "end") return new List<string>();

            return Plugin.BlockedCraftsUntilBronzeAge.Value.Split(',').Concat(Plugin.BlockedCraftsUntilIronAge.Value.Split(',')).Concat(Plugin.BlockedCraftsUntilSilverAge.Value.Split(',')).Concat(Plugin.BlockedCraftsUntilBlackmetalAge.Value.Split(',')).Concat(Plugin.BlockedCraftsUntilMistAge.Value.Split(',').Concat(Plugin.BlockedCraftsUntilFireAge.Value.Split(',')).Concat(Plugin.BlockedCraftsUntilIceAge.Value.Split(',')).Concat(Plugin.BlockedCraftsUntilEndAge.Value.Split(','))).ToList();
        }
    }
}
