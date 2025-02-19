using UnityEngine;

namespace _Game.Development.Item
{
    [CreateAssetMenu(fileName = "ProductItemDataSo", menuName = "Game/Item/Product/Data")]
    public class ProductItemDataSo : ItemDataSo
    {
        [Header("ProductItemData")] public ProductType productType;

        public ProductItemDataSo()
        {
            itemType = ItemType.Product;
        }
    }
}