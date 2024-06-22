using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GeneraterCoin : MonoBehaviour
{
    public GameObject coinPrefab;
    public Vector2 spawnAreaSize;
    public int numberOfCoins = 10;

    private IObjectPool<Coin> pool;
    public void Awake()
    {
        pool = new ObjectPool<Coin>(CreateCoin, OnGetCoin, OnDestroyCoin, OnReleaseCoin, maxSize: 40);
    }
    void Start()
    {
        GenerateCoins();
    }

    void GenerateCoins()
    {
        for (int i = 0; i < numberOfCoins; i++)
        {
            var coin = pool.Get();
        }
    }

    private Coin CreateCoin()
    {
        Vector2 spawnPosition = new Vector2(
                Random.Range(transform.position.x - spawnAreaSize.x, transform.position.x + spawnAreaSize.x),
                Random.Range(transform.position.y - spawnAreaSize.y, transform.position.y + spawnAreaSize.y)
        );
        Coin coin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity).GetComponent<Coin>();
        coin.SetManagedPool(pool);
        return coin;
    }

    private void OnGetCoin(Coin coin)
    {
        coin.gameObject.SetActive(true);
    }
    private void OnReleaseCoin(Coin coin)
    {
        coin.gameObject.SetActive(false);
    }
    private void OnDestroyCoin(Coin coin)
    {
        Destroy(coin.gameObject);
    }
}
