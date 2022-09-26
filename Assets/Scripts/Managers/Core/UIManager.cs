using GameWarriors.EventDomain.Abstraction;
using GameWarriors.TaskDomain.Abstraction;
using GameWarriors.UIDomain.Abstraction;
using GameWarriors.UIDomain.Core;
using System;
using GameUI.Screens;
using UnityEngine;


namespace Managers.Core
{
    public class UIManager : MonoBehaviour, IUIEventHandler
    {
        private IEvent _event;
        private IScreen _screen;
        private Action _uiUpdate;


        public void Initialization(IScreen screen, IToast toast, IEvent @event, IUpdateTask updateTask, IServiceProvider serviceProvider)
        {
            _screen = screen;
            UIScreenItem.Initialization(screen, serviceProvider, toast);
            _event = @event;
            updateTask.RegisterUpdateTask(_uiUpdate);
            _event.ListenToInitializeEvent(Startup);
        }

        private void Startup(IServiceProvider serviceProvider)
        {
            _screen.ShowScreen(MainHud.SCREEN_NAME, ECanvasType.MainCanvas, EPreviosScreenAct.Close);
        }

        public void OnCanvasCameraChange(Camera newCamera)
        {

        }

        public void OnCloseLastScreen()
        {

        }

        public void OnCloseScreen(IUIScreen screen)
        {

        }

        public void OnHideScreen(IUIScreen screen)
        {

        }

        public void OnOpenScreen(IUIScreen screen)
        {

        }

        public void OnScreenForceClose(IUIScreen screen)
        {

        }

        public void OnShowScreen(IUIScreen screen)
        {

        }

        public void OnToastRises(float showTimeLength, IToastItem toast)
        {

        }

        public void SetUIUpdate(Action uiUpdate)
        {
            _uiUpdate = uiUpdate;
        }
    }
}