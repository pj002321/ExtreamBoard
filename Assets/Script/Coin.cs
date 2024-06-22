using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool; 

public class Coin : MonoBehaviour
{
    private IObjectPool<Coin> managedPool;

    public void SetManagedPool(IObjectPool<Coin> pool)
    {
        managedPool = pool;
    }

    public void DestroyCoin()
    {
        managedPool.Release(this);
    }
    // Update is called once per frame
    void Update()
    {

        transform.Rotate(0, 1, 0);  
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 3.0f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            transform.Translate(Vector2.zero);
        }
        else
        {
            transform.Translate(Vector2.down *100* Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DestroyCoin();
        }
    }
}
