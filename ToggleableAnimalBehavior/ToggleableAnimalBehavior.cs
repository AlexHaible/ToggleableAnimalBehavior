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
        }

        public override void WorldEvent_WorldUnloaded()
        {
            CanUnload = true;
        }

        public static void LogTree(Transform transform)
        {
            Debug.Log(GetLogTree(transform));
        }

        public static string GetLogTree(Transform transform, string prefix = " -", string step = "--")
        {
            string str = "\n" + prefix + transform.name;
            var c = transform.GetComponents<Component>();
            if (c.Length > 0)
                str += $": {c.Join(x => x.GetType().FullName)}";
            var r = transform.GetComponent<RectTransform>();
            if (r)
                str += $"\n{"".PadLeft(prefix.Length, ' ')}rect:[{r.rect.x},{r.rect.y},{r.rect.width},{r.rect.height}]\n{"".PadLeft(prefix.Length, ' ')}anchor:[{r.anchorMin.x},{r.anchorMin.y},{r.anchorMax.x},{r.anchorMax.y}]\n{"".PadLeft(prefix.Length, ' ')}offset:[{r.offsetMin.x},{r.offsetMin.y},{r.offsetMax.x},{r.offsetMax.y}]";
            foreach (Transform sub in transform)
                str += GetLogTree(sub, prefix + step);
            return str;
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

            GameModeValueManager.GetCurrentGameModeValue().sharkVariables.isTame = sharkIsTame;
            GameModeValueManager.GetCurrentGameModeValue().sharkVariables.attackRateMultiplier = sharkAtkMultiFloat;
            GameModeValueManager.GetCurrentGameModeValue().seagullVariables.isTame = seagullIsTame;
            GameModeValueManager.GetCurrentGameModeValue().seagullVariables.attacksCrops = seagullNoAtksCrops;
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