using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DanielLochner.CreatureCrafter.SDK
{
    public abstract class UnlockableItemProxyEditor : Editor
    {
        protected abstract string Label { get; }
        protected abstract IReadOnlyDictionary<string, string> CommunityItems { get; }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            }
            DrawPropertiesExcluding(serializedObject, "m_Script", "itemId");

            SerializedProperty itemId = serializedObject.FindProperty("itemId");
            bool isCommunity = CommunityItems.TryGetValue(itemId.stringValue, out string path);

            Rect rect = EditorGUI.PrefixLabel(EditorGUILayout.GetControlRect(), new GUIContent(Label));
            string label = itemId.hasMultipleDifferentValues ? "—" : (isCommunity ? path : "Workshop Item");
            if (EditorGUI.DropdownButton(rect, new GUIContent(label), FocusType.Keyboard))
            {
                GenericMenu menu = new();
                menu.AddItem(new GUIContent("Workshop Item"), !isCommunity, () => { if (isCommunity) SetItemId(""); });
                foreach (var item in CommunityItems.OrderBy(x => x.Value, StringComparer.Ordinal))
                {
                    menu.AddItem(new GUIContent($"Other/{item.Value}"), item.Key == itemId.stringValue, () => SetItemId(item.Key));
                }
                menu.DropDown(rect);
            }

            if (!isCommunity)
            {
                EditorGUILayout.PropertyField(itemId, new GUIContent("Workshop ID"));
            }

            serializedObject.ApplyModifiedProperties();
        }

        // Menu callbacks run after OnInspectorGUI, so they write through a fresh update.
        private void SetItemId(string id)
        {
            serializedObject.Update();
            serializedObject.FindProperty("itemId").stringValue = id;
            serializedObject.ApplyModifiedProperties();
        }
    }
}
