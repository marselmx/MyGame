using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

//<summary>
//данный класс служит для хранение коллекций дочерних классов унаследованного от базового класса BaseClass
//коллекция дочерних классов хранятся в виде массива строк формата Json и при старте игры эти данные json серализирутся в объекты класса BaseClass
//</summary>
[Serializable]
public class BaseChildClassesSerializable<BaseClass> : ISerializationCallbackReceiver where BaseClass: class
{
    //<summary> список объектов дочерних классов, которые образуются после серилизаций </summary>
    private List<BaseClass> objs = new List<BaseClass>();

    //<summary> в данном структуре храниться данные объекта дочернего класса в формате Json</summary>
    [Serializable] private struct JsonContainer { public string ClassName; public string ClassData; }
    
    //<summary> список объектов дочерних классов хранящиеся в формате Json, эти данные храняться на жестком диске до серилизаций </summary>
    [SerializeField] private List<JsonContainer> jsons = new List<JsonContainer>();

    //<summary>получаем массив всех дочерних классов унаследованных от класса BaseClass </summary>
    public Type[] GetSubclassTypes = Assembly.
        GetAssembly(typeof(BaseClass)).
        GetTypes().
        Where(t => t.IsSubclassOf(typeof(BaseClass))).
        ToArray();

    //<summary> кол-во дочерних объектов </summary>
    public int Count
    {
        get { return objs.Count; }
        protected set{}
    }

    //<summary> получение и добавление дочернего класса в коллекцию по ключу </summary>
    public BaseClass this[int index] {
        get{ return objs[index]; }
        set{ objs[index] = value; }
    }
    
    //<summary> добавляем объект дочернего класса в коллекцию </summary>
    public void Add(BaseClass value) {
        objs.Add(value);
    }
    
    //<summary> удаляем объект дочернего класса из колекции </summary>
    public void Remove(BaseClass value) {
        objs.Remove(value);
    }

    //<summary> удаляем все дочерние классы из колекции </summary>
    public void Clear() {
        objs.Clear();
    }
    
    //<summary> для перебора наших дочерних объектов через цикл foreach </summary>
    public IEnumerator GetEnumerator() {
        return objs.GetEnumerator();
    }
    
    //<summary> вызывается до сериализаций, где мы записываем все данные в контейнер jsons </summary>
    public void OnBeforeSerialize() {
        //if(objs.Count == 0) return;
        jsons.Clear();
        foreach (BaseClass obj in objs)
        {
            JsonContainer json = new JsonContainer
            {
                ClassName = obj.ToString(), 
                ClassData = JsonUtility.ToJson(obj)
            };
            jsons.Add(json);
        }
    }

    //<summary> вызывается после десирилизаций, где мы записываем все данные в контейнер objs </summary>
    public void OnAfterDeserialize() {
        objs.Clear();
        foreach (JsonContainer json in jsons)
        {
            BaseClass obj = JsonUtility.FromJson(json.ClassData, Type.GetType(json.ClassName)) as BaseClass;
            objs.Add(obj);
        }
    }

    #if UNITY_EDITOR
    //<summary> данный класс служит для отображений в редакторе систему добавление/удаление дочерних классов </summary>
    public abstract class BaseChildClassesSerializableEditor : PropertyDrawer
    {
        private BaseChildClassesSerializable<BaseClass> _objs;
        
        private int _indexClass = 0;
        
        private RectPosition _position;
        
        //<summary> класс для работы с позициями </summary>
        private class RectPosition
        {
            private Rect position;

            private float shift;

            public float x
            {
                get { return this.position.x; }
                set { this.position.x = value; }
            }
            
            public float y
            {
                get { return this.position.y; }
                set { this.position.y = value; }
            }
            
            public float width
            {
                get { return this.position.width; }
                set { this.position.width = value; }
            }

            public float height
            {
                get { return 20; }
                protected set { }
            }

            public RectPosition(Rect position, float shift = 22) {
                this.position = position;
                this.shift = shift;
            }

            public static Rect operator +(RectPosition obj, float value)
            {
                obj.y += value;
                Rect Result = new Rect(obj.x, obj.y, obj.width, obj.height);
                return Result;
            }

            public static implicit operator Rect(RectPosition obj)
            {
                obj.y += obj.shift;
                Rect Result = new Rect(obj.x, obj.y, obj.width, obj.height);
                return Result;
            }
        }

        public List<float> sizesHeight = new List<float>();

        //<summary> перезагружаем метод GetPropertyHeight, в котором мы определяем высоту </summary>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float sizeHeight = 120;
            foreach (float size in sizesHeight)
                sizeHeight += size;
            sizesHeight.Clear();
            return sizeHeight;
        }

        //<summary> перезагружаем метод OnGUI, в котором мы верстаем поле редактора </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            
            //EditorGUI.BeginProperty(position, GUIContent.none, property);
            
            if (_objs == null) 
            {
                var target = property.serializedObject.targetObject;
                _objs = fieldInfo.GetValue(target) as BaseChildClassesSerializable<BaseClass>;
                if (_objs == null)
                {
                    _objs = new BaseChildClassesSerializable<BaseClass>();
                    fieldInfo.SetValue(target, _objs);
                }
            }
            
            _position = new RectPosition(position, 22);
            
            GUIContent l = new GUIContent(label.text + " (" +_objs.Count.ToString() + ")");
            EditorGUI.LabelField(this._position + 0, l, EditorStyles.centeredGreyMiniLabel);
            
            _indexClass = EditorGUI.Popup(this._position, "", _indexClass, 
                _objs.GetSubclassTypes.Select(t => t.ToString()).ToArray() );
            
            if (GUI.Button(this._position, new GUIContent("Добавить"))) 
                AddItem(_indexClass);

            if (GUI.Button(this._position, new GUIContent("Очистить"))) 
                ClearItems();
            
            //EditorGUI.EndProperty();
            
            //EditorGUI.LabelField(this._position,  "");
            
            foreach (BaseClass obj in _objs) 
                ShowItems(obj);
        }
        
        //<summary> данный метод служит для добавление нового дечернего класса</summary>
        private void AddItem(int index) {
            Type myType = _objs.GetSubclassTypes[index];
            BaseClass obj = (BaseClass)Activator.CreateInstance(myType);
            _objs.Add(obj);
        }
        
        //<summary> метод в котором мы удаляем определенный дочерний класс </summary>
        private void RemoveItem(BaseClass obj) {
            _objs.Remove(obj);
        }
        
        //<summary> метод в котором мы очищаем все доступные дочерние классы </summary>
        private void ClearItems() {
            _objs.Clear();
        }
        
        //<summary> в этом методе мы реализуем отображение дочернего класса и изменение данных свойств в этом классе </summary>
        private void ShowItems(BaseClass obj) {
            
            EditorGUI.LabelField(this._position + 15, "");

            Rect BoxPosition = _position;
            Rect BoxPositionRemove = new Rect(BoxPosition.xMax - 33, BoxPosition.y, 30, this._position.height );
            
            if (GUI.Button(BoxPositionRemove, new GUIContent("X"), EditorStyles.miniButtonLeft)) 
                RemoveItem(obj);
            
            float sizeField = 22;
            float sizeBox = 0;
            foreach (FieldInfo field in obj.GetType().GetFields()) {
                
                Rect fieldPosition = _position + sizeField;
                Rect fieldPositionName = new Rect(fieldPosition.x + 10, fieldPosition.y, fieldPosition.width/2, fieldPosition.height);
                Rect fieldPositionValue = new Rect(fieldPosition.width/2 - 75, fieldPosition.y, fieldPosition.width/2 + 90, fieldPosition.height);
                EditorGUI.LabelField(fieldPositionName, field.Name);
                
                EditorGUI.BeginChangeCheck();
                var value = DoFiled(field.FieldType, field.GetValue(obj), fieldPositionValue, ref sizeField);
                sizeBox += sizeField;
                
                if (field.FieldType.Name == "List`1")
                {
                    if ((int)value > 30) value = 32;
                    Type listType = field.GetValue(obj).GetType().GenericTypeArguments[0];
                    MethodInfo method = typeof(BaseChildClassesSerializableEditor).GetMethod(nameof(DoFiledsArray));
                    MethodInfo generic = method.MakeGenericMethod(listType);
                    object[] parametersArray = { field.GetValue(obj), (int)value, sizeField, sizeBox}; //ref sizeField, ref sizeBox 
                    var values = generic.Invoke(this, parametersArray);
                    sizeField = (float)parametersArray[2];
                    sizeBox = (float)parametersArray[3];
                    if (EditorGUI.EndChangeCheck()) field.SetValue(obj, values);
                    continue;
                }
                
                if (EditorGUI.EndChangeCheck()) field.SetValue(obj, value);
            }
            GUI.Box(new Rect(BoxPosition.x, BoxPosition.y, BoxPosition.width, sizeBox + 27), obj.GetType().Name );
            sizesHeight.Add(sizeBox + 32);
        }
        
        //<summary> в этом методе мы рисуем и отображаем поля свойствы класса </summary>
        private object DoFiled(Type type, object value, Rect position, ref float size) {
            
            object result = null;
            
            switch ( type.Name ) {
                case "Int32": result = EditorGUI.IntField(position, (int)value); size = 22; break;
                case "Single": result = EditorGUI.FloatField(position, (float)value); size = 22; break;
                case "Double": result = EditorGUI.DoubleField(position, (double)value); size = 22; break;
                case "Boolean": result = EditorGUI.Toggle(position, (bool)value); size = 22; break;
                case "String": result = EditorGUI.TextField(position, (string)value); size = 22; break; 
                case "Vector2": result = EditorGUI.Vector2Field(position, GUIContent.none, (Vector2) value); size = 22; break;
                case "Rect": result = EditorGUI.RectField(position, (Rect) value); size = 42; break; 
                case "Bounds": result = EditorGUI.BoundsField(position, (Bounds)value); size = 42; break; 
            }
            
            if (result == null) {
                
                size = 22;
                
                if (type.IsEnum) 
                    result = EditorGUI.EnumPopup(position, (Enum)(object)value);
                
                if (type.IsArray)
                    EditorGUI.LabelField(position, "Data array is not displayed");

                if (typeof(IList).IsAssignableFrom(type) && type.Name == "List`1")
                {
                    IList list = value as IList;
                    result = EditorGUI.IntField(position, list.Count);
                }
            }

            return result;
        }
        
        //<summary> рисуем контейнер List свойствы дочернего класса </summary>
        public List<T> DoFiledsArray<T>(object value, int count, ref float size, ref float sizeBox) {
            IList values = value as IList;
            Type type = typeof(T);
            
            List<T> result = new List<T>();
 
            for (int i = 0; i < count; i++)
            {
                Rect fieldPosition = _position + size;
                Rect fieldPositionValue = new Rect((fieldPosition.width/2 - 55), fieldPosition.y, (fieldPosition.width/2 + 70), fieldPosition.height);
                Rect fieldPositionNum = new Rect((fieldPosition.width/2 - 75), fieldPosition.y, (fieldPosition.width/2 + 90), fieldPosition.height);
                EditorGUI.LabelField(fieldPositionNum, (i + 1) + ".");
                object val = null;
                if(type.IsPrimitive) val = i < values.Count ? values[i] : 0;
                else if (type.Name == "String") val = i < values.Count ? values[i] : null;
                else val = i < values.Count ? values[i] : Activator.CreateInstance(type);
                var resval = DoFiled(type, val, fieldPositionValue, ref size);
                result.Add((T)resval);
                sizeBox += size;
            }

            return result;
        }
    }
    #endif
}
