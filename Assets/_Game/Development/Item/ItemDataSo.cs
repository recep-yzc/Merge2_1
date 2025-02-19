using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _Game.Development.Item
{
    public abstract class ItemDataSo : ScriptableObject
    {
        [PreviewField] public Sprite icon;

        public int uniqueId;
        public ItemType itemType;

        public ItemDataSo nextItemDataSo;

        public string GetIconPath()
        {
            return icon ? AssetDatabase.GetAssetPath(icon) : string.Empty;
        }

        public abstract int GetSpecialId();
    }
}