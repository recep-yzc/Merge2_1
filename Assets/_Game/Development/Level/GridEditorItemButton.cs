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

        private ItemDataSo _ıtemDataSo;

        #endregion

        #region First

        private void Awake()
        {
            btnIcon.onClick.AddListener(OnClicked);
        }

        #endregion

        public void Init(ItemDataSo ıtemDataSo)
        {
            _ıtemDataSo = ıtemDataSo;
            imgIcon.sprite = ıtemDataSo.icon;
        }

        private void OnClicked()
        {
            LevelEditor.SelectedItemDataSo = _ıtemDataSo;
        }
    }
}