using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

/// <summary>
/// Tar hand om event kring n�r kort tas bort och l�ggs till bland valda kort 
/// </summary>
public class CardToDeck : MonoBehaviour, IPointerClickHandler
{

    /// <summary>
    /// Konstruktor f�r CardToDeck
    /// </summary>
    public CardToDeck()
    {

    }

    /// <summary>
    /// Unitys inbyggda startfunktion
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Unitys inbyggda startfunktion kring n�r man klickar p� ett objekt 
    /// </summary>
    /// <param name="eventData">Data kring eventet</param>
    public void OnPointerClick(PointerEventData eventData)
    {

        if(transform.parent.name == "Card Options")
        {
            
            int k = 0; 
            foreach(GameObject obj in GameObject.Find("Scripts").GetComponent<SelectDeck>().chosenCards)
            {
                if (obj.name == this.name)
                    k++;
            }

            if(k < 2)
            {
                GameObject example = GameObject.Find("Card Deck Example");
                GameObject cardObject = GameObject.Instantiate(example);

                cardObject.name = this.name;
                cardObject.transform.parent = GameObject.Find("Cards").transform;

                cardObject.AddComponent<CardToDeck>();

                int cardsChosen = GameObject.Find("Scripts").GetComponent<SelectDeck>().cardsChosen;

                Card card = Resources.Load<Card>("Cards/" + this.name);

                cardObject.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = card.mana + "";
                cardObject.transform.GetChild(3).GetComponent<Text>().text = card.name + "";

                GameObject.Find("Card Count").transform.GetChild(0).GetComponent<Text>().text = (cardsChosen + 1) + " из 10";
                GameObject.Find("Scripts").GetComponent<SelectDeck>().cardsChosen++;

                GameObject.Find("Scripts").GetComponent<SelectDeck>().chosenCards.Add(cardObject);

                ReloadChosenDeck();
            }

        }
        else if(transform.parent.name == "Cards")
        {
            int cardsChosen = GameObject.Find("Scripts").GetComponent<SelectDeck>().cardsChosen;

            GameObject.Find("Scripts").GetComponent<SelectDeck>().cardsChosen--;
            GameObject.Find("Card Count").transform.GetChild(0).GetComponent<Text>().text = (cardsChosen - 1) + " из 10";

            GameObject.Find("Scripts").GetComponent<SelectDeck>().chosenCards.Remove(this.gameObject);
            ReloadChosenDeck();

            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// L�ser om listan med valda kort 
    /// </summary>
    void ReloadChosenDeck()
    {

        List<GameObject> cardDeckOrder = new List<GameObject>();
        int maxMana = 10; 
        for(int i = 0; i <= maxMana; i++)
        {
            List<GameObject> objectsByName = new List<GameObject>();

            foreach (GameObject card in GameObject.Find("Scripts").GetComponent<SelectDeck>().chosenCards)
            {
                try
                {
                    int mana = int.Parse(card.transform.GetChild(2).GetChild(0).GetComponent<Text>().text);
                    if (mana == i)
                    {
                        objectsByName.Add(card);
                    }
                } catch(System.FormatException fe) { }
            }

            GameObject[] objectsarray = objectsByName.ToArray();
            objectsarray = objectsarray.OrderBy(go => go.name).ToArray();

            for(int j = 0; j < objectsarray.Length; j++)
            {
                cardDeckOrder.Add(objectsarray[j]);
            }
        }

        for(int i = 0; i < cardDeckOrder.Count; i++)
        {
            cardDeckOrder[i].transform.SetSiblingIndex(i);
        }

    }
}
