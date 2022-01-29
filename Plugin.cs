using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using ServerSync;
using ItemManager;
using UnityEngine;
using EraSystem.Drop;

namespace EraSystem
{
    [BepInPlugin(PluginGUID, PluginGUID, Version)]

    public class Plugin : BaseUnityPlugin
    {
        ConfigSync configSync = new ConfigSync(PluginGUID) { DisplayName = PluginGUID, CurrentVersion = Version, MinimumRequiredVersion = Version };

        public const string Version = "1.0.0";
        public const string PluginGUID = "Detalhes.EraSystem";
        public static ConfigEntry<string> Era;
        public static ConfigEntry<string> Tag;
        public static ConfigEntry<string> BlockedCraftsUntilStoneAge;
        public static ConfigEntry<string> BlockedCraftsUntilBronzeAge;
        public static ConfigEntry<string> BlockedCraftsUntilIronAge;
        public static ConfigEntry<string> BlockedCraftsUntilSilverAge;
        public static ConfigEntry<string> BlockedCraftsUntilBlackmetalAge;
        public static ConfigEntry<string> BlockedCraftsUntilFireAge;
        public static ConfigEntry<string> BlockedCraftsUntilMistAge;
        public static ConfigEntry<string> BlockedCraftsUntilIceAge;
        public static ConfigEntry<string> BlockedCraftsUntilEndAge;

        public static ConfigEntry<string> BlockedBossesAtStoneAge;
        public static ConfigEntry<string> BlockedBossesAtBronzeAge;
        public static ConfigEntry<string> BlockedBossesAtIronAge;
        public static ConfigEntry<string> BlockedBossesAtSilverAge;
        public static ConfigEntry<string> BlockedBossesAtBlackmetalAge;
        public static ConfigEntry<string> BlockedBossesAtFireAge;
        public static ConfigEntry<string> BlockedBossesAtMistAge;
        public static ConfigEntry<string> BlockedBossesAtIceAge;
        public static ConfigEntry<string> BlockedBossesAtEndAge;

        public static ConfigEntry<string> BlockedMetalsAtStoneAge;
        public static ConfigEntry<string> BlockedMetalsAtBronzeAge;
        public static ConfigEntry<string> BlockedMetalsAtIronAge;
        public static ConfigEntry<string> BlockedMetalsAtSilverAge;
        public static ConfigEntry<string> BlockedMetalsAtBlackmetalAge;
        public static ConfigEntry<string> BlockedMetalsAtFireAge;
        public static ConfigEntry<string> BlockedMetalsAtMistAge;
        public static ConfigEntry<string> BlockedMetalsAtIceAge;
        public static ConfigEntry<string> BlockedMetalsAtEndAge;

        public static ConfigEntry<string> BlockedCreatureDropAtStoneAge;
        public static ConfigEntry<string> BlockedCreatureDropAtBronzeAge;
        public static ConfigEntry<string> BlockedCreatureDropAtIronAge;
        public static ConfigEntry<string> BlockedCreatureDropAtSilverAge;
        public static ConfigEntry<string> BlockedCreatureDropAtBlackmetalAge;
        public static ConfigEntry<string> BlockedCreatureDropAtFireAge;
        public static ConfigEntry<string> BlockedCreatureDropAtMistAge;
        public static ConfigEntry<string> BlockedCreatureDropAtIceAge;
        public static ConfigEntry<string> BlockedCreatureDropAtEndAge;

        public static ConfigEntry<string> BlockedRockAndTreeDropAtStoneAge;
        public static ConfigEntry<string> BlockedRockAndTreeDropAtBronzeAge;
        public static ConfigEntry<string> BlockedRockAndTreeDropAtIronAge;
        public static ConfigEntry<string> BlockedRockAndTreeDropAtSilverAge;
        public static ConfigEntry<string> BlockedRockAndTreeDropAtBlackmetalAge;
        public static ConfigEntry<string> BlockedRockAndTreeDropAtFireAge;
        public static ConfigEntry<string> BlockedRockAndTreeDropAtMistAge;
        public static ConfigEntry<string> BlockedRockAndTreeDropAtIceAge;
        public static ConfigEntry<string> BlockedRockAndTreeDropAtEndAge;

        public static GameObject dontCraftPrefab;

        Harmony _harmony = new Harmony(PluginGUID);

        ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
        {
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = configSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        [HarmonyPatch(typeof(ConfigSync), "RPC_ConfigSync")]
        public static class RPC_ConfigSync
        {
            [HarmonyPriority(Priority.Last)]
            private static void Postfix()
            {
                if (ZNet.instance.IsServer()) return;
                ItemService.RemoveDisabledThings();
            }
        }

        [HarmonyPatch(typeof(Player), "OnSpawned")]
        public static class OnSpawned
        {
            [HarmonyPriority(Priority.Last)]
            private static void Postfix()
            {
                ItemService.RemoveDisabledThings();
            }
        }

        ConfigEntry<T> config<T>(string group, string name, T value, string description, bool synchronizedSetting = true) => config(group, name, value, new ConfigDescription(description), synchronizedSetting);
        private void Awake()
        {
            Config.SaveOnConfigSet = true;

            Era = config("Age Server config", "Age", "stone",
                       "Age: stone, bronze, iron, silver, blackmetal, mist, fire, ice, end");

            BlockedCraftsUntilStoneAge = config("Crafts Server config", "BlockedCraftsUntilStoneAge", "",
                "Stone");

            BlockedCraftsUntilBronzeAge = config("Crafts Server config", "BlockedCraftsUntilBronzeAge", "item_chest_bronze,item_legs_bronze,item_helmet_bronze,item_shield_bronzebuckler,item_mace_bronze,item_spear_bronze,item_sword_bronze,item_pickaxe_bronze,item_axe_bronze,item_atgeir_bronze,item_knife_copper,item_carrotsoup,Porridge,jam,Carrot Butter,item_meadbase,,Recipe_HelmetLeatherT2,Recipe_ArmorLeatherChestT2,Recipe_ArmorLeatherLegsT2,Recipe_ArmorRagsChestT2,Recipe_ArmorRagsLegsT2,Recipe_HelmetTrollLeatherT2,Recipe_ArmorTrollLeatherChestT2,Recipe_ArmorTrollLeatherLegsT2",
                "Bronze");

            BlockedCraftsUntilIronAge = config("Crafts Server config", "BlockedCraftsUntilIronAge", "iron,stonecutter,arrow_poison,huntsman,piece_workbench_ext4,piece_forge_ext3,sausage,draugr,ooze,Vial,Elixir,Flask,Potion,knifechitin,razor,banded,serpentscale,battleaxe,Recipe_HelmetLeatherT3,Recipe_ArmorLeatherChestT3,Recipe_ArmorLeatherLegsT3,Recipe_ArmorRagsChestT3,Recipe_ArmorRagsLegsT3,Recipe_HelmetTrollLeatherT3,Recipe_ArmorTrollLeatherChestT3,Recipe_ArmorTrollLeatherLegsT3,Recipe_HelmetBronzeT3,Recipe_ArmorBronzeChestT3,Recipe_ArmorBronzeLegsT3,Recipe_ArmorBarbarianBronzeHelmetJDT3,Recipe_ArmorBarbarianBronzeChestJDT3,Recipe_ArmorBarbarianBronzeLegsJDT3",
                "Iron");

            BlockedCraftsUntilSilverAge = config("Crafts Server config", "BlockedCraftsUntilSilverAge", "arrow_obsidian,arrow_frost,item_serpentstew,silver,drake,wolf,item_spear_wolffang,item_spear_ancientbark,Kebab,Smoked Fish,Pancakes,draugr,Omlette,Recipe_HelmetLeatherT4,Recipe_ArmorLeatherChestT4,Recipe_ArmorLeatherLegsT4,Recipe_ArmorRagsChestT4,Recipe_ArmorRagsLegsT4,Recipe_HelmetTrollLeatherT4,Recipe_ArmorTrollLeatherChestT4,Recipe_ArmorTrollLeatherLegsT4,Recipe_HelmetBronzeT4,Recipe_ArmorBronzeChestT4,Recipe_ArmorBronzeLegsT4,Recipe_HelmetIronT4,Recipe_ArmorIronChestT4,Recipe_ArmorIronLegsT4,Recipe_ArmorPlateIronHelmetJDT4,Recipe_ArmorPlateIronChestJDT4,Recipe_ArmorPlateIronLegsJDT4,Recipe_ArmorBarbarianBronzeHelmetJDT4,Recipe_ArmorBarbarianBronzeChestJDT4,Recipe_ArmorBarbarianBronzeLegsJDT4,dragomellete,rice,rk_acidcream,rk_electriccream,rk_firecream,rk_icecream",
                "Silver");

            BlockedCraftsUntilBlackmetalAge = config("Crafts Server config", "BlockedCraftsUntilBlackmetalAge", "artisan,windmill,arrow_needle,needle,lox,item_loxpie,item_fishwraps,item_bloodpudding,item_bread,Fish Stew,Blood Sausage,lox,spinning,blastfurnace,blackmetal,item_mace_needle,padded,Recipe_HelmetLeatherT5,Recipe_ArmorLeatherChestT5,Recipe_ArmorLeatherLegsT5,Recipe_ArmorRagsChestT5,Recipe_ArmorRagsLegsT5,Recipe_HelmetTrollLeatherT5,Recipe_ArmorTrollLeatherChestT5,Recipe_ArmorTrollLeatherLegsT5,Recipe_HelmetBronzeT5,Recipe_ArmorBronzeChestT5,Recipe_ArmorBronzeLegsT5,Recipe_HelmetIronT5,Recipe_ArmorIronChestT5,Recipe_ArmorIronLegsT5,Recipe_HelmetDrakeT5,Recipe_ArmorWolfChestT5,Recipe_ArmorWolfLegsT5,Recipe_ArmorPlateIronHelmetJDT5,Recipe_ArmorPlateIronChestJDT5,Recipe_ArmorPlateIronLegsJDT5,Recipe_ArmorBarbarianBronzeHelmetJDT5,Recipe_ArmorBarbarianBronzeChestJDT5,Recipe_ArmorBarbarianBronzeLegsJDT5",
                "Blackmetal");

            BlockedCraftsUntilMistAge = config("Crafts Server config", "BlockedCraftsUntilMistAge", "Recipe_HelmetLeatherT6,Recipe_ArmorLeatherChestT6,Recipe_ArmorLeatherLegsT6,Recipe_ArmorRagsChestT6,Recipe_ArmorRagsLegsT6,Recipe_HelmetTrollLeatherT6,Recipe_ArmorTrollLeatherChestT6,Recipe_ArmorTrollLeatherLegsT6,Recipe_HelmetBronzeT6,Recipe_ArmorBronzeChestT6,Recipe_ArmorBronzeLegsT6,Recipe_HelmetIronT6,Recipe_ArmorIronChestT6,Recipe_ArmorIronLegsT6,Recipe_HelmetDrakeT6,Recipe_ArmorWolfChestT6,Recipe_ArmorWolfLegsT6,Recipe_ArmorPlateIronHelmetJDT6,Recipe_ArmorPlateIronChestJDT6,Recipe_ArmorPlateIronLegsJDT6,Recipe_ArmorBarbarianBronzeHelmetJDT6,Recipe_ArmorBarbarianBronzeChestJDT6,Recipe_ArmorBarbarianBronzeLegsJDT6,Recipe_ArmorBlackmetalgarbHelmetT6,Recipe_ArmorBlackmetalgarbChestT6,Recipe_ArmorBlackmetalgarbLegsT6,Recipe_ArmorWandererChestT6,Recipe_ArmorWandererLegsT6,Recipe_ArmorWandererHelmetT6,Recipe_SageTunicBlackT6,Recipe_SageRobeBlackT6,Recipe_SageHoodBlackT6,Recipe_SageTunicBlueT6,Recipe_SageRobeBlueT6,Recipe_SageHoodBlueT6,Recipe_SageTunicBrownT6,Recipe_SageRobeBrownT6,Recipe_SageHoodBrownT6,Recipe_SageTunicGrayT6,Recipe_SageRobeGrayT6,Recipe_SageHoodGrayT6,Recipe_SageTunicGreenT6,Recipe_SageRobeGreenT6,Recipe_SageHoodGreenT6,Recipe_SageTunicRedT6,Recipe_SageRobeRedT6,Recipe_SageHoodRedT6,Recipe_SageTunicWhiteT6,Recipe_SageRobeWhiteT6,Recipe_SageHoodWhiteT6,piece_world_blueprint_rune_stack,piece_world_blueprint_run",
                 "Mist");

            BlockedCraftsUntilFireAge = config("Crafts Server config", "BlockedCraftsUntilFireAge", "",
                "Fire");

            BlockedCraftsUntilIceAge = config("Crafts Server config", "BlockedCraftsUntilIceAge", "",
                "Ice");

            BlockedCraftsUntilEndAge = config("Crafts Server config", "BlockedCraftsUntilEndAge", "",
                "End");

            BlockedBossesAtStoneAge = config("Bosses Server config", "BlockedBossesUntilStoneAge", "Eikthyr,gd_king,Bonemass,Dragonqueen,GoblinKing",
                "Stone");

            BlockedBossesAtBronzeAge = config("Bosses Server config", "BlockedBossesUntilBronzeAge", "gd_king,Bonemass,Dragonqueen,GoblinKing",
                "Bronze");

            BlockedBossesAtIronAge = config("Bosses Server config", "BlockedBossesAtIronAge", "Bonemass,Dragonqueen,GoblinKing",
                "Iron");

            BlockedBossesAtSilverAge = config("Bosses Server config", "BlockedBossesAtSilverAge", "Dragonqueen,GoblinKing",
                "Silver");

            BlockedBossesAtBlackmetalAge = config("Bosses Server config", "BlockedBossesUntilBlackmetalAge", "GoblinKing",
                "BlackMetal");

            BlockedBossesAtMistAge = config("Bosses Server config", "BlockedBossesUntilMistAge", "",
                "Mist");

            BlockedBossesAtFireAge = config("Bosses Server config", "BlockedBossesUntilFireAge", "",
                "Fire");

            BlockedBossesAtIceAge = config("Bosses Server config", "BlockedBossesUntilIceAge", "",
                "Ice");

            BlockedBossesAtEndAge = config("Bosses Server config", "BlockedBossesUntilEndAge", "",
                "End");

            BlockedMetalsAtStoneAge = config("Metals Server config", "BlockedMetalsUntilStoneAge", " tinore,bronzeore,ironscrap,silverore,blackmetalscrap,flametalore",
                "Stone");

            BlockedMetalsAtBronzeAge = config("Metals Server config", "BlockedMetalsUntilBronzeAge", "ironscrap,silverore,blackmetalscrap,flametalore",
                "Bronze");

            BlockedMetalsAtIronAge = config("Metals Server config", "BlockedMetalsAtIronAge", "silverore,blackmetalscrap,flametalore",
                "Iron");

            BlockedMetalsAtSilverAge = config("Metals Server config", "BlockedMetalsAtSilverAge", "blackmetalscrap,flametalore",
                "Silver");

            BlockedMetalsAtBlackmetalAge = config("Metals Server config", "BlockedMetalsUntilBlackmetalAge", "flametalore",
                "BlackMetal");

            BlockedMetalsAtMistAge = config("Metals Server config", "BlockedMetalsUntilMistAge", "flametalore",
                "Mist");

            BlockedMetalsAtFireAge = config("Metals Server config", "BlockedMetalsUntilFireAge", "flametalore",
                "Fire");

            BlockedMetalsAtIceAge = config("Metals Server config", "BlockedMetalsUntilIceAge", "",
                "Ice");

            BlockedMetalsAtEndAge = config("Metals Server config", "BlockedMetalsUntilEndAge", "",
                "End");

            BlockedCreatureDropAtStoneAge = config("CraetureDrop Server config", "BlockedCreatureDropUntilStoneAge", "",
          "Stone");

            BlockedCreatureDropAtBronzeAge = config("CraetureDrop Server config", "BlockedCreatureDropUntilBronzeAge", "gd_king,Greydwarf,Greydwarf_Elite,Greydwarf_Shaman,Skeleton,Skeleton_Poison",
                "Bronze");

            BlockedCreatureDropAtIronAge = config("CraetureDrop Server config", "BlockedCreatureDropUntilIronAge", "Bonemass,Blob,Ghost,Leech,Wraith,Draugr,Draugr_Ranged,Surtling,Troll",
                "Iron");

            BlockedCreatureDropAtSilverAge = config("CraetureDrop Server config", "BlockedCreatureDropUntilSilverAge", "Dragon,Draugr_Elite,BlobElite,Wolf,Fenring,Drake,Hatchling,StoneGolem",
                "Silver");

            BlockedCreatureDropAtBlackmetalAge = config("CraetureDrop Server config", "BlockedCreatureDropUntilBlackmetalAge", "GoblinKing,Fuling,Goblin,GoblinArcher,Serpent,Deathsquito,Lox,GoblinBrute,GoblinShaman",
                "BlackMetal");

            BlockedCreatureDropAtMistAge = config("CraetureDrop Server config", "BlockedCreatureDropUntilMistAge", "",
                "Mist");

            BlockedCreatureDropAtFireAge = config("CraetureDrop Server config", "BlockedCreatureDropUntilFireAge", "",
                "Fire");

            BlockedCreatureDropAtIceAge = config("CraetureDrop Server config", "BlockedCreatureDropUntilIceAge", "",
                "Ice");

            BlockedCreatureDropAtEndAge = config("CraetureDrop Server config", "BlockedCreatureDropUntilEndAge", "",
                "End");

            BlockedRockAndTreeDropAtStoneAge = config("RockAndTreeDrop Server config", "BlockedRockAndTreeDropUntilStoneAge", "rock4_copper,rock4_copper,MineRock_Tin,MineRock_Copper",
          "Stone");

            BlockedRockAndTreeDropAtBronzeAge = config("RockAndTreeDrop Server config", "BlockedRockAndTreeDropUntilBronzeAge", "Birch_log_half,Oak_log_half,OakStub,SwampTree1_log,SwampTree1_Stub,MineRock_Iron,GuckSack,GuckSack_small,OakStub,Leviathan,mudpile_frac,mudpile2_frac",
                "Bronze");

            BlockedRockAndTreeDropAtIronAge = config("RockAndTreeDrop Server config", "BlockedRockAndTreeDropUntilIronAge", "rock3_silver,rock3_silver_frac,silvervein_frac,silvervein,,MineRock_Obsidian,MineRock_Meteorite",
                "Iron");

            BlockedRockAndTreeDropAtSilverAge = config("RockAndTreeDrop Server config", "BlockedRockAndTreeDropUntilSilverAge", "tarlump1_frac,Pickable_Barley,Pickable_Flax,Pickable_Flax_Wild",
                "Silver");

            BlockedRockAndTreeDropAtBlackmetalAge = config("RockAndTreeDrop Server config", "BlockedRockAndTreeDropUntilBlackmetalAge", "",
                "BlackMetal");

            BlockedRockAndTreeDropAtMistAge = config("RockAndTreeDrop Server config", "BlockedRockAndTreeDropUntilMistAge", "",
                "Mist");

            BlockedRockAndTreeDropAtFireAge = config("RockAndTreeDrop Server config", "BlockedRockAndTreeDropUntilFireAge", "",
                "Fire");

            BlockedRockAndTreeDropAtIceAge = config("RockAndTreeDrop Server config", "BlockedRockAndTreeDropUntilIceAge", "",
                "Ice");

            BlockedRockAndTreeDropAtEndAge = config("RockAndTreeDrop Server config", "BlockedRockAndTreeDropUntilEndAge", "",
                "End");

            _harmony.PatchAll();

            var item = new Item("erasystem", "DontCraftStone");
            dontCraftPrefab = item.Prefab;
        }

    }
}
