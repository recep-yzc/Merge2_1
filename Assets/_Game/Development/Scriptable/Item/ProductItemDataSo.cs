using _Game.Development.Enum.Item;
using _Game.Development.Static;
using UnityEngine;

namespace _Game.Development.Scriptable.Item
{
    [CreateAssetMenu(fileName = "ProductItemDataSo", menuName = "Game/Board/Item/Product/Data")]
    public class ProductItemDataSo : ItemDataSo
    {
        [Header("ProductItemData")] public ProductType productType;

        public ProductItemDataSo()
        {
            itemType = ItemType.Product;
        }

        public override int GetSpecialId()
        {
            return productType.ToInt();
        }
    }
}