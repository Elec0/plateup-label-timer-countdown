using System;
using System.Collections.Concurrent;
using HarmonyLib;
using Kitchen;
using KitchenData;
using Newtonsoft.Json;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace LabelCountdown.Patches
{
    [HarmonyPatch]

    public static partial class CustomerIndicatorViewPatch
    {
        /// <summary>
        /// So we can get the data out of this update method into the OnGUI method. 
        /// Not sure how you're supposed to do it, but this should work.
        /// </summary>
        public static ConcurrentDictionary<int, PatienceContainer> DataMap = new();

        

        /// <summary>
        /// Update is called even when the game is paused, so we can use it to clean up old values, since they won't remove themselves 
        /// from the map, and we can't rely on easy cleanup.
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(typeof(CustomerIndicatorView), "Update")]
        public static void Postfix(CustomerIndicatorView __instance)
        {
            //SetupText(__instance);
            //Plugin.Log.LogInfo("CustomerIndicator Update " + Time.time);
            PruneOldValues();
        }

        [HarmonyPatch(typeof(CustomerIndicatorView), "UpdateData")]
        public static void Postfix(CustomerIndicatorView __instance, ref CustomerIndicatorView.ViewData view_data)
        {
            // First thing remove the entry
            DataMap.TryRemove(__instance.GetInstanceID(), out _);

            // This query might be too slow for running in every single UpdateData call, I'm not sure.
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var patienceQuery = entityManager.
                CreateEntityQuery((ComponentType)typeof(CCustomerSettings));
            var cSettings = patienceQuery.ToComponentDataArray<CCustomerSettings>(Allocator.TempJob);
            
            float time = view_data.Patience * cSettings[0][view_data.PatienceReason];
            
            //Plugin.Log.LogInfo(view_data.Patience+ ", " + view_data.PatienceReason + ", " + cSettings[0].Patience);
            //Plugin.Log.LogInfo(time + ", " + view_data.PatienceReason + ", @" + Time.time);
            if (ShouldLogPatience(time, view_data.PatienceReason))
            {
                bool isQ = IsQueue(view_data.PatienceReason);
                if(isQ) // It's a percent instead
                {
                    time = view_data.Patience;
                }

                DataMap.TryAdd(__instance.GetInstanceID(), 
                    new PatienceContainer(time, view_data.PatienceReason, Time.time, isQ));
            }
        }

        /// <summary>
        /// Determine if we should include this event in the display log.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        private static bool ShouldLogPatience(float time, PatienceReason reason)
        {            
            // When they leave it gets set to 0, and otherwise the game's over anyway so it doesn't matter.
            if (time == 0 && !IsQueue(reason))
            {
                return false;
            }
            if(reason == PatienceReason.Eating || reason == PatienceReason.Thinking)
            {
                return false;
            }
            return true;
        }

        public static bool IsQueue(PatienceReason reason)
        {
            return reason switch
            {
                PatienceReason.Queue => true,
                PatienceReason.QueueInSnow => true,
                PatienceReason.QueueInRain => true,
                PatienceReason.QueueInDarkness => true,
                _ => false
            };
        }

        public static void PruneOldValues()
        {
            foreach(var item in DataMap)
            {
                if(Math.Abs(Time.time - item.Value.lastUpdateTime) >= Plugin.PATIENCE_PRUNE_TIME)
                {
                    DataMap.TryRemove(item.Key, out _);
                    Plugin.Log.LogDebug("Pruned " + item.Value.patience + ", " + item.Value.reasonName);
                }
            }
        }

        /// <summary>
        /// WIP create 3D text for timer
        /// </summary>
        /// <param name="__instance"></param>
        /// <returns></returns>
        private static TextMeshPro SetupText(CustomerIndicatorView __instance)
        {
            Plugin.Log.LogInfo("SetupText");
            var text = __instance.gameObject.AddComponent<TextMeshPro>();

            text.name = "e0textname";
            StyleText(text);

            return text;
        }

        /// <summary>
        /// WIP update 3D text above/next to the indicator instead of on a GUI
        /// </summary>
        /// <param name="text"></param>
        private static void StyleText(TextMeshPro text)
        {
            Plugin.Log.LogInfo("StyleText");

            text.fontSizeMin = 18;
            text.fontSizeMax = 72;
            text.horizontalAlignment = HorizontalAlignmentOptions.Center;
            text.verticalAlignment = VerticalAlignmentOptions.Middle;
            text.alignment = TextAlignmentOptions.Center;

            //Plugin.Log.LogInfo(text.rectTransform.localScale.x + ", " + text.rectTransform.localScale.y + ", " + text.rectTransform.localScale.z + ", " + text.rectTransform.localScale.ToString());
            text.rectTransform.localScale = UnityEngine.Vector3.one;
            Plugin.Log.LogInfo(text.rectTransform.sizeDelta.ToString());
            //text.rectTransform.position.Set(-2.61f, 1.61f, -9.02f);
            text.rectTransform.sizeDelta = new UnityEngine.Vector2(127.86f, 46.80f);
            Plugin.Log.LogInfo(text.rectTransform.anchoredPosition3D.ToString());
            text.rectTransform.anchoredPosition3D = new UnityEngine.Vector3(-0.68f, 1, 0.01f);
            //text.useGUILayout = false;

        }
    }
}
