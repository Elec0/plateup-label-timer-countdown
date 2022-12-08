using System;
using System.Collections.Generic;
using System.Text;
using LabelCountdown.Patches;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace LabelCountdown
{
    public class TimerGUI
    {

        private Rect windowRect = new Rect(20, 20, 200, 10);
        private bool collapseWindow = false;

        GUIStyle nonbreakingLabelStyle;

        public void OnGUI()
        {
            CustomerIndicatorViewPatch.PruneOldValues();

            nonbreakingLabelStyle = new GUIStyle();
            nonbreakingLabelStyle.wordWrap = false;
            nonbreakingLabelStyle.normal.textColor = Color.white;
            windowRect = GUILayout.Window(0, windowRect, WindowFunction, "Timers");

            //windowRectRes = GUILayout.Window(0, windowRectRes, ScalingWindow, "resizeable", GUILayout.MinHeight(80), GUILayout.MaxHeight(200));
        }

        private void WindowFunction(int windowID)
        {
            if (GUI.Button(new Rect(5, 3, 15, 15), "X"))
            {
                collapseWindow = !collapseWindow;
            }

            if (!collapseWindow)
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
                
                lst.ForEach(elem => GUILayout.Label($"{elem.patience}{(elem.isPercent ? "%" : "s")}, State: {elem.reason}", nonbreakingLabelStyle));
                
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
