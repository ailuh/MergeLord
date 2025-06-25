using System;
using Code.Game.Configs.Buildings;
using Code.Game.Configs.Interfaces;
using Code.UI.ViewModels;

namespace Code.UI.Popups
{
    public class BuildingConfirmData : IPopupData
    {
        public readonly BuildingConfig Config;
        public readonly Action OnConfirm;
        public readonly Action OnCancel;

        public BuildingConfirmData(BuildingConfig config, Action onConfirm, Action onCancel)
        {
            Config = config;
            OnConfirm = onConfirm;
            OnCancel = onCancel;
        }
    }

    public class BuildingShopPopupData : IPopupData
    {
        public readonly BuildingShopViewModel BuildingShopViewModel;
        public readonly PopupManager PopupManager;
        public readonly CurrencyViewModel CurrencyViewModel;
        public readonly Action? OnClose = null;

        public BuildingShopPopupData(
            BuildingShopViewModel buildingShopViewModel, 
            PopupManager popupManager, 
            CurrencyViewModel currencyViewModel,
            Action? onClose)
        {
            BuildingShopViewModel = buildingShopViewModel;
            PopupManager = popupManager;
            CurrencyViewModel = currencyViewModel;
            OnClose = onClose;
        }
    }

    public class UnitInfoPopupData : IPopupData
    {
        public IObjectInfoChainProvider Provider;

        public UnitInfoPopupData(IObjectInfoChainProvider provider)
        {
            Provider = provider;
        }
    }

    public class MainQuestsPopupData : IPopupData
    {
        public readonly QuestViewModel QuestViewModel;

        public MainQuestsPopupData(QuestViewModel questViewModel)
        {
            QuestViewModel = questViewModel;
        }
    }
    
}