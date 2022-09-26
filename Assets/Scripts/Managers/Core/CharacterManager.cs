using GameWarriors.EventDomain.Abstraction;
using GameWarriors.PoolDomain.Abstraction;
using GameWarriors.TaskDomain.Abstraction;
using GameWarriors.UIDomain.Abstraction;
using Scripts.Common.ResourceKey;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Managers.Core.Characters
{
    public class CharacterManager : MonoBehaviour
    {
        private IEvent _event; 
        private ThirdPersonMovement _mainCharacter;

        public void Initialization(IPool pool, IEvent @event, IUpdateTask updateTask)
        {
            _mainCharacter = pool.GetGameBehavior<ThirdPersonMovement>(PoolKey.CHARACTER_MAIN_KEY);
            updateTask.RegisterUpdateTask(ManagerUpdate);
        }
        
        private void Startup(IServiceProvider serviceProvider) { }

        private void ManagerUpdate()
        {
            _mainCharacter?.CharacterUpdate();
        }
    }
}