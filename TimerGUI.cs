using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;
using LabelCountdown.Patches;
using UnityEngine;
using static LabelCountdown.ConfigHelper;

namespace LabelCountdown
{
    public class TimerGUI
    {
        private const int GUI_ID = 976625;

        private Rect windowRect;

        private GUIStyle nonbreakingLabelStyle;


        public TimerGUI()
        {
            nonbreakingLabelStyle = new GUIStyle();
            nonbreakingLabelStyle.wordWrap = false;
            nonbreakingLabelStyle.normal.textColor = Color.white;
            windowRect = new Rect(WindowPosition.Value.x, WindowPosition.Value.y, 200, 10);
        }

        public void OnGUI()
        {
            CustomerIndicatorViewPatch.PruneOldValues();
            WindowPosition.Value = new Vector2(windowRect.x, windowRect.y);

            windowRect = GUILayout.Window(GUI_ID, windowRect, WindowFunction, "Timers");
        }

        private void WindowFunction(int windowID)
        {
            if (GUI.Button(new Rect(5, 3, 15, 15), "X"))
            {
                WindowStartsExpanded.Value = !WindowStartsExpanded.Value;
            }

            if (WindowStartsExpanded.Value)
            {
                GUILayout.BeginVertical();
                List<PatienceContainer> lst = new();

                foreach (var el in CustomerIndicatorViewPatch.DataMap.Values)
                {
                    // Only have decimal points once we pass 10s
                    // Also don't have decimals when it's a whole number. 3.00s isn't very helpful.
                    float time;

                    if (!el.isPercent)
                    {
                        time = (float)((el.patience < 10 && el.patience % 1 != 0) ? Math.Round(el.patience, 2) : Math.Round(el.patience, 0));
                    }
                    else
                    {
                        time = (float)Math.Round(el.patience * 100, 0);
                    }
                    lst.Add(new PatienceContainer(time, el.reason, el.lastUpdateTime, el.isPercent));
                }
                lst.Sort(delegate (PatienceContainer one, PatienceContainer two)
                    {
                        return one.patience.CompareTo(two.patience);
                    });

                lst.ForEach(elem => GUILayout.Label($"{elem.patience}{(elem.isPercent ? "%" : "s")}, {elem.reasonName}", nonbreakingLabelStyle));

                // Need to add something if the map is empty, or it won't expand.
                if (CustomerIndicatorViewPatch.DataMap.IsEmpty)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(5);
                    GUILayout.Label("<Empty>", nonbreakingLabelStyle);
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            // Insert a huge dragging area at the end.
            // This gets clipped to the window (like all other controls) so you can never
            //  drag the window from outside it.
            GUI.DragWindow(new Rect(0, 0, 500, 500));
        }
    }
}
