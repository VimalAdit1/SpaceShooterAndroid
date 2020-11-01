using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Start is called before the first frame update
    Renderer renderer;
    public GameObject indicator;
    public GameObject player;
    public GameManager gameManager;
    public int gunNo;
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
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
                    indicator.transform.up = transform.position - indicator.transform.position;
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
        if(collision.collider.CompareTag("Player"))
        {
            gameManager.AddScore(50);
            Destroy(gameObject);
        }
    }

}
