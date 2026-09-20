using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.CreatureCrafter.SDK
{
    [SelectionBase]
    public class WaterProxy : ProxyBehaviour
    {
        public WaterType type;
        public GameObject customSplashPrefab;
        public bool allowSwimming = true;

        public static List<WaterProxy> Proxies { get; private set; } = new ();

        private static readonly Vector3 VISUAL_SCALE = new(10f, 1f, 10f);
        private static readonly Vector3 VISUAL_POSITION = new(0f, 0f, 0f);

        private void OnEnable()
        {
            Proxies.Add(this);
        }
        private void OnDisable()
        {
            Proxies.Remove(this);
        }
        private void OnValidate()
        {
            if (transform.childCount > 0)
            {
                var visual = transform.GetChild(0);
                visual.localScale = VISUAL_SCALE;
                visual.localPosition = VISUAL_POSITION;
            }
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            var previousMatrix = Gizmos.matrix;
            var previousColor = Gizmos.color;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = new Color(0.2f, 0.75f, 1f);
            Gizmos.DrawWireCube(new Vector3(0f, -50f, 0f), new Vector3(100f, 100f, 100f));
            Gizmos.matrix = previousMatrix;
            Gizmos.color = previousColor;
        }

        public override bool IsValid(out string error)
        {
            if ((int)type < (int)WaterType.Empty || (int)type > (int)WaterType.Lake)
            {
                error = "Water type has an unsupported value.";
                return false;
            }

            var scale = transform.localScale;
            if (!IsFinite(scale) || scale.x <= 0f || scale.y <= 0f || scale.z <= 0f)
            {
                error = "Water width, depth, and length scales must be positive finite values.";
                return false;
            }

            return base.IsValid(out error);
        }

        public enum WaterType
        {
            Empty,
            Ocean,
            Lake
        }
    }
}
