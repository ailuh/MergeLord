using Code.Common.EditorUtils;
using Code.Game.Configs.Monsters;
using Code.Game.Systems.Interfaces;
using Code.UI.Popups;
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
        
        public void ShowMessage(string message, bool isHasLvl = false, ObjectRef? objectRef = null)
        {
            _messageText.text = message;
            _lvlPanel.gameObject.SetActive(isHasLvl);
            if (isHasLvl && objectRef != null)
            {
                SetInfoButton(objectRef.Value);
            }
        }

        private void ShowUnitLvlPopup(ObjectRef objectRef)
        {
            _popupManager.ShowAsync(PopupType.UnitInfo, objectRef).Forget();
        }

        private void SetInfoButton(ObjectRef objectRef)
        {
            _testInfoButton.onClick.AddListener(() => ShowUnitLvlPopup(objectRef));
        }
    }
}