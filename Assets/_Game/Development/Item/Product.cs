using UnityEngine;

namespace _Game.Development.Item
{
    public class Product : Item, IProduct, IClickable
    {
        public void OnClick()
        {
            Debug.Log("OnClick " + transform.position);
        }
    }
}