using GameWarriors.EventDomain.Abstraction;
using GameWarriors.PoolDomain.Abstraction;
using GameWarriors.TaskDomain.Abstraction;
using GameWarriors.UIDomain.Abstraction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Managers.Core
{
    public class CharacterManager : MonoBehaviour
    {
        private IEvent _event;


        public void Initialization(IPool pool, IToast toast, IEvent @event, IUpdateTask updateTask, IServiceProvider serviceProvider)
        {

        }

        private void Startup(IServiceProvider serviceProvider) { }

    }
}