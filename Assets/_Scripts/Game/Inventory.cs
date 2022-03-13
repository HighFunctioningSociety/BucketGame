using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<Item> items = new List<Item>();

    public int space = 9;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int walletAmount = 0;
    public int walletSize = 999;

    public delegate void OnWalletChanged();
    public OnWalletChanged onWalletChangedCallback;

    public AudioSource coinJingleSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCurrency(int amountAdded)
    {
        walletAmount = Mathf.Clamp(walletAmount + amountAdded, 0, walletSize);
        coinJingleSource.Play();
        InvokeWalletChangedCallback();
    }

    public void RemoveCurrency(int amountRemoved)
    {
        walletAmount = Mathf.Clamp(walletAmount - amountRemoved, 0, walletSize);
        coinJingleSource.Play();
        InvokeWalletChangedCallback();
    }

    public bool AddItem(Item item)
    {
        if (items.Count >= space)
        {
            return false;
        }

        items.Add(item);
        InvokeItemChangedCallback();
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        InvokeItemChangedCallback();
    }

    private void InvokeItemChangedCallback()
    {
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    private void InvokeWalletChangedCallback()
    {
        if (onWalletChangedCallback != null)
        {
            onWalletChangedCallback.Invoke();
        }
    }
}
