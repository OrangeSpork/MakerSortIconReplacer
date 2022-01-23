using BepInEx;
using BepInEx.Configuration;
using CharaCustom;
using KKAPI;
using KKAPI.Maker;
using KKAPI.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MakerSortIconReplacer
{
    [BepInPlugin(GUID, PluginName, Version)]
    [BepInDependency(KoikatuAPI.GUID, KoikatuAPI.VersionConst)]
#if HS2
    [BepInProcess("HoneySelect2.exe")]
#else
    [BepInProcess("AI-Syoujyo.exe")]
#endif
    public class MakerSortIconReplacer : BaseUnityPlugin
    {
        public const string GUID = "orange.spork.makersorticonreplacer";
        public const string PluginName = "MakerSortIconReplacer";
        public const string Version = "1.0.0";

        public static ConfigEntry<bool> ReplaceSortIcons { get; set; }

        public static MakerSortIconReplacer Instance { get; set; }

        internal BepInEx.Logging.ManualLogSource Log => Logger;

        public MakerSortIconReplacer()
        {
            if (Instance != null)
                throw new InvalidOperationException("Singleton only.");

            Instance = this;

            ReplaceSortIcons = Config.Bind("Options", "Replace Sort Icons", true, "Replaces Illusion Sort Controls with Custom Set");

#if DEBUG
            Log.LogInfo("Jump to Maker Loaded.");
#endif
        }

        public void Start()
        {
            MakerAPI.MakerBaseLoaded += MakerLoaded;


        }

        private void MakerLoaded(object sender, RegisterCustomControlsEvent eventArgs)
        {
            if (ReplaceSortIcons.Value)
                StartCoroutine(OnMakerLoading());
        }

        private static MakerSortWindowHelper saveCharaHelper, loadCharaHelper, saveClothesHelper, loadClothesHelper;

        private static IEnumerator OnMakerLoading()
        {
            yield return new WaitUntil(() => GameObject.Find("CharaCustom/CustomControl/CanvasSub/SettingWindow/WinClothes/DefaultWin/C_Clothes/Setting/Setting01/SelectBox/Scroll View") != null);

            CustomCharaWindow saveWindow = GameObject.Find("CharaCustom/CustomControl/CanvasSub/SettingWindow/WinOption/SystemWin/O_SaveDelete").GetComponent<CustomCharaWindow>();
            GameObject saveSortToggle = GameObject.Find("CharaCustom/CustomControl/CanvasSub/SettingWindow/WinOption/SystemWin/O_SaveDelete/menu/tglSort");
            GameObject saveSortMethodToggle = GameObject.Find("CharaCustom/CustomControl/CanvasSub/SettingWindow/WinOption/SystemWin/O_SaveDelete/menu/btnSortCategory");

            saveCharaHelper = new MakerSortWindowHelper();
            saveCharaHelper.SetupButtons(saveWindow, saveSortToggle, saveSortMethodToggle);

            CustomCharaWindow loadWindow = GameObject.Find("CharaCustom/CustomControl/CanvasSub/SettingWindow/WinOption/SystemWin/O_Load").GetComponent<CustomCharaWindow>();
            GameObject loadSortToggle = GameObject.Find("CharaCustom/CustomControl/CanvasSub/SettingWindow/WinOption/SystemWin/O_Load/menu/tglSort");
            GameObject loadSortMethodToggle = GameObject.Find("CharaCustom/CustomControl/CanvasSub/SettingWindow/WinOption/SystemWin/O_Load/menu/btnSortCategory");

            loadCharaHelper = new MakerSortWindowHelper();
            loadCharaHelper.SetupButtons(loadWindow, loadSortToggle, loadSortMethodToggle);

            CustomClothesWindow saveClothesWindow = GameObject.Find("CharaCustom/CustomControl/CanvasSub/SettingWindow/WinClothes/SystemWin/C_SaveDelete").GetComponent<CustomClothesWindow>();
            GameObject saveClothesSortToggle = GameObject.Find("CharaCustom/CustomControl/CanvasSub/SettingWindow/WinClothes/SystemWin/C_SaveDelete/menu/tglSort");
            GameObject saveClothesSortMethodToggle = GameObject.Find("CharaCustom/CustomControl/CanvasSub/SettingWindow/WinClothes/SystemWin/C_SaveDelete/menu/btnSortCategory");

            saveClothesHelper = new MakerSortWindowHelper();
            saveClothesHelper.SetupClothesButtons(saveClothesWindow, saveClothesSortToggle, saveClothesSortMethodToggle);

            CustomClothesWindow loadClothesWindow = GameObject.Find("CharaCustom/CustomControl/CanvasSub/SettingWindow/WinClothes/SystemWin/C_Load").GetComponent<CustomClothesWindow>();
            GameObject loadClothesSortToggle = GameObject.Find("CharaCustom/CustomControl/CanvasSub/SettingWindow/WinClothes/SystemWin/C_Load/menu/tglSort");
            GameObject loadClothesSortMethodToggle = GameObject.Find("CharaCustom/CustomControl/CanvasSub/SettingWindow/WinClothes/SystemWin/C_Load/menu/btnSortCategory");

            loadClothesHelper = new MakerSortWindowHelper();
            loadClothesHelper.SetupClothesButtons(loadClothesWindow, loadClothesSortToggle, loadClothesSortMethodToggle);
        }

        
    }
}
