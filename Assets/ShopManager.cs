using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    //public Item item;
    public int hpPrice;
    public int epPrice;
    public int damagePrice;

    private GameManager gameManager;
    //private InventoryManager inventoryManager;
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        //inventoryManager = FindAnyObjectByType<InventoryManager>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void BuyHP()
    {
        if (gameManager.score <= hpPrice)
        {
            return;
        }
        else
        {
            gameManager.score -= hpPrice;
            gameManager.UpdateScore();
            InventoryManager.Instance.Add(items.Find(item => item.itemName == "HealthPotion"));
        }
    }
    public void BuyEP()
    {
        if (gameManager.score <= epPrice)
        {
            return;
        }
        else
        {
            gameManager.score -= epPrice;
            gameManager.UpdateScore();
            InventoryManager.Instance.Add(items.Find(item => item.itemName == "EnergyPotion"));
        }
    }
    public void BuyDamage()
    {
        if (gameManager.score <= damagePrice)
        {
            return;
        }
        else
        {
            gameManager.score -= damagePrice;
            gameManager.UpdateScore();
            InventoryManager.Instance.Add(items.Find(item => item.itemName == "DamagePotion"));
        }
    }

}
