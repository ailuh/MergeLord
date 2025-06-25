using Code.Common.EditorUtils;
using Code.Game.Configs.Interfaces;
using Code.Game.Configs.Monsters;
using Code.Game.Systems.Interfaces;
using Code.UI.Popups;
using Configs.Objects;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Code.Game.Systems.Services
{
    public class GameMessageService : MonoBehaviour, IGameMessageService
    {
        [SerializeField, CantBeNull] private TextMeshProUGUI _messageText = null!;
        [SerializeField, CantBeNull] private GameObject _lvlPanel = null!;
        [SerializeField] private Button _testInfoButton = null!;
        private PopupManager _popupManager;
        
        private void Start()
        {
            ShowMessage("Select something to learn about it");
        }

        [Inject]
        public void Construct(PopupManager popupManager)
        {
            _popupManager = popupManager;
        }
        
        public void ShowMessage(string message, bool isHasLvl = false, GridObjectConfigBase? config = null)
        {
            _messageText.text = message;
            _lvlPanel.gameObject.SetActive(isHasLvl);
            if (isHasLvl && config != null)
            {
                SetInfoButton(config);
            }
        }

        private void ShowUnitLvlPopup(GridObjectConfigBase config)
        {
            if (config is IObjectInfoChainProvider provider)
            {
                _popupManager
                    .ShowAsync(PopupType.UnitInfo, new UnitInfoPopupData(provider))
                    .Forget();
            }
            else
            {
                Debug.LogWarning($"Config {config.Id} does not implement IObjectInfoChainProvider");
            }        }

        private void SetInfoButton(GridObjectConfigBase config)
        {
            _testInfoButton.onClick.AddListener(() => ShowUnitLvlPopup(config));
        }
    }
}