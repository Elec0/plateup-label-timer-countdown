using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using Kitchen;

namespace LabelCountdown.Patches
{
    [HarmonyPatch]
    public static class HandleRestaurantQuitEventPatch
    {

        [HarmonyPatch(typeof(HandleRestaurantQuitEvent), "OnUpdate")]
        public static void Postfix()
        {
            Plugin.Log.LogInfo("Restaurant quit patch called");

        }
    }
}
