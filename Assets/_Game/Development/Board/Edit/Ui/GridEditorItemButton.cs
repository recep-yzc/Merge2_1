using _Game.Development.Board.Edit.Controller;
using _Game.Development.Item.Scriptable;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Development.Board.Edit.Ui
{
    public class GridEditorItemButton : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Image imgIcon;

        [SerializeField] private Button btnIcon;

        #region First

        private void Awake()
        {
            btnIcon.onClick.AddListener(OnClicked);
        }

        #endregion

        public void Init(ItemDataSo itemDataSo, BoardEditController boardEditController)
        {
            _itemDataSo = itemDataSo;
            _boardEditController = boardEditController;

            imgIcon.sprite = itemDataSo.icon;
        }

        private void OnClicked()
        {
            _boardEditController.SetSelectedItemDataSo(_itemDataSo);
        }

        #region Private

        private ItemDataSo _itemDataSo;
        private BoardEditController _boardEditController;

        #endregion
    }
}