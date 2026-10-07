using System.Collections.Generic;
using UnityEditor;

namespace DanielLochner.CreatureCrafter.SDK
{
    [CustomEditor(typeof(UnlockablePatternProxy), true), CanEditMultipleObjects]
    public class UnlockablePatternProxyEditor : UnlockableItemProxyEditor
    {
        protected override string Label => "Pattern";
        protected override IReadOnlyDictionary<string, string> CommunityItems => CommunityPatterns.Paths;
    }
}
