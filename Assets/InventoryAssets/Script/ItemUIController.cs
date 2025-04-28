using UnityEngine;

public class ItemUIController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Item item;
    public void SetItem(Item item)
    {
        this.item = item;
    }
    public void Remove()
    {
        InventoryManager.Instance.Remove(item);
        Destroy(this.gameObject);
    }
    public void UseItem()
    {
        Remove();
        switch (item.itemName)
        {
            case "HealthPotion":
                FindAnyObjectByType<PlayerAttack>().Heal(item.value);
                break;
            case "EnergyPotion":
                FindAnyObjectByType<PlayerController>().HealEnergy(item.value);
                break;
        }
    }
}
