using System;
using UnityEngine;

public class MoneyHandler : MonoBehaviour
{
    #region Singleton
    public static MoneyHandler Instance;
    private void Awake() 
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    public int CurrentMoney;

    public bool IsAffordable(Item item)
    {
        if(item.ItemPrice > CurrentMoney)
        {
            OnCantAfford();
            return false;
        }
        
        SpendMoney(item.ItemPrice);
        return true;
    }
    public void SpendMoney(int amount)
    {
        CurrentMoney -= amount;
    }

    private void OnCantAfford()
    {
        Debug.Log("Can't afford item");
    }

    public void AddMoney(int amount) => CurrentMoney += amount;

}