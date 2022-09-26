using Common.Extensions;
using Fusion;
using GameWarriors.DependencyInjection.Attributes;
using GameWarriors.DependencyInjection.Core;
using GameWarriors.DependencyInjection.Extensions;
using GameWarriors.EventDomain.Abstraction;
using GameWarriors.EventDomain.Core;
using GameWarriors.PoolDomain.Abstraction;
using GameWarriors.PoolDomain.Core;
using GameWarriors.ResourceDomain.Abstraction;
using GameWarriors.ResourceDomain.Core;
using GameWarriors.TaskDomain.Abstraction;
using GameWarriors.TaskDomain.Core;
using GameWarriors.UIDomain.Abstraction;
using GameWarriors.UIDomain.Core;
using Managers.Core;
using Managers.Handlers;
using Services.Abstraction;
using Services.Core.App;
using Services.Core.Network;
using Services.Core.Resource;
using Services.Core.User;
using Services.Core.UserInterface;
using System;
using UnityEngine;

namespace Managers
{

    public class GameManager : MonoBehaviour
    {
        public const string INIT_METHOD_NAME = "Initialization";

        [SerializeField]
        private GameObject _splash;

        [Inject]
        private IEvent Event { get; set; }
        [Inject]
        private IServiceProvider ServiceProvider { get; set; }

        private void Awake()
        {
            ServiceCollectionEnumerator serviceCollection = new ServiceCollectionEnumerator(INIT_METHOD_NAME);
            serviceCollection.AddSingleton<GameManager>(this);
            serviceCollection.AddSingleton<IEvent, EventSystem>();

            serviceCollection.AddSingleton<IScreen, UISystem>();
            serviceCollection.AddSingleton<IToast, UISystem>();
            serviceCollection.AddSingleton<IAspectRatio, UISystem>();

            serviceCollection.AddSingleton<IPool, PoolSystem>();

            serviceCollection.AddSingleton<ITaskRunner, TaskSystem>();
            serviceCollection.AddSingleton<IUpdateTask, TaskSystem>();

            UIManager uiManager = GetComponent<UIManager>();
            serviceCollection.AddSingleton<IUIEventHandler, UIManager>(uiManager);

            serviceCollection.AddSingleton<IResourceConfig, GameConfig>();
            serviceCollection.AddSingleton<IVariableDatabase, ResourceSystem>();
            serviceCollection.AddSingleton<IContentDatabase, ResourceSystem>();
            serviceCollection.AddSingleton<ISpriteDatabase, ResourceSystem>();

            serviceCollection.AddSingleton<IUIService, UIService>();
            serviceCollection.AddSingleton<IPlayerInventory, UserDataService>();
            serviceCollection.AddSingleton<IResourceService, ResourceService>();
            serviceCollection.AddSingleton<IAppService, AppService>();
            serviceCollection.AddSingleton<RealtimeManager>(GetComponent<RealtimeManager>());
            serviceCollection.AddSingleton<IRealTimeService, RealTimeService>();
            StartCoroutine(serviceCollection.Build(Startup));
        }

        private void Startup()
        {
            IUpdateTask updateTask = ServiceProvider.GetService<IUpdateTask>();
            updateTask.EnableUpdate();
            Destroy(_splash);
            IEvent @event = ServiceProvider.GetService<IEvent>();
            @event.BroadcastInitializeEvent(ServiceProvider);
        }

        private void OnApplicationQuit()
        {
            Event?.BroadcastEvent(EEventType.OnApplicationQuit);
        }
    }
}