using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    public static DropManager _DropManager;

    private void Awake()
    {
        _DropManager = this;
    }

    public Currency coin_1;
    public Currency coin_5;
    public Currency coin_20;

    public void CalculateAndDropEnemyCoins(EnemyContainer _enemy)
    {
        int minCurrency = _enemy.enemyStats.currencyDroppedMin;
        int maxCurrency = _enemy.enemyStats.currencyDroppedMax;
        int amountToDrop = CalculateDroppedValue(minCurrency, maxCurrency);
        Debug.Log("Amount of coins to generate: " + amountToDrop);

        SpawnCoins(amountToDrop, _enemy.transform);
    }

    public void DropChestContents(ChestController _chest)
    {
        int amountToDrop = _chest.currencyAmount;
        GameObject[] chestItemContents = _chest.ItemContents;
        Debug.Log("Amount of coins to generate: " + amountToDrop);

        SpawnCoins(amountToDrop, _chest.transform);
        SpawnItems(chestItemContents, _chest.transform);
    }

    public void SpawnItems(GameObject[] itemsToSpawn, Transform spawnLocation)
    {
        Vector2 velocity;
        Rigidbody2D rb;
        foreach (GameObject item in itemsToSpawn)
        {
            velocity = GenerateVelocity();
            GameObject itemClone = Instantiate(item, spawnLocation.transform.position, spawnLocation.transform.rotation);
            rb = item.GetComponent<Rigidbody2D>();
            rb.AddForce(velocity, ForceMode2D.Impulse);
        }
    }

    private void SpawnCoins(int amountToDrop, Transform spawnLocation)
    {
        int NumCoins_20 = amountToDrop / 20;
        amountToDrop -= NumCoins_20 * 20;


        int NumCoins_5 = amountToDrop / 5;
        amountToDrop -= NumCoins_5 * 5;

        int NumCoins_1 = amountToDrop;
        Debug.Log("Coin_1: " + NumCoins_1 + ", Coin_5: " + NumCoins_5 + ", Coin_20: " + NumCoins_20);

        SpawnCoins_1(NumCoins_1, spawnLocation);
        SpawnCoins_5(NumCoins_5, spawnLocation);
        SpawnCoins_20(NumCoins_20, spawnLocation);
    }


    private void SpawnCoins_1(int _numCoins_1, Transform spawnLocation)
    {
        GameObject coin;
        Rigidbody2D rb;
        Vector2 velocity;
        for (int i = 0; i < _numCoins_1; i++)
        {
            velocity = GenerateVelocity();
            coin = Instantiate(coin_1.coinPrefab, spawnLocation.transform.position + new Vector3(0, 5, 0), spawnLocation.transform.rotation);
            rb = coin.GetComponent <Rigidbody2D>();
            rb.AddForce(velocity, ForceMode2D.Impulse);
        }
    }

    private void SpawnCoins_5(int _numCoins_5, Transform spawnLocation)
    {
        GameObject coin;
        Rigidbody2D rb;
        Vector2 velocity;
        for (int i = 0; i < _numCoins_5; i++)
        {
            velocity = GenerateVelocity();
            coin = Instantiate(coin_5.coinPrefab, spawnLocation.transform.position + new Vector3 (0, 5, 0), spawnLocation.transform.rotation);
            rb = coin.GetComponent<Rigidbody2D>();
            rb.AddForce(velocity, ForceMode2D.Impulse);
        }
    }

    private void SpawnCoins_20(int _numCoins_20, Transform spawnLocation)
    {
        GameObject coin;
        Rigidbody2D rb;
        Vector2 velocity;
        for (int i = 0; i < _numCoins_20; i++)
        {
            velocity = GenerateVelocity();
            coin = Instantiate(coin_20.coinPrefab, spawnLocation.transform.position + new Vector3(0, 5, 0), spawnLocation.transform.rotation);
            rb = coin.GetComponent<Rigidbody2D>();
            rb.AddForce(velocity, ForceMode2D.Impulse);
        }
    }

    private Vector2 GenerateVelocity()
    {
        float xVariance = Random.Range(-80, 80);
        float yVariance = Random.Range(80, 140);
        return new Vector2(xVariance, yVariance);
    }

    private int CalculateDroppedValue(int minCurrency, int maxCurrency)
    {
       return Random.Range(minCurrency, maxCurrency);
    }
}
