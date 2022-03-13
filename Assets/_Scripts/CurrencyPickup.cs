using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyPickup : MonoBehaviour
{
    public Currency currency;
    
    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        if (_colInfo.GetComponent<PlayerContainer>() != null)
        {
            PickUp();
        }
    }

    public void PickUp()
    {
        Debug.Log("Picking up " + currency.currencyType);
        FindObjectOfType<Inventory>().AddCurrency(currency.value);
        Destroy(gameObject);
    }
}
