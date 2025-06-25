using Code.Common.EditorUtils;
using Code.Game.Configs.Interfaces;
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
        private IGridObjectResolver _objectResolver;

        public void Init(
            RewardBufferModel model, 
            GridManager gridManager, 
            IGameMessageService messageService,
            IGridObjectResolver objectResolver)
        {
            _model = model;
            _messageService = messageService;
            _gridManager = gridManager;
            _objectResolver = objectResolver;
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
                var config = _objectResolver.Resolve(reward.Id);
                if (config == null)
                {
                    return;
                }
                if (!_gridManager.TryPlaceObject(config))
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