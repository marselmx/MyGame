using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

//<summary> Класс в которм мы храним все событий котгорые будут происходить с нашим персонажем </summary>
[Serializable]
public class GameEventItems : BaseChildClassesSerializable<GameEvent> {
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(GameEventItems))]
    public class GameEventItemsEditor : BaseChildClassesSerializableEditor { }
#endif
}

//<summary> Класс словаря производящий расчёт количество уровней у событий </summary>
[Serializable]
public class SizeLevelsDictionary : DictionarySerializable<Game.LevelType, int> {
    #if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(SizeLevelsDictionary))]
        public class SizeLevelsDictionaryEditor : DictionarySerializableEditor { }
    #endif
}

//<summary> В этом классе хранятся данные событий </summary>
[CreateAssetMenu(fileName = "New Event", menuName = "Data/Game Event", order = 50)]
public class GameEventData : ScriptableObject
{
    //<summary> Текст отображаемое во время событий </summary>
    [Tooltip("Текст")] [SerializeField] private string text = "";
    public string Text
    {
        get { return text; }
        protected set{}
    }
    
    //<summary> уровени игры и их количество, в котором может произойти данное событие </summary>
    [Tooltip("Кол-во уровней")] [SerializeField] public SizeLevelsDictionary levelsSize;

    //<summary> Список пунктов для событий </summary>
    [Tooltip("Пункты событий")] [SerializeField] public GameEventItems gameEvents;
}