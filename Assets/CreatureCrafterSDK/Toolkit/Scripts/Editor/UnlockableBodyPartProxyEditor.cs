using System.Collections.Generic;
using UnityEditor;

namespace DanielLochner.CreatureCrafter.SDK
{
    [CustomEditor(typeof(UnlockableBodyPartProxy), true), CanEditMultipleObjects]
    public class UnlockableBodyPartProxyEditor : UnlockableItemProxyEditor
    {
        protected override string Label => "Body Part";
        protected override IReadOnlyDictionary<string, string> CommunityItems => CommunityBodyParts.Paths;
    }
}
