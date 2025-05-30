using Code.Common.EditorUtils;
using Code.UI.Animations;
using Code.UI.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Views
{
    public class QuestEntryView : PopupElementAnimation
    {
        [SerializeField, CantBeNull] private TextMeshProUGUI _description = null!;
        [SerializeField, CantBeNull] private Image _icon = null!;
        [SerializeField, CantBeNull] private Image _progress = null!;
        [SerializeField, CantBeNull] private Image _background = null!;
        [SerializeField, CantBeNull] private Button _claimButton = null!;
        [SerializeField, CantBeNull] private TextMeshProUGUI _progressLabel = null!;

        private QuestEntryViewModel _viewModel;

        public void Init(QuestEntryViewModel viewModel)
        {
            _viewModel = viewModel;
            _description.text = _viewModel.Description;
            _icon.sprite = _viewModel.RewardIcon;
            _background.color = _viewModel.IsCompleted ? Color.green : _background.color;
            _progressLabel.text = $"{_viewModel.CurrentProgress}/{_viewModel.TargetProgress}";
            _progress.fillAmount = (float)_viewModel.CurrentProgress / _viewModel.TargetProgress;
            _claimButton.onClick.RemoveAllListeners();
            _claimButton.onClick.AddListener(OnRewardClaimed);
            _icon.color = _viewModel.RewardClaimed ? Color.gray : _background.color;
            _claimButton.interactable = _viewModel.IsCompleted && !_viewModel.RewardClaimed;
        }

        private void OnRewardClaimed()
        {
            _viewModel.TryClaimReward();
            if (_viewModel.RewardClaimed)
            {
                _icon.color = _viewModel.RewardClaimed ? Color.gray : _background.color;
            }
        }
        
        private void OnDestroy()
        {
            _claimButton.onClick.RemoveAllListeners();
        }
    }
}
