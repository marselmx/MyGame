using System;
using Leopotam.Ecs;
using UnityEngine;

namespace Client {
    sealed class GameInitSystem : IEcsSystem, IEcsInitSystem {
        
        //private EcsWorld _world = null;
        private DataService _data;

        public void Init ()
        {
            
        }
    }
}