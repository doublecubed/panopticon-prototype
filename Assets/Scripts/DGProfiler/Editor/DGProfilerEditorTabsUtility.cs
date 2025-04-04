using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System;

namespace DeificGames.Profiler.Internal
{
    public static class DGProfilerTabsUtility
    {
        private static Dictionary<Type, Dictionary<string, AnimBool>> inspectedObjects = new Dictionary<Type, Dictionary<string, AnimBool>>();

        public static void ExpandAllButton(Editor editor)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.MaxHeight(15));
            //GUILayout.FlexibleSpace();

            GUILayout.Space(Screen.width - 200);

            GUILayoutOption[] options = new GUILayoutOption[]
            {
            GUILayout.MaxWidth(90),
            GUILayout.MaxHeight(15)
            };

            if (GUILayout.Button("Expand All", EditorStyles.toolbarDropDown, options)) {
                foreach (KeyValuePair<Type, Dictionary<string, AnimBool>> dicts in inspectedObjects) {
                    foreach (KeyValuePair<string, AnimBool> animBool in dicts.Value) {
                        animBool.Value.value = true;
                    }
                }
                editor.Repaint();
            }

            if (GUILayout.Button("Collapse All", EditorStyles.toolbarDropDown, options)) {
                foreach (KeyValuePair<Type, Dictionary<string, AnimBool>> dicts in inspectedObjects) {
                    foreach (KeyValuePair<string, AnimBool> animBool in dicts.Value) {
                        animBool.Value.value = false;
                    }
                }
                editor.Repaint();
            }
            GUILayout.Space(5);
            EditorGUILayout.EndHorizontal();

        }

        public static void ExpandAllButton(EditorWindow editor)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.MaxHeight(15));
            //GUILayout.FlexibleSpace();

            //GUILayout.Space(Screen.width - 200);

            GUILayoutOption[] options = new GUILayoutOption[]
            {
            GUILayout.MaxWidth(90),
            GUILayout.MaxHeight(15)
            };

            if (GUILayout.Button("Expand All", EditorStyles.toolbarDropDown, options)) {
                foreach (KeyValuePair<Type, Dictionary<string, AnimBool>> dicts in inspectedObjects) {
                    foreach (KeyValuePair<string, AnimBool> animBool in dicts.Value) {
                        animBool.Value.value = true;
                    }
                }
                editor.Repaint();
            }

            if (GUILayout.Button("Collapse All", EditorStyles.toolbarDropDown, options)) {
                foreach (KeyValuePair<Type, Dictionary<string, AnimBool>> dicts in inspectedObjects) {
                    foreach (KeyValuePair<string, AnimBool> animBool in dicts.Value) {
                        animBool.Value.value = false;
                    }
                }
                editor.Repaint();
            }
            GUILayout.Space(5);
            EditorGUILayout.EndHorizontal();

        }

        public static bool BeginFadeGroup(string label, EditorWindow editorWindow, out bool rightButtonTriggered)
        {
            if (!inspectedObjects.TryGetValue(editorWindow.GetType(), out Dictionary<string, AnimBool> tempStringDictionary)) {
                tempStringDictionary = new Dictionary<string, AnimBool>();
                inspectedObjects.Add(editorWindow.GetType(), tempStringDictionary);
            }

            if (!tempStringDictionary.TryGetValue(label, out AnimBool animBool)) {
                animBool = new AnimBool();
                tempStringDictionary.Add(label, animBool);
                animBool.valueChanged.AddListener(editorWindow.Repaint);
            }

            Rect rect = EditorGUILayout.BeginVertical(GUILayout.MaxWidth(Screen.width));

            GUI.Box(new Rect(rect.x, rect.y, rect.width, rect.height - 2), "", EditorStyles.helpBox);
            if (GUI.Button(new Rect(rect.x, rect.y, rect.width - 70, 20), "")) {
                animBool.target = !animBool.target;
            }

            if (GUI.Button(new Rect(rect.x + rect.width - 70, rect.y, 70, 20), "Reset")) {
                rightButtonTriggered = true;
            }
            else
                rightButtonTriggered = false;

            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

            if (EditorGUILayout.BeginFadeGroup(animBool.faded)) {
                GUILayout.Space(4);
                return true;
            }
            else
                return false;
        }

        public static void EndFadeGroup()
        {
            EditorGUILayout.Space(6);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndFadeGroup();
            EditorGUILayout.EndVertical();
        }

        private static Dictionary<Type, int[]> toolbarIndices = new Dictionary<Type, int[]>();

        public static int Toolbar(string[][] texts, Editor editor)
        {
            if (texts == null || editor == null)
                return 0;

            Type editorType = editor.GetType();
            if (!toolbarIndices.TryGetValue(editorType, out int[] indices)) {
                int[] newIndices = new int[texts.Length];
                for (int i = 1; i < newIndices.Length; i++)
                    newIndices[i] = -1;
                toolbarIndices.Add(editorType, newIndices);
                indices = newIndices;
                Debug.Log("Didnt get value");
            }

            if (indices == null || indices.Length != texts.Length) {
                int[] newIndices = new int[texts.Length];
                for (int i = 1; i < newIndices.Length; i++)
                    newIndices[i] = -1;
                toolbarIndices[editorType] = newIndices;
                indices = newIndices;
                Debug.Log("Indices was null or something");
            }

            int currentCount = 0;
            int returnSelected = 0;
            for (int i = 0; i < indices.Length; i++) {
                int tempIndex = GUILayout.Toolbar(indices[i], texts[i]);
                if (indices[i] == -1) {
                    if (tempIndex != -1) {
                        returnSelected = tempIndex + currentCount;
                        indices[i] = tempIndex;
                        for (int r = 0; r < indices.Length; r++)
                            if (r != i)
                                indices[r] = -1;
                    }
                }
                else {
                    returnSelected = tempIndex + currentCount;
                    indices[i] = tempIndex;
                }

                currentCount += texts[i].Length;
            }

            toolbarIndices[editorType] = indices;
            return returnSelected;
        }
    }
}