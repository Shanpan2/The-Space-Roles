﻿using AmongUs.GameOptions;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using BepInEx.Configuration;
using Il2CppSystem.Runtime.Remoting.Messaging;
using UnityEngine.UIElements;
using UnityEngine.XR;
using static TheSpaceRoles.Helper;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using Hazel;
using Reactor.Networking.Rpc;
using Steamworks;
using JetBrains.Annotations;

namespace TheSpaceRoles
{
    public static class CustomRoleOptions
    {
    }
    [Serializable]
    /// <summary>
    /// めっちゃTORからもってきました
    /// </summary>
    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
    class GameOptionsMenuStartPatch
    {
        public static void Postfix(GameOptionsMenu __instance)
        {
            var template = UnityEngine.Object.FindObjectsOfType<StringOption>().FirstOrDefault();

            if(template == null) { return; }
            if (__instance?.transform?.FindChild("TSRSettings") != null) { return; }
            if (__instance?.transform?.FindChild("CustomRoleSettings") != null) { return; }
            var gameSettings = GameObject.Find("Game Settings");
            var gameSettingMenu = UnityEngine.Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();

            var tsrSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var tsrMenu = tsrSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            tsrSettings.name = "TSRSettings";

            var customroleSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var customroleMenu = customroleSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            customroleSettings.name = "CustomRoleSettings";

            Logger.Info(customroleMenu?.ToString() ?? "null","CustomRoleSetting_GameGroup");

            var gameTab = GameObject.Find("GameTab");
            var roleTab = GameObject.Find("RoleTab");


            var tsrTab = UnityEngine.Object.Instantiate(roleTab, roleTab.transform.parent);
            var tsrTabHighlight = tsrTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            //tsrTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Sprites.GetSprite("TSRlogo.png", 100f);

            var customroleTab = UnityEngine.Object.Instantiate(roleTab, roleTab.transform.parent);
            var customroleTabHighlight = customroleTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            //customroleTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Sprites.GetSprite("TSRlogo.png", 100f);


            gameTab.transform.position += Vector3.left * 3.5f;
            roleTab.transform.position += Vector3.left * 3.75f;
            tsrTab.transform.position += Vector3.left * 2.75f;
            customroleTab.transform.position += Vector3.left * 1.75f;


            gameSettingMenu.RolesSettings.gameObject.SetActive(false);
            tsrSettings.gameObject.SetActive(false);
            customroleSettings.gameObject.SetActive(false);
            gameSettingMenu.RolesSettingsHightlight.enabled = false;
            tsrTabHighlight.enabled = false;
            customroleTabHighlight.enabled = false;

            if (GameOptionsManager.Instance.currentGameMode == GameModes.HideNSeek)
                gameSettingMenu.HideNSeekSettings.gameObject.SetActive(true);
            else
                gameSettingMenu.RegularGameSettings.SetActive(true);
            gameSettingMenu.GameSettingsHightlight.enabled = true;

            var tabs = new GameObject[] { gameTab, roleTab, tsrTab, customroleTab };
            for (int i = 0;i<tabs.Length;i++)
            {
                var tab = tabs[i];
                var button = tab.GetComponentInChildren<PassiveButton>();
                var copyindex = i;
                if (button == null) continue;
                button.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                button.OnClick.AddListener((System.Action)(() =>
                {
                    gameSettingMenu.RegularGameSettings.SetActive(false);
                    gameSettingMenu.RolesSettings.gameObject.SetActive(false);
                    gameSettingMenu.HideNSeekSettings.gameObject.SetActive(false);
                    tsrSettings.gameObject.SetActive(false);
                    customroleSettings.gameObject.SetActive(false);
                    gameSettingMenu.GameSettingsHightlight.enabled = false;
                    gameSettingMenu.RolesSettingsHightlight.enabled = false;
                    tsrTabHighlight.enabled = false;
                    customroleTabHighlight.enabled = false;

                    if (tsrMenu == null) Logger.Info("null", "tsrMenu");
                    if (customroleMenu == null) Logger.Info("null", "customroleMenu");

                    if (tsrTab == null) Logger.Info("null", "tsrTab");
                    if (customroleTab == null) Logger.Info("null", "customroleTab");

                    if (copyindex == 0)
                    {

                        Logger.Info($"open : {copyindex},{((GameObject.Find("TSRSettings") == null )? "null" : "TSRSettings!")}");

                        if (GameOptionsManager.Instance.currentGameMode == GameModes.HideNSeek)
                            gameSettingMenu.HideNSeekSettings.gameObject.SetActive(true);
                        else
                            gameSettingMenu.RegularGameSettings.SetActive(true);
                        gameSettingMenu.GameSettingsHightlight.enabled = true;
                    }else if (copyindex == 1)
                    {

                        Logger.Info($"open : {copyindex}");
                        gameSettingMenu.RolesSettings.gameObject.SetActive(true);
                        gameSettingMenu.RolesSettingsHightlight.enabled = true;
                    }
                    else if(copyindex == 2)
                    {
                        Logger.Info($"open : {copyindex}");
                        tsrSettings.gameObject.SetActive(true) ;
                        tsrTabHighlight.enabled = true;

                    }
                    else if(copyindex ==3)
                    {
                        Logger.Info($"open : {copyindex}");
                        customroleSettings.gameObject.SetActive(true);
                        customroleTabHighlight.enabled=true;
                    }
                }));
            }
            DestroyOptions([.. customroleMenu.GetComponentsInChildren<OptionBehaviour>()]);
            DestroyOptions([.. tsrMenu.GetComponentsInChildren<OptionBehaviour>()]);

            List<OptionBehaviour> tsrOptions = new List<OptionBehaviour>();

            GameObject tsrSliderInner = tsrSettings.transform.FindChild("GameGroup").FindChild("SliderInner").gameObject;
            CustomOptionsHolder.CreateCustomOptions();
            for(int i = 0; i < CustomOption.options.Count; i++)
            {
                var option = CustomOption.options[i];
                if (option.optionBehaviour == null)
                {
                    StringOption stringOption = UnityEngine.Object.Instantiate(template, tsrMenu.transform);
                    stringOption.OnValueChanged = new Action<OptionBehaviour>((o) => { });

                    stringOption.TitleText.text = option.GetName();
                    stringOption.Value = stringOption.oldValue = option.selection;
                    stringOption.ValueText.text = option.GetSelectionName();
                    option.optionBehaviour = stringOption;
                    stringOption.enabled = true;
                    
                    tsrOptions.Add(stringOption);
                }
                option.optionBehaviour.gameObject.SetActive(true);
            }





            SetOptions(tsrMenu,tsrOptions,tsrSettings);
            AdaptTaskCount(__instance);
        }
        private static void AdaptTaskCount(GameOptionsMenu __instance)
        {
            // Adapt task count for main options
            var commonTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumCommonTasks").TryCast<NumberOption>();
            if (commonTasksOption != null) commonTasksOption.ValidRange = new FloatRange(0f, 4f);

            var shortTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumShortTasks").TryCast<NumberOption>();
            if (shortTasksOption != null) shortTasksOption.ValidRange = new FloatRange(0f, 23f);

            var longTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumLongTasks").TryCast<NumberOption>();
            if (longTasksOption != null) longTasksOption.ValidRange = new FloatRange(0f, 15f);
        }
        private static void DestroyOptions(List<OptionBehaviour> optionBehavioursList)
        {
            foreach (OptionBehaviour option in optionBehavioursList)
                UnityEngine.Object.Destroy(option.gameObject);

        }
        private static void SetOptions(GameOptionsMenu menus, List<OptionBehaviour> options, GameObject settings)
        {
                menus.Children = options.ToArray();
                settings.gameObject.SetActive(false);
            for (int i = 0; i < options.Count; i++) 
            {
                var option = (StringOption)options[i];
                option.transform.localPosition = new Vector3(0,2f-0.5f*i,0);
                    
                    }

        }

    }
    [HarmonyPatch(typeof(GameSettingMenu),nameof(GameSettingMenu.Start))]
    class GameSettingStart
    {
        public static void Postfix(GameSettingMenu __instance) 
        {

        }


    }
    [HarmonyPatch(typeof(StringOption), nameof(StringOption.OnEnable))]
    public class StringOptionEnablePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return true;

            __instance.OnValueChanged = new Action<OptionBehaviour>((o) => { });
            __instance.TitleText.text = option.GetName();
            __instance.Value = __instance.oldValue = option.selection;
            __instance.ValueText.text = option.GetSelectionName();
            option.optionBehaviour = __instance;
            __instance.enabled = true;
            __instance.GetComponentInChildren<PassiveButton>().enabled = true;
            //__instance.Values = (Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray<StringNames>)Enum.GetValues(typeof(StringNames));
            return false;
        }
    }
    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Increase))]
    public class StringOptionIncreasePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return false;
            option.updateSelection(option.selection + 1);
            return false;
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Decrease))]
    public class StringOptionDecreasePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return false;
            option.updateSelection(option.selection - 1);
            return false;
        }
    }
    public class CustomOption
    {
        public static int preset = 0;
        public static List<CustomOption> options = [];
        public string GetName() => Translation.GetString("tsroption."+name);
        public string GetSelectionName() => Translation.GetString("tsroption.selection." + selections[selection]);

        public string name;
        public string parentId;
        public int selection;
        public int defaultSelection;
        public object[] selections;
        public ConfigEntry<int> entry;
        public Func
            <int, bool> func;
        public Action onChange;
        public OptionBehaviour optionBehaviour;

        public CustomOption(string name,
            object[] selections, object dafaultValue, string parentId = null, Func<int, bool> func = null, Action onChange = null
            )
        {
            this.name = name;
            this.func = func;
            this.onChange = onChange;
            this.selections = selections;
            int index = Array.IndexOf(selections, dafaultValue);
            selection = index;
            this.defaultSelection = index >= 0 ? index : 0;
            this.parentId = parentId;
            
            entry = TSR.Instance.Config.Bind<int>($"Preset{preset}", name, defaultSelection);

            options.Add(this);

        }
        public static CustomOption Create(string name, bool DefaultValue = false, string parentId = null, Func<int, bool> func = null, Action onChange = null)
        {
            return new CustomOption(name, ["Off", "On"], DefaultValue ? "On" : "Off", parentId, func, onChange);
        }
        public static Func<int, bool> OnOff =  x => x == 0  ;

        public static CustomOption Create(string name, string[] selections,string selection,string parentId =null, Func<int, bool> func = null, Action onChange = null)
        {
            return new CustomOption( name, selections, selection, parentId,func, onChange);
        }
        public void updateSelection(int newSelection)
        {

            var stringOption = this.optionBehaviour as StringOption;
            selection = Mathf.Clamp((newSelection + selections.Length) % selections.Length, 0, selections.Length - 1);
            stringOption.oldValue = stringOption.Value = selection;
            stringOption.ValueText.text = GetSelectionName();
            Logger.Info($"{name}:{selection}");
            ShareOptionSelections();
        }

        public static void ShareOptionSelections()
        {
            if (PlayerControl.AllPlayerControls.Count <= 1 || AmongUsClient.Instance?.AmHost == false && PlayerControl.LocalPlayer == null) return;

            MessageWriter writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)Rpcs.ShareOptions, Hazel.SendOption.Reliable);
            writer.WritePacked((uint)options.Count);
            foreach (CustomOption option in options)
            {
                writer.Write((uint)Convert.ToUInt32(option.selection));
            }
            writer.EndMessage();
            Reload();
        }
        public static void GetOptionSelections(MessageReader reader)
        {
            if (reader == null) return;
            
            uint count =  reader.ReadPackedUInt32();
            for (int i = 0; i < count; i++){
                options[i].selection = Convert.ToInt32(reader.ReadPackedUInt32());
            }
        }
        public static void Reload()
        {
            options.Where(x=>x.func!=null&&x.parentId != null)
                .Do(x => x.optionBehaviour.enabled = x.func(options.First(y => y.name == x.parentId).selection));
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSyncSettings))]
    public class RpcSyncSettingsPatch
    {
        public static void Postfix()
        {
            CustomOption.ShareOptionSelections();
        }
    }


}
