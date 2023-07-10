using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public GameObject inventoryUI;
    public Transform itemsParent;
    public Text itemNameText;
    public Text itemDescriptionText;

    private List<GameObject> items = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void AddItem(GameObject itemObject)
    {
        items.Add(itemObject);
        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (GameObject itemObject in items)
        {
            Item item = itemObject.GetComponent<Item>();

            GameObject itemUI = new GameObject();
            itemUI.transform.SetParent(itemsParent);

            Image itemImage = itemUI.AddComponent<Image>();
            itemImage.sprite = GetItemSprite(item);

            Button itemButton = itemUI.AddComponent<Button>();
            itemButton.onClick.AddListener(() => DisplayItemDescription(item));

            EventTrigger eventTrigger = itemUI.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => { DisplayItemDescription(item); });
            eventTrigger.triggers.Add(entry);

            itemUI.transform.localScale = Vector3.one;
        }
    }

    private Sprite GetItemSprite(Item item)
    {
        SpriteRenderer itemRenderer = item.GetComponent<SpriteRenderer>();
        if (itemRenderer != null)
        {
            return itemRenderer.sprite;
        }

        return null;
    }

    private void DisplayItemDescription(Item item)
    {
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;
    }
}
