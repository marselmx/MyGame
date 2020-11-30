using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace Client {
    
    [System.Serializable]
    public class DataService
    {
        //<summary> префаб игрока </summary>
        public PlayerScript playerPrefab = null;
        
        //<summary> игровые событий </summary>
        public GameEventData gameEvent;
    }
    
    sealed class EcsStartup : MonoBehaviour 
    {
        //<summary> мир </summary>
        EcsWorld _world;
        
        //<summary> системы </summary>
        EcsSystems _systems;

        //<summary> Тут содержаться различные данные сцены </summary>
        public DataService data = new DataService();
        
        void Start ()
        {
            //Debug.Log(data.gameEvent.gameEvents.a);
            //foreach (var obj in data.gameEvent.gameEvents.gameEvents)
            //{
            //    Debug.Log(obj);
            //}

            // void can be switched to IEnumerator for support coroutines.
            
            _world = new EcsWorld ();
            _systems = new EcsSystems (_world);

            Game.player = Object.Instantiate(data.playerPrefab);

            #if UNITY_EDITOR
                 Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create (_world);
                 Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create (_systems);
            #endif
            
            _systems
                    
                // register your systems here, for example:
                //.Add (new TestSystem()) //тестовая система
                .Add (new GameInitSystem()) //система в котором будет инициализироваться поле, где каждое поле это сущность, представляющий собою событие GameEventData
                .Add (new MovePlayerSystem()) //система которая отвечает за передвижение игрока и сробатывание событие (срабатывание событие можно перенести в отдельную систему)
                
                // register one-frame components (order is important), for example:
                //.OneFrame<TestComponent> ()

                // inject service instances here (order doesn't important), for example:
                .Inject (data)
                // .Inject (new CameraService ())
                // .Inject (new NavMeshSupport ())
                
                .Init ();
        }

        void Update () {
            _systems?.Run ();
        }

        void OnDestroy () {
            if (_systems != null) {
                _systems.Destroy ();
                _systems = null;
                _world.Destroy ();
                _world = null;
            }
        }
    }
}