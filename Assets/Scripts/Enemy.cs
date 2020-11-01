using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public GameObject indicator;
    public GameObject explosionEffect;
    public GameObject spawnEffect;
    public GameManager gameManager;
    public float speed = 1f;
    bool destroyed = false;
    bool isActive = false;
    Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }
        viewIndicator();
    }

    private void viewIndicator()
    {
        if (!renderer.isVisible)
        {
            RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
            if (ray.collider != null)
            {
                if (ray.collider.CompareTag("Player"))
                {
                    indicator.SetActive(true);
                    indicator.transform.position = ray.point;
                    indicator.transform.up = transform.position-indicator.transform.position;
                   
                }
            }
        }
        else
        {
            if (indicator.activeSelf)
            {
                indicator.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Bullet") && isActive)
        {
            if (!destroyed)
            {
                gameManager.noOfObstacles--;
                gameManager.AddScore(20);
                destroyed = true;
                isActive = false;
            }
            Destroy(gameObject);
            GameObject explosion = Instantiate(explosionEffect,transform.position,Quaternion.identity);
            Destroy(explosion, 3f);
        }
        else if(collision.collider.CompareTag("Player")&&isActive)
        {
            gameManager.GameOver();
            Destroy(collision.collider.gameObject);
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 3f);
        }
        
    }
    IEnumerator Spawn()
    {
        
        GameObject spawn = Instantiate(spawnEffect, transform.position, Quaternion.identity);
        this.GetComponent<PolygonCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.75f);
        Destroy(spawn);
        this.GetComponent<PolygonCollider2D>().enabled = true;
        isActive = true;
    }


}
