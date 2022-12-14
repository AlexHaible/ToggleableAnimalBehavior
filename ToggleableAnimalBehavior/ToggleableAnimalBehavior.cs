using HarmonyLib;
using HMLLibrary;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace ToggleableAnimalBehavior {
    public class ToggleableAnimalBehavior : Mod
    {
        Harmony harmony;
        public static ToggleableAnimalBehavior instance;
        public static bool sharkIsTame;
        public static string sharkAtkMulti;
        public static float sharkAtkMultiFloat;
        public static bool sharkNoAttacksRaft;
        public static bool seagullIsTame;
        public static bool seagullNoAtksCrops;
        public static bool stonebirdIsTame;
        public static bool boarIsTame;
        public static bool butlerBotIsTame;
        public static bool bearIsTame;
        public static bool anglerfishIsTame;
        public static bool hyenaIsTame;
        public static bool pigIsTame;
        public static bool pufferfishIsTame;
        public static bool ratIsTame;
        public static bool utopiaBossIsTame;
        public static bool varunaBossIsTame;
        public static bool TABdebugging;
        static bool started;
        static bool CanUnload
        {
            get => !instance.modlistEntry.jsonmodinfo.isModPermanent;
            set
            {
                if (instance.modlistEntry.jsonmodinfo.isModPermanent == value)
                {
                    instance.modlistEntry.jsonmodinfo.isModPermanent = !value;
                    ModManagerPage.RefreshModState(instance.modlistEntry);
                }
            }
        }
        public ToggleableAnimalBehavior() : base()
        {
            instance = this;
        }
        public void Start()
        {
            started = false;
            var unloadButton = modlistEntry.modinfo.unloadBtn.GetComponent<Button>();
            if (SceneManager.GetActiveScene().name != Raft_Network.MenuSceneName)
            {
                unloadButton.onClick.Invoke();
                throw new ModLoadException("Mod must be loaded on the main menu");
            }
            started = true;
            (harmony = new Harmony("com.Sauravisus.ToggleableAnimalBehavior")).PatchAll();
            Log("Mod has been loaded!");
        }

        public void OnModUnload()
        {
            if (!started)
                return;
            harmony.UnpatchAll(harmony.Id);
            Log("Mod has been unloaded!");
        }

        public override void WorldEvent_WorldLoaded()
        {
            CanUnload = false;
            // Call the settings close function, so any variables set in the main menu
            // will take effect in the newly loaded world without needing to open/close
            // the mod options menu again.
            ExtraSettingsAPI_SettingsClose();
        }

        public override void WorldEvent_WorldUnloaded()
        {
            CanUnload = true;
        }

        public void ExtraSettingsAPI_Load() => ExtraSettingsAPI_SettingsClose();
        public void ExtraSettingsAPI_SettingsClose()
        {
            sharkIsTame         = ExtraSettingsAPI_GetCheckboxState("sharkIsTame");
            sharkAtkMulti       = ExtraSettingsAPI_GetInputValue("sharkAtkMulti");
            sharkAtkMultiFloat  = (float) Convert.ToDouble(sharkAtkMulti);
            sharkNoAttacksRaft  = ExtraSettingsAPI_GetCheckboxState("sharkNoAttacksRaft");
            seagullIsTame       = ExtraSettingsAPI_GetCheckboxState("seagullIsTame");
            seagullNoAtksCrops  = ExtraSettingsAPI_GetCheckboxState("seagullNoAtksCrops");
            stonebirdIsTame     = ExtraSettingsAPI_GetCheckboxState("stonebirdIsTame");
            boarIsTame          = ExtraSettingsAPI_GetCheckboxState("boarIsTame");
            butlerBotIsTame     = ExtraSettingsAPI_GetCheckboxState("butlerBotIsTame");
            bearIsTame          = ExtraSettingsAPI_GetCheckboxState("bearIsTame");
            anglerfishIsTame    = ExtraSettingsAPI_GetCheckboxState("anglerfishIsTame");
            hyenaIsTame         = ExtraSettingsAPI_GetCheckboxState("hyenaIsTame");
            pigIsTame           = ExtraSettingsAPI_GetCheckboxState("pigIsTame");
            pufferfishIsTame    = ExtraSettingsAPI_GetCheckboxState("pufferfishIsTame");
            ratIsTame           = ExtraSettingsAPI_GetCheckboxState("ratIsTame");
            utopiaBossIsTame    = ExtraSettingsAPI_GetCheckboxState("utopiaBossIsTame");
            varunaBossIsTame    = ExtraSettingsAPI_GetCheckboxState("varunaBossIsTame");
            TABdebugging        = ExtraSettingsAPI_GetCheckboxState("TABdebugging");

            if(TABdebugging == true){
                Log("--- Before saving creature aggression status variables ---");
                Log("Shark is tame: " + GameModeValueManager.GetCurrentGameModeValue().sharkVariables.isTame);
                Log("Shark attack rate multiplier: " + GameModeValueManager.GetCurrentGameModeValue().sharkVariables.attackRateMultiplier);
                Log("Seagull is tame: " + GameModeValueManager.GetCurrentGameModeValue().seagullVariables.isTame);
                Log("Seagull attacks crops: " + GameModeValueManager.GetCurrentGameModeValue().seagullVariables.attacksCrops);
                Log("Screecher is tame: " + GameModeValueManager.GetCurrentGameModeValue().stonebirdVariables.isTame);
                Log("Boar is tame: " + GameModeValueManager.GetCurrentGameModeValue().boarVariables.isTame);
                Log("Butler Bot is tame: " + GameModeValueManager.GetCurrentGameModeValue().butlerBotVariables.isTame);
                Log("Bear is tame: " + GameModeValueManager.GetCurrentGameModeValue().bearVariables.isTame);
                Log("Anglerfish is tame: " + GameModeValueManager.GetCurrentGameModeValue().anglerFishVariables.isTame);
                Log("Hyena is tame: " + GameModeValueManager.GetCurrentGameModeValue().hyenaVariables.isTame);
                Log("Mudhog is tame: " + GameModeValueManager.GetCurrentGameModeValue().pigVariables.isTame);
                Log("Pufferfish is tame: " + GameModeValueManager.GetCurrentGameModeValue().pufferfishVariables.isTame);
                Log("Lurker is tame: " + GameModeValueManager.GetCurrentGameModeValue().ratVariables.isTame);
                Log("Alpha is tame: " + GameModeValueManager.GetCurrentGameModeValue().utopiaBossVariables.isTame);
                Log("Rhino Shark is tame: " + GameModeValueManager.GetCurrentGameModeValue().varunaBossVariables.isTame);
            }

            GameModeValueManager.GetCurrentGameModeValue().sharkVariables.isTame = sharkIsTame;
            GameModeValueManager.GetCurrentGameModeValue().sharkVariables.attackRateMultiplier = sharkAtkMultiFloat;
            GameModeValueManager.GetCurrentGameModeValue().seagullVariables.isTame = seagullIsTame;
            GameModeValueManager.GetCurrentGameModeValue().seagullVariables.attacksCrops = !seagullNoAtksCrops;
            GameModeValueManager.GetCurrentGameModeValue().stonebirdVariables.isTame = stonebirdIsTame;
            GameModeValueManager.GetCurrentGameModeValue().boarVariables.isTame = boarIsTame;
            GameModeValueManager.GetCurrentGameModeValue().butlerBotVariables.isTame = butlerBotIsTame;
            GameModeValueManager.GetCurrentGameModeValue().bearVariables.isTame = bearIsTame;
            GameModeValueManager.GetCurrentGameModeValue().anglerFishVariables.isTame = anglerfishIsTame;
            GameModeValueManager.GetCurrentGameModeValue().hyenaVariables.isTame = hyenaIsTame;
            GameModeValueManager.GetCurrentGameModeValue().pigVariables.isTame = pigIsTame;
            GameModeValueManager.GetCurrentGameModeValue().pufferfishVariables.isTame = pufferfishIsTame;
            GameModeValueManager.GetCurrentGameModeValue().ratVariables.isTame = ratIsTame;
            GameModeValueManager.GetCurrentGameModeValue().utopiaBossVariables.isTame = utopiaBossIsTame;
            GameModeValueManager.GetCurrentGameModeValue().varunaBossVariables.isTame = varunaBossIsTame;

            if(TABdebugging == true){
                Log("--- After saving creature aggression status variables ---");
                Log("Shark is tame: " + GameModeValueManager.GetCurrentGameModeValue().sharkVariables.isTame);
                Log("Shark attack rate multiplier: " + GameModeValueManager.GetCurrentGameModeValue().sharkVariables.attackRateMultiplier);
                Log("Seagull is tame: " + GameModeValueManager.GetCurrentGameModeValue().seagullVariables.isTame);
                Log("Seagull attacks crops: " + GameModeValueManager.GetCurrentGameModeValue().seagullVariables.attacksCrops);
                Log("Screecher is tame: " + GameModeValueManager.GetCurrentGameModeValue().stonebirdVariables.isTame);
                Log("Boar is tame: " + GameModeValueManager.GetCurrentGameModeValue().boarVariables.isTame);
                Log("Butler Bot is tame: " + GameModeValueManager.GetCurrentGameModeValue().butlerBotVariables.isTame);
                Log("Bear is tame: " + GameModeValueManager.GetCurrentGameModeValue().bearVariables.isTame);
                Log("Anglerfish is tame: " + GameModeValueManager.GetCurrentGameModeValue().anglerFishVariables.isTame);
                Log("Hyena is tame: " + GameModeValueManager.GetCurrentGameModeValue().hyenaVariables.isTame);
                Log("Mudhog is tame: " + GameModeValueManager.GetCurrentGameModeValue().pigVariables.isTame);
                Log("Pufferfish is tame: " + GameModeValueManager.GetCurrentGameModeValue().pufferfishVariables.isTame);
                Log("Lurker is tame: " + GameModeValueManager.GetCurrentGameModeValue().ratVariables.isTame);
                Log("Alpha is tame: " + GameModeValueManager.GetCurrentGameModeValue().utopiaBossVariables.isTame);
                Log("Rhino Shark is tame: " + GameModeValueManager.GetCurrentGameModeValue().varunaBossVariables.isTame);
            }
        }

        public static bool ExtraSettingsAPI_GetCheckboxState(string name) => false;
        public static string ExtraSettingsAPI_GetInputValue(string name) => "";

        [HarmonyPatch(typeof(AI_State_Attack_Block_Shark), "FindBlockToAttack")]
        public static class raftAttackPatch
        {
            private static void Postfix(ref Block __result)
            {
                if (sharkNoAttacksRaft == true)
                {
                    __result = null;
                }
            }
        }
    }

    [HarmonyPatch(typeof(ModManagerPage), "ShowModInfo")]
    class Patch_ShowModInfo
    {
        static void Prefix(ModData md, ref bool? __state)
        {
            if (md.modinfo.mainClass && md.modinfo.mainClass.GetType() == typeof(ToggleableAnimalBehavior))
            {
                __state = md.jsonmodinfo.isModPermanent;
                md.jsonmodinfo.isModPermanent = false;
            }
            ModManagerPage.modInfoObj.transform.Find("MakePermanent").gameObject.SetActive(true);
        }
        static void Postfix(ModData md, bool? __state)
        {
            if (__state != null)
                md.jsonmodinfo.isModPermanent = __state.Value;
        }
    }
}
class ModLoadException : Exception
{
    public ModLoadException(string message) : base(message) { }
}