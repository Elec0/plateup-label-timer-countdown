using BepInEx;
using HarmonyLib.Tools;
using HarmonyLib;
using System.Reflection;
using BepInEx.Logging;
using UnityEngine;
using System;
using Unity.Entities;

namespace LabelCountdown
{

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        /// <summary>
        /// How many seconds an entry needs to be before it's deleted.
        /// </summary>
        public const float PATIENCE_PRUNE_TIME = 1;

        internal static ManualLogSource Log;

        private static Harmony _plugin;
        private TimerGUI timerGUI = new TimerGUI();

        private void Awake()
        {
            Log = Logger;

            _plugin = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginInfo.PLUGIN_GUID);
            HarmonyFileLog.Enabled = true;
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        // TODO: Hook into GameOver, or quit or something, to figure out when to clear shit
        // TODO: Add a clear button

        public void FixedUpdate()
        {
            //var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            // CGamePauseRequest
            //Log.LogInfo("FixedUpdate");
        }

        public void OnGUI()
        {
            timerGUI.OnGUI();
        }
 
        public void OnDestroy()
        {
            _plugin?.UnpatchSelf();
        }
    }
}
