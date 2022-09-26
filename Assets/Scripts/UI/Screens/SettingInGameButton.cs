using GameWarriors.DependencyInjection.Extensions;
using GameWarriors.UIDomain.Abstraction;
using GameWarriors.UIDomain.Core;
using Services.Abstraction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI.Screens
{
    public class SettingInGameButton : UIScreenItem
    {
        public const string SCREEN_NAME = "SettingInGameButton";
        [SerializeField]
        private Button _settingButton;
        [SerializeField]
        private Text _coinCountLable;

        public override string ScreenName => SCREEN_NAME;
        public override bool HasBlackScreen => false;
        public override bool CanCloseByBack => false;

        public override void Initialization()
        {
            base.Initialization();
            _settingButton.onClick.AddListener(OnSettingButtonClick);
            IPlayerInventory playerInventory = ServiceProvider.GetService<IPlayerInventory>();
            _coinCountLable.text = playerInventory.CoinCount.ToString();
        }

        private void OnSettingButtonClick()
        {
            SettingScreen screen = ScreenHandler.ShowScreen<SettingScreen>(SettingScreen.SCREEN_NAME, ECanvasType.ScreenCanvas, EPreviosScreenAct.Queue);
            screen.SetData("herrlooooo");
        }
    }
}