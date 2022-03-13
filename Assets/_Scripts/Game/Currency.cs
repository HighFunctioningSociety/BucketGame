using UnityEngine;

[CreateAssetMenu(fileName = "New Currency", menuName = "Inventory/Currency")]
public class Currency : ScriptableObject
{
    public string currencyType;
    public int value = 1;
    public GameObject coinPrefab;
}
