using System.Collections;
using System.Collections.Generic;
using Client;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //<summary> Кол-во пищи персонажа </summary>
    [Tooltip("Пища")] [SerializeField] private int food = 10;
    public int Food
    {
        get { return food; }
        set { food = value; }
    }
    
    //<summary> Кол-во денег персонажа </summary>
    [Tooltip("Деньги")] [SerializeField] private int money = 0;
    public int Money
    {
        get { return money; }
        set { money = value; }
    }
    
    //<summary> текущий уровень игрока </summary>
    [Tooltip("Уровень")] [SerializeField] private Game.LevelType level = Game.LevelType.Level1;
    public Game.LevelType Level
    {
        get { return level; }
        set { level = value; }
    }
}
