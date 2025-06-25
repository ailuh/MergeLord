using Code.Common.EditorUtils;
using Code.UI.ViewModels;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Views
{
    public class MainScreenView : MonoBehaviour
    {
        [SerializeField, CantBeNull] private Button _questButton = null!;
        [SerializeField, CantBeNull] private Button _buildsButton = null!;
        [SerializeField, CantBeNull] private Button _shopButton = null!;
        [SerializeField, CantBeNull] private Button _bookButton = null!;
        [SerializeField, CantBeNull] private RectTransform _topPanel = null!;
        [SerializeField, CantBeNull] private RectTransform _bottomPanel = null!;
        [SerializeField] private float _panelSlideDuration = 0.3f;
        [SerializeField]private float _topPanelOffset = 100f;
        [SerializeField]private float _bottomPanelOffset = 200f;
        
        private Vector2 _topInitialPos;
        private Vector2 _bottomInitialPos;
        private MainScreenViewModel _mainScreenViewModel;
        private QuestViewModel _questViewModel;
        private BuildingShopViewModel _buildingShopViewModel;
        private CurrencyViewModel _currencyViewModel;
        
        private void Awake()
        {
            _topInitialPos = _topPanel.anchoredPosition;
            _bottomInitialPos = _bottomPanel.anchoredPosition;
        }
        
        public void Init(
            QuestViewModel questViewModel, 
            MainScreenViewModel mainScreenViewModel,
            BuildingShopViewModel buildingShopViewModel,
            CurrencyViewModel currencyViewModel)
        {
            _questViewModel = questViewModel;
            _mainScreenViewModel = mainScreenViewModel;
            _buildingShopViewModel = buildingShopViewModel;
            _currencyViewModel = currencyViewModel;
            SetBindings();
        }

        private void SetBindings()
        {
            _questButton.onClick.AddListener(() => _mainScreenViewModel.OnQuestClicked(_questViewModel));
            _buildsButton.onClick.AddListener(() =>
            {
                HideTopBottomPanels();
                _mainScreenViewModel.OnBuildingShopClicked(_buildingShopViewModel, _currencyViewModel, ShowTopBottomPanels);
            });
            _shopButton.onClick.AddListener(() => _mainScreenViewModel.OnShopClicked?.Invoke());
            _bookButton.onClick.AddListener(() => _mainScreenViewModel.OnBookClicked?.Invoke());
        }
        
        private void HideTopBottomPanels()
        {
            _topPanel.DOAnchorPosY(_topInitialPos.y + _topPanelOffset, _panelSlideDuration).SetEase(Ease.InBack);
            _bottomPanel.DOAnchorPosY(_bottomInitialPos.y - _bottomPanelOffset, _panelSlideDuration).SetEase(Ease.InBack);
        }

        public void ShowTopBottomPanels()
        {
            _topPanel.DOAnchorPos(_topInitialPos, _panelSlideDuration).SetEase(Ease.OutBack);
            _bottomPanel.DOAnchorPos(_bottomInitialPos, _panelSlideDuration).SetEase(Ease.OutBack);
        }
        
        private void OnDestroy()
        {
            _questButton.onClick.RemoveAllListeners();
            _buildsButton.onClick.RemoveAllListeners();
            _shopButton.onClick.RemoveAllListeners();
            _bookButton.onClick.RemoveAllListeners();
        }
    }
}
