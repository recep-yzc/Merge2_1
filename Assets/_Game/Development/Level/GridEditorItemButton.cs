using _Game.Development.Item;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Development.Level
{
    public class GridEditorItemButton : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Image imgIcon;

        [SerializeField] private Button btnIcon;

        #region Private

        private Item.ItemDataSo _itemDataSo;

        #endregion

        #region First

        private void Awake()
        {
            btnIcon.onClick.AddListener(OnClicked);
        }

        #endregion

        public void Init(Item.ItemDataSo itemDataSo)
        {
            _itemDataSo = itemDataSo;
            imgIcon.sprite = itemDataSo.icon;
        }

        private void OnClicked()
        {
            LevelEditor.SelectedItemDataSo = _itemDataSo;
        }
    }
}