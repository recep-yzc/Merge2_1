using UnityEngine;

namespace _Game.Development.Item
{
    public abstract class Item : MonoBehaviour, IItem
    {
        [Header("References")] [SerializeField]
        private SpriteRenderer sprIcon;

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetSprite(Sprite icon)
        {
            sprIcon.sprite = icon;
        }
    }
}