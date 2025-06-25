using System;
using Code.UI.Popups;
using Cysharp.Threading.Tasks;

namespace Code.UI.ViewModels
{
    public class MainScreenViewModel
    {
        public Action OnBuildsClicked;
        public Action OnShopClicked;
        public Action OnBookClicked;

        private readonly PopupManager _popupManager;

        public MainScreenViewModel(PopupManager popupManager)
        {
            _popupManager = popupManager;
        }
        
        public void OnQuestClicked(QuestViewModel questViewModel)
        {
            _popupManager.ShowAsync(PopupType.QuestInfo, new MainQuestsPopupData(questViewModel)).Forget();
        }
        
        public void OnBuildingShopClicked(BuildingShopViewModel buildingShopViewModel, CurrencyViewModel currencyViewModel, Action? onClose = null)
        {
            _popupManager.ShowAsync(
                    PopupType.BuildingsInfo, 
                    new BuildingShopPopupData(buildingShopViewModel, _popupManager, currencyViewModel, onClose))
                    .Forget();
        }
    }
}
