using BepInEx.Logging;
using HarmonyLib;
using Kitchen;
using KitchenData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using Unity.Assertions;
using Unity.Collections;
using Unity.Entities;

using static LabelCountdown.Plugin;

namespace LabelCountdown.Patches
{
    [HarmonyPatch]
    public static class TrackLowestPatiencePatch
    {

        [HarmonyPatch(typeof(TrackLowestPatience), "OnUpdate")]
        public static void Postfix(TrackLowestPatience __instance)
        {
            
            EntityManager entityManager = __instance.EntityManager;
            // Get the pairs of CPatience and CCustomerSettings, we can calculate the time left from that.
            var patienceQuery = entityManager.
                CreateEntityQuery((ComponentType)typeof(CPatience), (ComponentType)typeof(CCustomerSettings));

            var indicatorQuery = entityManager.
                CreateEntityQuery((ComponentType)typeof(CustomerIndicatorView));

            
            //var indicator = indicatorQuery.ToComponentDataArray<CCustomerIndicator>(Allocator.TempJob);
            //var indicator = indicatorQuery.ToComponentDataArray<CustomerIndicatorView>(Allocator.TempJob);
            //foreach (var i in indicator)
            //{
            //    //Log.LogInfo(string.Format("{0}: {1}, ({2}), drink={3}", i.HasPatience, i.Patience, i.PatienceReason, i.WantsDrink));
            //}
            //indicatorQuery.ToEntityArray(Allocator.Temp).ToList().ForEach(e => Log.LogInfo(""));
            //entityManager.GetAllEntities().ToList().ForEach(e => entityManager.GetComponentTypes(e).ToList().ForEach(
            //ty => Log.LogInfo(string.Format("{0}: {1}", entityManager.GetComponentCount(e), string.Join(",", ty.GetManagedType().GenericTypeArguments.Select(x => x.Name + "[" + x.DeclaringType + "]"))
            //))));
            //Log.LogInfo(__instance.Time.ElapsedTime + ", " + __instance.ToString());

            if (true) return;
            if (!patienceQuery.IsEmpty)
            {
                var cPatience = patienceQuery.ToComponentDataArray<CPatience>(Allocator.TempJob);
                var cSettings = patienceQuery.ToComponentDataArray<CCustomerSettings>(Allocator.TempJob);

                // Hang on to all the values for the minimum one. There might be a more C#-ish way of doing this
                //  but I don't know how to do a multi-variable LINQ
                float minValue = float.MaxValue;
                string minReason = "Unknown reason";

                for (int i = 0; i < cPatience.Length; i++)
                {
                    if (ShouldSkipReason(cPatience[i].Reason))
                    {
                        continue;
                    }
                    // todo: Skip Thinking phase, it's always value 3
                    // Skip Eating

                    // Calculate the actual seconds left, as RemainingTime is between 0-1
                    float cur = cPatience[i].RemainingTime * cSettings[i].Patience[cPatience[i].Reason];
                    if (cur < minValue)
                    {
                        minValue = cur;
                        minReason = cPatience[i].Reason.ToString();
                    }
                }

                Log.LogInfo(string.Format("Lowest time: {0}, reason: {1}", minValue, minReason));

                //for (int i = 0; i < cPatience.Length; i++)
                //{
                //    CPatience patience = cPatience[i];
                //    CCustomerSettings settings = cSettings[i];
                //    Log.LogInfo("Time left: " + (patience.RemainingTime * settings.Patience[patience.Reason]) + ", " + patience.Reason);

                //}

                //cPatience.ToList().ForEach(
                //p => Log.LogInfo(string.Format("start: {0}, remaining: {1}, active: {2}", p.StartTime, p.RemainingTime, p.Active)));                
            }
        }

        private static bool ShouldSkipReason(PatienceReason reason)
        {
            switch (reason)
            {
                case PatienceReason.Thinking:
                case PatienceReason.Eating:
                    return true;
                default:
                    return false;
            }
        }
    }
}
