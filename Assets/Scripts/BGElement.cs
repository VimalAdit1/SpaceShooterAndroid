using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGElement : MonoBehaviour
{
    public GameObject player;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if((this.transform.position-player.transform.position).sqrMagnitude>20000)
        {
            gameManager.noOfBGElements--;
            Destroy(gameObject);
        }
    }
}
