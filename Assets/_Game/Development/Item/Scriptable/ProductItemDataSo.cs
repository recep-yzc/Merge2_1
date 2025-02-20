using _Game.Development.Extension.Static;
using UnityEngine;

namespace _Game.Development.Item.Scriptable
{
    [CreateAssetMenu(fileName = "ProductItemDataSo", menuName = "Game/Core/Board/Item/Product/Data")]
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