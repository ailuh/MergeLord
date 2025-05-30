using System;
using System.Collections.Generic;
using Code.Common.EditorUtils;
using Code.UI.Animations;
using Code.UI.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Views
{
    public class QuestView : MonoBehaviour
    {
        [SerializeField, CantBeNull] private Transform _questListRoot = null!;
        [SerializeField, CantBeNull] private QuestEntryView _questEntryPrefab = null!;
        [SerializeField, CantBeNull] private Button _claimMainReward = null!;
        [SerializeField, CantBeNull] private TextMeshProUGUI _progressLabel = null!;
        [SerializeField, CantBeNull] private TextMeshProUGUI _description = null!;
        [SerializeField, CantBeNull] private Image _finalRewardIcon = null!;
        [SerializeField, CantBeNull] private ScrollRect _scrollRect = null!;
        
        private readonly List<PopupElementAnimation> _animatedElements = new ();
        private QuestViewModel _viewModel;
        private readonly List<QuestEntryView> _questViews = new();
        private Action _animateCallback;

        public List<PopupElementAnimation> Init(QuestViewModel viewModel, Action animateCallback)
        {
            _viewModel = viewModel;
            Refresh();
            _animateCallback = animateCallback;
            return _animatedElements;
        }

        private void Refresh()
        {
            _scrollRect.verticalNormalizedPosition = 1f;
            foreach (var view in _questViews)
            {
                Destroy(view.gameObject);
            }

            _questViews.Clear();
            _animatedElements.Clear();

            foreach (var entry in _viewModel.QuestEntries)
            {
                var view = Instantiate(_questEntryPrefab, _questListRoot);
                view.gameObject.SetActive(true);
                view.Init(entry);
                _questViews.Add(view);
                _animatedElements.Add(view);
            }

            if (_viewModel.FinalRewardEntry != null)
            {
                _finalRewardIcon.gameObject.SetActive(true);
                _description.gameObject.SetActive(true);

                _finalRewardIcon.sprite = _viewModel.FinalRewardEntry.RewardIcon;
                _description.text = _viewModel.FinalRewardEntry.Description;

                _claimMainReward.interactable = _viewModel.FinalRewardEntry.IsCompleted && !_viewModel.FinalRewardEntry.RewardClaimed;

                _claimMainReward.onClick.RemoveAllListeners();
                _claimMainReward.onClick.AddListener(() =>
                {
                    if (_viewModel.TryClaimFinalReward())
                    {
                        Refresh();
                        _animateCallback.Invoke();
                    }
                });
            }
            else
            {
                _finalRewardIcon.gameObject.SetActive(false);
                _description.gameObject.SetActive(false);
                _claimMainReward.interactable = false;
                _claimMainReward.onClick.RemoveAllListeners();
            }

            UpdateProgress();
        }

        private void UpdateProgress()
        {
            _progressLabel.text = $"{_viewModel.CompletedCount}/{_viewModel.QuestEntries.Count}";

            if (_viewModel.FinalRewardEntry != null)
            {
                _finalRewardIcon.color = _viewModel.FinalRewardEntry.RewardClaimed ? Color.gray : Color.white;
            }
        }

        private void OnDestroy()
        {
            _claimMainReward.onClick.RemoveAllListeners();
        }
    }
}
