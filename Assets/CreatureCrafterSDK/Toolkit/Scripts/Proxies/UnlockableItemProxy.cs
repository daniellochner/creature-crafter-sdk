using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.CreatureCrafter.SDK
{
    public class UnlockableItemProxy : ProxyBehaviour
    {
        public const float MinSpacing = 20f;

        public string itemId;

#if UNITY_EDITOR
        private static GUIStyle tooCloseStyle;
#endif

        public bool IsTooClose(Vector3 position)
        {
            return (transform.position - position).sqrMagnitude < MinSpacing * MinSpacing;
        }

        protected void DrawSpacingGizmo<T>(string itemName) where T : UnlockableItemProxy
        {
#if UNITY_EDITOR
            float nearest = float.MaxValue;
            foreach (T other in FindObjectsByType<T>(FindObjectsSortMode.None))
            {
                if (other != this)
                {
                    nearest = Mathf.Min(nearest, Vector3.Distance(transform.position, other.transform.position));
                }
            }
            if (nearest >= MinSpacing)
            {
                return;
            }

            Color color = Gizmos.color;
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
            Gizmos.DrawSphere(transform.position, 1f);
            Gizmos.color = color;

            tooCloseStyle ??= new GUIStyle { fontStyle = FontStyle.Bold, normal = { textColor = Color.red } };
            UnityEditor.Handles.Label(transform.position + Vector3.up * 2f, $"Too close to another {itemName} ({nearest:0.0}m, must be {MinSpacing}m apart)", tooCloseStyle);
#endif
        }
    }
}
