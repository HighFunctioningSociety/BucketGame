using UnityEngine;
using UnityEngine.UI;

public class Wallet : MonoBehaviour
{
    public Text walletAmount;


    private void Start()
    {
        walletAmount.text = Inventory.instance.walletAmount.ToString();
    }

    public void UpdateWalletAmount(int newAmount)
    {
        walletAmount.text = newAmount.ToString();
    }
}
