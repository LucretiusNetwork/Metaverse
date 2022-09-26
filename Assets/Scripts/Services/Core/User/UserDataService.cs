using Services.Abstraction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Core.User
{
    public class UserDataService : IPlayerInventory
    {
        public int CoinCount => 10;
    }
}