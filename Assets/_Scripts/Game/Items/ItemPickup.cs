using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        if (_colInfo.GetComponent<PlayerContainer>() != null)
        {
            PickUp();
        }
    }

    public void PickUp()
    {
        Debug.Log("Picking up " + item.itemName);
        FindObjectOfType<Inventory>().AddItem(item);
        Destroy(gameObject);
    }
}
