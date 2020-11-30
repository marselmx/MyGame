using Leopotam.Ecs;

namespace Client {
    sealed class MovePlayerSystem : IEcsSystem, IEcsRunSystem {
        
        // auto-injected fields.
        readonly EcsWorld _world = null;
        
        void IEcsRunSystem.Run () {
            // add your run code here.
        }
    }
}