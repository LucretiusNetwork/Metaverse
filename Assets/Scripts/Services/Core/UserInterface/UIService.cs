using GameWarriors.EventDomain.Abstraction;
using GameWarriors.ResourceDomain.Abstraction;
using GameWarriors.TaskDomain.Abstraction;
using GameWarriors.UIDomain.Abstraction;
using GameWarriors.UIDomain.Core;
using Services.Abstraction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Core.UserInterface
{
    public class UIService :IUIService
    {
        public UIService(IUpdateTask updateTask)
        {
            updateTask.RegisterUpdateTask(ServiceUpdate);
        }

        public void Initialization(IVariableDatabase variableDatabase)
        {
            
        }

        private void ServiceUpdate()
        {
            //throw new NotImplementedException();
        }
    }
}