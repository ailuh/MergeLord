using System.Collections.Generic;
using Code.Common.EditorUtils;
using Code.Game.Configs.Buildings;
using Code.UI.Animations;
using Code.UI.Views;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Popups
{
    public class BuildingConfirmPopup : PopupBase
    {
        [Header("Animated Elements")]
        [SerializeField, CantBeNull] private List<PopupElementAnimation> _animatedElements = null!;

        [Header("Content")]
        [SerializeField, CantBeNull] private Image _icon = null!;
        [SerializeField, CantBeNull] private TextMeshProUGUI _nameText = null!;
        [SerializeField, CantBeNull] private TextMeshProUGUI _levelText = null!;
        [SerializeField, CantBeNull] private Transform _costContainer = null!;
        [SerializeField, CantBeNull] private BuildingCostView _costPrefab = null!;

        [Header("Buttons")]
        [SerializeField, CantBeNull] private Button _confirmButton = null!;
        [SerializeField, CantBeNull] private Button _cancelButton = null!;

        public override PopupType Type => PopupType.BuildingConfirm;

        protected override List<PopupElementAnimation> GetAnimatedElements() => _animatedElements;

        public override void SetData(IPopupData popupData)
        {
            if (popupData is not BuildingConfirmData buildingConfirmData)
            {
                return;
            }

            SetContent(buildingConfirmData.Config);

            _confirmButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();

            _confirmButton.onClick.AddListener(() =>
            {
                buildingConfirmData.OnConfirm?.Invoke();
                HideAsync().Forget();
            });

            _cancelButton.onClick.AddListener(() =>
            {
                buildingConfirmData.OnCancel?.Invoke();
                HideAsync().Forget();
            });
        }

        private void SetContent(BuildingConfig config)
        {
            _icon.sprite = config.Sprite;
            _nameText.text = config.DisplayName;
            _levelText.text = config.RequiredLevel.ToString();

            foreach (Transform child in _costContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var cost in config.Costs)
            {
                var costView = Instantiate(_costPrefab, _costContainer);
                costView.Set(cost.Currency, cost.Amount);
            }
        }
    }
}