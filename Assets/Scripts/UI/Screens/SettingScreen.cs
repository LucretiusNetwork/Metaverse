using GameWarriors.UIDomain.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI.Screens
{
    public class SettingScreen : UIScreenItem
    {
        public const string SCREEN_NAME = "SettingScreen";
        public override string ScreenName => SCREEN_NAME;

        [SerializeField]
        private Button  _closeButton;

        public override void Initialization()
        {
            base.Initialization();
            _closeButton.onClick.AddListener(CloseClick);
        }

        private void CloseClick()
        {
            ScreenHandler.CloseScreen(ScreenName);
        }

        public void SetData(string data)
        {
            
        }
    }
}