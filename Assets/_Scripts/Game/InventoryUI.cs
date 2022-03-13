using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;
    public InventorySlot[] slots;

    public Transform itemsParent;
    public Wallet wallet;

    private bool inventoryActive = false;


    void Start()
    {
        Inputs._inputs.onInventoryCallback += ToggleInventory;

        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateInventoryUI;
        inventory.onWalletChangedCallback += UpdateWalletUI;

        slots = GetComponentsInChildren<InventorySlot>();
        itemsParent.gameObject.SetActive(false);
    }

    void UpdateInventoryUI()
    {
        Debug.Log("Updating Inventory UI");
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    void UpdateWalletUI()
    {
        Debug.Log("Updating Wallet UI");
        wallet.UpdateWalletAmount(inventory.walletAmount);
    }

    void ToggleInventory()
    {
        inventoryActive = !inventoryActive;
        itemsParent.gameObject.SetActive(inventoryActive);
        if (inventoryActive)
        {
            UpdateInventoryUI();
        }
    }
}
