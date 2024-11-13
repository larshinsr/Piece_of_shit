using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Klass som tar hand om algoritmer kring val av kort till spelet
/// </summary>
public class SelectDeck : MenuButtonEvents
{

    int cardsInMenu = 0; 
    public int cardsChosen = 0;
    
    Vector2 startPos = new Vector2(-600f, 230);

    public List<GameObject> chosenCards;

    List<List<GameObject>> pages = new List<List<GameObject>>();

    /// <summary>
    /// Konstruktor f�r SelectDeck
    /// </summary>
    public SelectDeck()
    {

    }
    
    /// <summary>
    /// Unitys inbyggda start-funktion
    /// </summary>
    private void Start()
    {
        ImportCard("Annushka");
        ImportCard("Azazello");
        ImportCard("The_hippopotamus_cat");
        ImportCard("Gella");
        ImportCard("Ivan_the_homeless");
        ImportCard("Koroviev");
        ImportCard("Levi_Matthew");
        ImportCard("Margarita");
        ImportCard("Natasha");
        ImportCard("Nikanor_Ivanovich_barefoot");
        ImportCard("Not_a_good_apartment");
        ImportCard("Pontius_Pilate");
        ImportCard("Roman_Centurion");
        ImportCard("Satan's_Ball");
        ImportCard("Stepan_Likhodeev");

        ReloadPage(1);
    }

    /// <summary>
    /// L�gger till ett kort bland alternativen f�r kort man kan v�lja
    /// </summary>
    /// <param name="cardName">Namnet p� kortet</param>
    void ImportCard(string cardName)
    {

        GameObject example = GameObject.Find("Card Example");

        Card card = Resources.Load<Card>("Cards/" + cardName);

        GameObject cardObject = GameObject.Instantiate(example);
        cardObject.transform.parent = GameObject.Find("Card Options").transform;
        cardObject.name = cardName; 

        float xMargin = 400f, yMargin = 420f;

        int index = cardsInMenu;
        if(cardsInMenu >= 8)
        {
            index = cardsInMenu % 8; 
        }

        int xIndex = index;
        int yIndex = 0; 

        if(index >= 4)
        {
            xIndex -= 4;
            yIndex = 1;
        }

        int page = (int)(cardsInMenu / 8 + 1);
        if(pages.Count < page)
        {
            List<GameObject> list = new List<GameObject>();
            pages.Add(list);
        }

        cardObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPos.x + xMargin * xIndex, startPos.y - yMargin * yIndex);

        cardObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = card.image;

        cardObject.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = card.name;
        cardObject.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<Text>().text = card.description; 
        cardObject.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().text = card.mana + ""; 
        cardObject.transform.GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetComponent<Text>().text = card.attack + ""; 
        cardObject.transform.GetChild(0).GetChild(1).GetChild(4).GetChild(0).GetComponent<Text>().text = card.health + "";

        if(card.cardType == CardType.Weapon)
        {
            cardObject.transform.GetChild(0).GetChild(1).GetChild(3).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Cards/Armor");

            cardObject.transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Cards/Durability");
            cardObject.transform.GetChild(0).GetChild(1).GetChild(4).GetChild(0).GetComponent<Text>().text = card.durability + "";
        }
        else if(card.cardType == CardType.Spell)
        {
            cardObject.transform.GetChild(0).GetChild(1).GetChild(3).GetComponent<Image>().enabled = false;
            cardObject.transform.GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetComponent<Text>().enabled = false;

            cardObject.transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<Image>().enabled = false;
            cardObject.transform.GetChild(0).GetChild(1).GetChild(4).GetChild(0).GetComponent<Text>().enabled = false;
        }

        cardObject.AddComponent<CardToDeck>();
        cardObject.AddComponent<CanvasGroup>();

        //cardObject.transform.parent = GameObject.Find("Card Options/Pages").transform.GetChild(page-1);
        pages[page-1].Add(cardObject);

        cardsInMenu++;

    }

    /// <summary>
    /// S�tter ig�ng spelet
    /// </summary>
    public override void PlayGame()
    {
        SwitchScene(2);
    }

    public void BackMenu()
    {
        SwitchScene(0);
    }

    /// <summary>
    /// Byter sida bland kort-alternativen
    /// </summary>
    /// <param name="side">Den sida man "bl�ddrar �ver", dvsa "Left" eller "Right"</param>
    public void SwitchPage(string side)
    {

        int currentPage = 1; 
        try
        {
            currentPage = int.Parse(GameObject.Find("Page Text").GetComponent<Text>().text[5].ToString());
        } catch(System.FormatException fe) { }

        int amountOfPages = (int)(cardsInMenu / 8 + 1);

        switch(side)
        {
            case "Left": case "left":
                if (currentPage > 1)
                    --currentPage; 
                break;
            case "Right": case "right":
                if (currentPage < amountOfPages)
                    ++currentPage;
                break;
            default:
                break; 
        }

        GameObject.Find("Page Text").GetComponent<Text>().text = "Page " + currentPage;

        ReloadPage(currentPage);

    }

    /// <summary>
    /// Byter vilka kort som syns, efter att en sida har v�nds bland kort-alternativen
    /// </summary>
    /// <param name="page">Indexet p� den sida som nu ska visas</param>
    public void ReloadPage(int page)
    {

        for(int i = 0; i < pages.Count; i++)
        {
            bool b = false; 
            if(i+1 == page)
            {
                b = true; 
            }

            if(pages[i].Count > 0)
            {
                foreach (GameObject obj in pages[i])
                {
                    Transform tr = obj.transform; 

                    tr.GetChild(0).GetChild(0).GetComponent<Image>().enabled = b;
                    tr.GetChild(0).GetChild(1).GetComponent<Image>().enabled = b;

                    tr.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().enabled = b;
                    tr.GetChild(0).GetChild(1).GetChild(1).GetComponent<Text>().enabled = b;

                    tr.GetChild(0).GetChild(1).GetChild(2).GetComponent<Image>().enabled = b;
                    tr.GetChild(0).GetChild(1).GetChild(3).GetComponent<Image>().enabled = b;
                    tr.GetChild(0).GetChild(1).GetChild(4).GetComponent<Image>().enabled = b;

                    tr.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().enabled = b;
                    tr.GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetComponent<Text>().enabled = b;
                    tr.GetChild(0).GetChild(1).GetChild(4).GetChild(0).GetComponent<Text>().enabled = b;

                    tr.GetComponent<CardToDeck>().enabled = b;

                    tr.GetComponent<CanvasGroup>().blocksRaycasts = b;

                    Card card = Resources.Load<Card>("Cards/" + obj.name);
                    if(card.cardType == CardType.Spell)
                    {
                        tr.GetChild(0).GetChild(1).GetChild(3).GetComponent<Image>().enabled = false;
                        tr.GetChild(0).GetChild(1).GetChild(4).GetComponent<Image>().enabled = false;

                        tr.GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetComponent<Text>().enabled = false;
                        tr.GetChild(0).GetChild(1).GetChild(4).GetChild(0).GetComponent<Text>().enabled = false;
                    }
                }
            }
        }

    }

}
