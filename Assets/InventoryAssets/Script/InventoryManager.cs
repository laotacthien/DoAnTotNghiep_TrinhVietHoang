using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // sử dụng singleton
    public static InventoryManager Instance { get; private set; }

    public List<Item> items = new List<Item>();
    public Transform itemHolder;
    public GameObject itemPrefab;
    public Toggle enableRemoveItem;

    private void Awake()
    {
        if (Instance != null || Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }
    public void Add(Item item)
    {
        items.Add(item);
        //cập nhật lại kho đồ
        DisplayInventoty();
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }
    public void DisplayInventoty()
    {
        //xóa đi trước khi cập nhật lại (tránh trùng lặp và itemPrefab trắng)
        foreach(Transform item in itemHolder)
        {
            Destroy(item.gameObject);
        }

        foreach(Item item in items)
        {
            GameObject obj = Instantiate(itemPrefab, itemHolder);

            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemImage = obj.transform.Find("ItemImage").GetComponent<Image>();

            itemName.text = item.itemName;
            itemImage.sprite = item.image;

            obj.GetComponent<ItemUIController>().SetItem(item);
        }

        EnableRemoveButton();
    }
    void EnableRemoveButton()
    {
        if (enableRemoveItem.isOn)
        {
            foreach(Transform item in itemHolder)
            {
                item.transform.Find("RemoveButton").gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform item in itemHolder)
            {
                item.transform.Find("RemoveButton").gameObject.SetActive(false);
            }
        }
    }
}
