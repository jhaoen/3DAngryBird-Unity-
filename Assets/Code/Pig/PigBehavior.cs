using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigBehavior : MonoBehaviour
{
    private float currentHealth = 1; // 當前生命值 (基於 mass)
    public GameObject explosionEffect; // 爆炸效果的粒子系統 Prefab
    private Bounds planeBounds;
    private Rigidbody rb;
    private float floorHeight;
    private float FallingThreshold = -10f;
    private bool isFalling = false;
    private SphereCollider collider;

    void Start()
    {
        GameManager.Instance.RegisterPig();
        planeBounds = GameManager.planeBounds;
        floorHeight = GameManager.planeBounds.min.y;
        collider = GetComponent<SphereCollider>();
        if (collider == null)
        {
            Debug.LogError("PigBehavior: Collider is null");

        }

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("PigBehavior: Rigidbody is missing on the pig!");
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        // 獲取碰撞物件的 Rigidbody
        Rigidbody otherRb = collision.rigidbody;

        // 確保碰撞物件有 Rigidbody
        if (otherRb != null)
        {
            // 檢查碰撞方向
            Vector3 collisionNormal = collision.contacts[0].normal; // 碰撞點的法線
            Vector3 collisionDirection = -collisionNormal; // 碰撞方向（法線的反方向）

            // 確保碰撞方向是從上方壓下
            if (Vector3.Dot(collisionDirection, Vector3.up) > 0.5f) // 角度接近豬的上方
            {
                float otherMass = otherRb.mass; 

                // 如果碰撞物件的質量大於豬的剩餘生命值，豬死亡
                if (otherMass >= currentHealth)
                {
                    Die();
                }
                else
                { 
                    currentHealth -= otherMass;
                    Debug.Log($"豬受到傷害！剩餘生命值：{currentHealth}");
                }
            }

            if(transform.position.y == 0)
            {
                Debug.Log($"豬摔死了");
                Die();
            }
        }
    }

    void CheckOutOfBounds()
    {
        if (transform.position.x < planeBounds.min.x || transform.position.x > planeBounds.max.x ||
        transform.position.z < planeBounds.min.z || transform.position.z > planeBounds.max.z)
        {
            Die();
        }
    }

    void CheckFall()
    {
        
        if (rb.velocity.y < FallingThreshold)
        {

            isFalling = true;
        }
    }

    void FallToGround()
    {
        if(isFalling)
        {
            Debug.Log($"fall down velocity : {rb.velocity.y}");
        }
        if(isFalling && rb.position.y <= floorHeight + collider.radius)
        {
            Debug.Log($"豬摔死了");
            Die();
        }
    }
    void Die()
    {

        // 生成爆炸效果
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // 銷毀豬物件
        Debug.Log("豬死亡！");
        Destroy(gameObject);
        GameManager.Instance.UnregisterPig();

    }
    // Update is called once per frame
    void Update()
    {
        CheckFall();
        FallToGround();
    }
}
