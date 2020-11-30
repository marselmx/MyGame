using UnityEngine;
using Leopotam.Ecs;

namespace Client {
    sealed class TestSystem : IEcsSystem, IEcsInitSystem, IEcsRunSystem
    {
        readonly EcsWorld _world = null;
        EcsFilter<TestComponent> _filter = null;
        private DataService _data;

        public void Init ()
        {
            _world.NewEntity().Get<TestComponent>().value = 777;
        }
        
        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var a = ref _filter.Get1(index);
                Debug.Log(a.value);
            }
        }
    }

    public struct TestComponent
    {
        public int value;
    }
}