using Code.Application.Interfaces;
using Code.Utils;
using TMPro;
using UnityEngine;

namespace Code.Domain.Services
{
    public class GameMessageService : MonoBehaviour, IGameMessageService
    {
        [SerializeField, CantBeNull] private TextMeshProUGUI _messageText = null!;
        [SerializeField, CantBeNull] private GameObject _lvlPanel = null!;

        private void Start()
        {
            ShowMessage("Select something to learn about it", false);
        }

        public void ShowMessage(string message, bool isHasLvl = false)
        {
            _messageText.text = message;
            _lvlPanel.gameObject.SetActive(isHasLvl);
        }
        
    }
}