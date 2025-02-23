using _Game.Development.Enum.Item;
using _Game.Development.Static;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _Game.Development.Scriptable.Item
{
    public abstract class ItemDataSo : ScriptableObject
    {
        [PreviewField] public Sprite icon;

        public ItemType itemType;
        public int level;

        public ItemDataSo nextItemDataSo;

        public string GetIconPath()
        {
            return icon ? AssetDatabase.GetAssetPath(icon) : string.Empty;
        }

        public abstract int GetSpecialId();

        public int GetItemId()
        {
            return itemType.ToInt();
        }
    }
}