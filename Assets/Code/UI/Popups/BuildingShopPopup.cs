using System;
using System.Collections.Generic;
using Code.Common.EditorUtils;
using Code.Game.Configs.Buildings;
using Code.UI.Animations;
using Code.UI.ViewModels;
using Code.UI.Views;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.UI.Popups
{
    public class BuildingShopPopup : PopupBase
    {
        [Header("UI Panels")]
        [SerializeField, CantBeNull] private RectTransform _topCurrencyPanel = null!;
        [SerializeField, CantBeNull] private RectTransform _bottomBuildingsPanel = null!;
        [SerializeField] private float _topPanelSlideDistance = 44;
        [SerializeField] private float _bottomPanelSlideDistance = 260f;
        [SerializeField] private float _panelSlideDuration = 0.4f;

        [Header("UI Content")]
        [SerializeField, CantBeNull] private CurrencyView _currencyView = null!;
        [SerializeField, CantBeNull] private Transform _cardContainer = null!;
        [SerializeField, CantBeNull] private BuildingCardView _cardPrefab = null!;
        public override PopupType Type => PopupType.BuildingsInfo;

        private readonly List<BuildingCardView> _spawnedCards = new();
        private BuildingShopViewModel _viewModel;
        private PopupManager _popupManager;
        private Vector2 _topPanelInitialPos;
        private Vector2 _bottomPanelInitialPos;
        protected override List<PopupElementAnimation> GetAnimatedElements() => new();
        private bool _initialized;
        private Action? _onClosePopup = null;

        public override void SetData(IPopupData popupData)
        {
            if (popupData is not BuildingShopPopupData shopData)
            {
                return;
            }
            _currencyView.Init(shopData.CurrencyViewModel); 
            _viewModel = shopData.BuildingShopViewModel;
            _popupManager = shopData.PopupManager;
            _onClosePopup = shopData.OnClose;
            if (!_initialized)
            {
                _topPanelInitialPos = _topCurrencyPanel.anchoredPosition;
                _bottomPanelInitialPos = _bottomBuildingsPanel.anchoredPosition;
                _initialized = true;
            }
        }
        
        public override async UniTask ShowAsync()
        {
            gameObject.SetActive(true);
            AnimatePanelsIn();
            CreateBuildingCards();
        }
        
        public override async UniTask HideAsync()
        {
            gameObject.SetActive(false);
            AnimatePanelsOut();
            _onClosePopup?.Invoke();
        }
        
        private void CreateBuildingCards()
        {
            foreach (var card in _spawnedCards)
            {
                Destroy(card.gameObject);
            }
            _spawnedCards.Clear();

            foreach (var entry in _viewModel.Buildings.Value)
            {
                var card = Instantiate(_cardPrefab, _cardContainer);
                card.Init(entry, OnBuildingSelected);
                _spawnedCards.Add(card);
            }
        }
        
        private void AnimatePanelsIn()
        {
            _topCurrencyPanel.anchoredPosition = _topPanelInitialPos + Vector2.up * _topPanelSlideDistance;
            _topCurrencyPanel.DOAnchorPos(_topPanelInitialPos, _panelSlideDuration).SetEase(Ease.OutBack);

            _bottomBuildingsPanel.anchoredPosition = _bottomPanelInitialPos + Vector2.down * _bottomPanelSlideDistance;
            _bottomBuildingsPanel.DOAnchorPos(_bottomPanelInitialPos, _panelSlideDuration).SetEase(Ease.OutBack);
        }

        private void AnimatePanelsOut()
        {
            _topCurrencyPanel.DOAnchorPosY(_topPanelSlideDistance, _panelSlideDuration).SetEase(Ease.InBack);
            _bottomBuildingsPanel.DOAnchorPosY(-_bottomPanelSlideDistance, _panelSlideDuration).SetEase(Ease.InBack);
        }

        private void OnBuildingSelected(BuildingConfig config)
        {
            if (!_viewModel.CanBuy(config))
                return;

            var confirmData = new BuildingConfirmData(
                config,
                () =>
                {
                    _viewModel.ConfirmPurchase(config);
                    HideAsync().Forget();
                },
                () => { }
            );

            _popupManager.ShowAsync(PopupType.BuildingConfirm, confirmData).Forget();
        }
    }
}
