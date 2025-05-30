using Code.Common.EditorUtils;
using Code.UI.ViewModels;
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
        private MainScreenViewModel _mainScreenViewModel;
        private QuestViewModel _questViewModel;
        
        public void Init(QuestViewModel questViewModel, MainScreenViewModel mainScreenViewModel)
        {
            _questViewModel = questViewModel;
            _mainScreenViewModel = mainScreenViewModel;
            SetBindings();
        }

        private void SetBindings()
        {
            _questButton.onClick.AddListener(() => _mainScreenViewModel.OnQuestClicked(_questViewModel));
            _buildsButton.onClick.AddListener(() => _mainScreenViewModel.OnBuildsClicked?.Invoke());
            _shopButton.onClick.AddListener(() => _mainScreenViewModel.OnShopClicked?.Invoke());
            _bookButton.onClick.AddListener(() => _mainScreenViewModel.OnBookClicked?.Invoke());
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
