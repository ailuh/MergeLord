using Code.Common.EditorUtils;
using Code.Game.Model.Rewards;
using Code.Game.Systems.Interfaces;
using Code.Game.Systems.Managers;
using UnityEngine;

namespace Code.UI.Views
{
    public class RewardBufferView : MonoBehaviour
    {
        [SerializeField, CantBeNull] private Transform _container = null!;
        [SerializeField, CantBeNull] private RewardIconView _iconPrefab = null!;

        private RewardBufferModel _model;
        private GridManager _gridManager;
        private IGameMessageService _messageService;
        private RewardIconView _currentView;

        public void Init(RewardBufferModel model, GridManager gridManager, IGameMessageService messageService)
        {
            _model = model;
            _messageService = messageService;
            _gridManager = gridManager;
            _model.OnChanged += Refresh;
            Refresh();
        }

        private void OnDestroy()
        {
            if (_model != null)
            {
                _model.OnChanged -= Refresh;
            }
        }

        private void Refresh()
        {
            ClearCurrent();

            if (_model.IsEmpty)
                return;

            var reward = _model.Peek();
            if (reward == null)
                return;

            _currentView = Instantiate(_iconPrefab, _container);
            _currentView.Init(reward, OnRewardClicked);
        }

        private void OnRewardClicked()
        {
            if (_model.TryConsumeReward(out var reward))
            {
                if (!_gridManager.TryPlaceReward(reward))
                {
                    _messageService.ShowMessage("Grid is full!");
                    return;
                }

                ClearCurrent();
                Refresh();
            }
        }

        private void ClearCurrent()
        {
            if (_currentView != null)
            {
                Destroy(_currentView.gameObject);
                _currentView = null;
            }
        }
    }
}