using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class Shopmanager : MonoBehaviour
{
    public GameObject[] ships;
    public Button right;
    public Button left;
    public Button buy;
    public Button play;
    public Text coinsText;
    public Text scoreText;
    public GameManager gameManager;
    int selected;
    int coins;
    String owned;
    String[] ownedShips;
    HashSet<String> shipsList = new HashSet<string>();
    // Start is called before the first frame update
    void Start()
    {
        selected = PlayerPrefs.GetInt("Ship", 0);
        coins = PlayerPrefs.GetInt("Coins", 0);
        owned = PlayerPrefs.GetString("Owned", "0");
        ownedShips = owned.Split(',');
        foreach (String ship in ownedShips)
        {
            shipsList.Add(ship);
        }
        scoreText.text = PlayerPrefs.GetInt("highScore",0).ToString();
    }
    // Update is called once per frame
    void Update()
    {
        checkinput();
        right.interactable = !(selected == ships.Length - 1);
        left.interactable = !(selected == 0);
        coinsText.text = coins.ToString();
    }

    private void checkinput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Right();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Left();
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (play.interactable)
            { 
                gameManager.Play();
            }
        }
    }

    void changeShip(int index)
    {
        for(int i=0;i<ships.Length;i++)
        {
            ships[i].SetActive((i == index));
        }
        if(isShipUnlocked(selected))
        {
            play.interactable = true;
            buy.gameObject.SetActive(false);
            PlayerPrefs.SetInt("Ship", selected);
        }
        else
        {
            buy.gameObject.SetActive(true);
            int shipPrice = ships[selected].GetComponent<Ship>().price;
            buy.GetComponentInChildren<Text>().text = shipPrice.ToString();
            if(coins>=shipPrice)
            {
                buy.interactable = true;
            }
            else
            {
                buy.interactable = false;
            }
            play.interactable = false;
        }
    }

    private bool isShipUnlocked(int selected)
    {
        return shipsList.Contains(selected.ToString());
    }

    public void Right()
    {
        if(selected < ships.Length-1)
        {
            selected++;
            changeShip(selected);
        }
    }
    public void Left()
    {
        if (selected > 0)
        {
            selected--;
            changeShip(selected);
        }
    }
    public void Buy()
    {
        int shipPrice = ships[selected].GetComponent<Ship>().price;
        coins = coins - shipPrice;
        PlayerPrefs.SetInt("Coins", coins);
        owned += "," + selected;
        shipsList.Add(selected.ToString());
        PlayerPrefs.SetString("Owned", owned);
        buy.gameObject.SetActive(false);
        play.interactable = true;
    }
}
