using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DeificGames.Profiler.Internal;

namespace DeificGames.Profiler
{
    public class EditorWindow_DGProfiler : EditorWindow
    {
        [MenuItem("Deific Games/Profiler")]
        static void InitializeWindow()
        {
            EditorWindow_DGProfiler window = (EditorWindow_DGProfiler)EditorWindow.GetWindow(typeof(EditorWindow_DGProfiler), false, "DG Profiler");
            window.Show();            
        }

        private static int roundToDecimalPlace = 4;

        private GUIContent totalRuns = new GUIContent("N", "Total execution count");
        private GUIContent totalTime = new GUIContent('\u03A3'.ToString(), "Total lifetime execution time");
        private GUIContent lowestTime = new GUIContent('\u2193'.ToString(), "Shortest time for any execution");
        private GUIContent highestTime = new GUIContent('\u2191'.ToString(), "Longest time for any execution");
        private GUIContent averageOverTen = new GUIContent('\u2286'.ToString(), "Average time over a set of 10 executions");
        private GUIContent averageTime = new GUIContent('\u00b5'.ToString(), "Average time for all executions");

        private void OnGUI()
        {

            EditorGUILayout.BeginHorizontal();
            DGProfilerTabsUtility.ExpandAllButton(this);
            EditorGUILayout.LabelField("Round", GUILayout.MaxWidth(45));
            roundToDecimalPlace = EditorGUILayout.IntSlider(roundToDecimalPlace, 0, 15);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();


            foreach (KeyValuePair<string, Internal.DGProfilerManager.Stopwatch> watches in DGProfilerManager.stopwatches) {
                Draw(watches.Value);
            }            
        }

        private void Draw(Internal.DGProfilerManager.Stopwatch sw)
        {
            if (DGProfilerTabsUtility.BeginFadeGroup(sw.name, this, out bool reset)) {
                Color currentColor = GUI.contentColor;
                EditorGUILayout.BeginHorizontal();
                GUI.contentColor = new Color(1, .35f, 0, 1);
                EditorGUILayout.LabelField(totalRuns, GUILayout.MaxWidth(25));
                EditorGUILayout.DoubleField(System.Math.Round(sw.runCount, roundToDecimalPlace), EditorStyles.boldLabel, GUILayout.MaxWidth(150));
                GUI.contentColor = Color.yellow;
                EditorGUILayout.LabelField(totalTime, GUILayout.MaxWidth(26));
                EditorGUILayout.DoubleField(System.Math.Round(sw.totalTime, roundToDecimalPlace), EditorStyles.boldLabel, GUILayout.MaxWidth(150));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();                
                GUI.contentColor = Color.green;
                EditorGUILayout.LabelField(lowestTime, GUILayout.MaxWidth(26));
                EditorGUILayout.DoubleField(System.Math.Round(sw.lowestTime, roundToDecimalPlace), EditorStyles.boldLabel, GUILayout.MaxWidth(150));
                GUI.contentColor = Color.cyan;
                EditorGUILayout.LabelField(averageTime, GUILayout.MaxWidth(26));
                EditorGUILayout.DoubleField(System.Math.Round(sw.averageTime, roundToDecimalPlace), EditorStyles.boldLabel, GUILayout.MaxWidth(150));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUI.contentColor = Color.red;
                EditorGUILayout.LabelField(highestTime, GUILayout.MaxWidth(26));
                EditorGUILayout.DoubleField(System.Math.Round(sw.highestTime, roundToDecimalPlace), EditorStyles.boldLabel, GUILayout.MaxWidth(150));
                GUI.contentColor = new Color(0, .5f, 1, 1);
                EditorGUILayout.LabelField(averageOverTen, GUILayout.MaxWidth(26));
                EditorGUILayout.DoubleField(System.Math.Round(sw.averageOverOneSecond, roundToDecimalPlace), EditorStyles.boldLabel, GUILayout.MaxWidth(150));
                EditorGUILayout.EndHorizontal();


                GUI.contentColor = currentColor;
            }
            DGProfilerTabsUtility.EndFadeGroup();

            if (reset) {
                sw.Reset();
            }
        }

        private void Update()
        {
            Repaint();
        }
    }
}
