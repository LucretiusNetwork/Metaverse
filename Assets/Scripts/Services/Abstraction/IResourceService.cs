using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Services.Abstraction
{

    public interface IResourceService
    {
        void ConnectToShop(Action<(int status, string result)> onDone);
    }
}