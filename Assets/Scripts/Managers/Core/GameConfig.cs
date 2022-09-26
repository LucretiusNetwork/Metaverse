using GameWarriors.ResourceDomain.Abstraction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers.Core
{
    public class GameConfig : IResourceConfig
    {
        public int ShiftCount => 3;

        public bool IsPreloadBundles => false;
    }
}