using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class shipselect : MonoBehaviour
{
    public GameObject vcam;
    public GameObject[] ships;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        int selected = PlayerPrefs.GetInt("Ship", 0);
        ships[selected].SetActive(true);
        CinemachineVirtualCamera cam = vcam.GetComponent<CinemachineVirtualCamera>();
        cam.Follow = ships[selected].transform;
        gameManager.player = ships[selected];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
