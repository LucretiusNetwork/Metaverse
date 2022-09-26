using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Abstraction
{
    public interface IRealTimeService
    {
        void StartConnection(string appId);
        void UpdateRealTime();
    }
}