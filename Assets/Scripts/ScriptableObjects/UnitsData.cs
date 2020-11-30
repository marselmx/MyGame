using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Data/Units", order = 50)]
public class UnitsData : ScriptableObject
{
    //<summary> Id персонажа </summary>
    [Tooltip("ID")] [SerializeField] private int id = 0;
    public int Id
    {
        get { return id; }
        protected set{}
    }
    
    //<summary> Наименование персонажа </summary>
    [Tooltip("Название")] [SerializeField] private string unitName = "";
    public string UnitName
    {
        get { return unitName; }
        protected set{}
    }
    
    //<summary> Здоровье персонажа </summary>
    [Tooltip("Здоровье")] [SerializeField] private int health = 1;
    public int Health
    {
        get { return health; }
        protected set{}
    }
    
    //<summary> Урон персонажа </summary>
    [Tooltip("Урон")] [SerializeField] private int damage = 1;
    public int Damage
    {
        get { return damage; }
        protected set{}
    }
    
    //<summary> Скорость персонажа </summary>
    [Tooltip("Скорость")] [SerializeField] private int speed = 1;
    public int Speed
    {
        get { return speed; }
        protected set{}
    }

}