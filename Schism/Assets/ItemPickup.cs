public void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        Item item = GetComponent<Item>();
        GameObject itemObject = transform.gameObject; // Get the parent game object

        // Add the item to the inventory
        InventoryManager.instance.AddItem(itemObject);

        // Disable the item game object
        item.ResetItem();
    }
}
