using _Game.Development.Controller.Board;
using _Game.Development.Scriptable.Item;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Development.Ui.Grid
{
    public class GridEditorItemButton : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Image imgIcon;

        [SerializeField] private Button btnIcon;

        #region Unity Action

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