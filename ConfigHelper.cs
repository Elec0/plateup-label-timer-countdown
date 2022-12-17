using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;
using UnityEngine;

namespace LabelCountdown
{
    public static class ConfigHelper
    {

        public static ConfigEntry<bool> WindowStartsExpanded;
        public static ConfigEntry<Vector2> WindowPosition;

        public static void SetupConfig(ConfigFile Config)
        {
            WindowStartsExpanded = Config.Bind("If window starts expanded", "ExpandedWindow", true,
                "If the window should start large or minimized, when the plugin is loaded.");

            WindowPosition = Config.Bind("Window starting position", "WindowPosition", new Vector2(20, 20), 
                "Coordinates at which the window is loaded.");
        }
    }
}
