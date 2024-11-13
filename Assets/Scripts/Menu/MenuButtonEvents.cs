using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Klass som tar hand om event kring meny-knappen samt byte av scen
/// </summary>
public class MenuButtonEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    /// <summary>
    /// Konstuktor f�r MenuButtonEvents
    /// </summary>
    public MenuButtonEvents()
    {

    }

    /// <summary>
    /// Unitys inbyggda funktion f�r n�r en mus �r �ver ett objekt
    /// </summary>
    /// <param name="eventData">Data kring eventet</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        TextMeshProUGUI tmp = GetComponent<TextMeshProUGUI>();

        float multiplier = 1 / 255f;
        float dark1 = 70 * multiplier, dark2 = 170 * multiplier;

        Color tl = new Color(dark1, dark1, dark1);
        Color br = new Color(dark1, dark1, dark1);
        Color tr = new Color(dark2, dark2, dark2);
        Color bl = new Color(dark2, dark2, dark2);

        tmp.colorGradient = new TMPro.VertexGradient(tl, tr, bl, br);
    }

    /// <summary>
    /// Unitys inbyggda funktion f�r n�r en mus l�mnar ett objekt
    /// </summary>
    /// <param name="eventData">Data kring eventet</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        TextMeshProUGUI tmp = GetComponent<TextMeshProUGUI>();

        float multiplier = 1 / 255f;
        float light1 = 136 * multiplier, light2 = 255 * multiplier;

        Color tl = new Color(light1, light1, light1);
        Color br = new Color(light1, light1, light1);
        Color tr = new Color(light2, light2, light2);
        Color bl = new Color(light2, light2, light2);

        tmp.colorGradient = new TMPro.VertexGradient(tl, tr, bl, br);
    }

    /// <summary>
    /// Byter scen till scen 1, d�r kort v�ljs
    /// </summary>
    public virtual void PlayGame()
    {
        SwitchScene(1);
    }

    public void ClickButtonOnCollection(){
        SwitchScene(3);
    }

    /// <summary>
    /// Byter scen av spelet
    /// </summary>
    /// <param name="sceneIndex">Index p� vilken scen som ska synas nu</param>
    protected void SwitchScene(int sceneIndex)
    {
        switch(sceneIndex)
        {
            case 0:
                AsyncOperation menu = SceneManager.LoadSceneAsync(0);
            break;
            case 1:
                AsyncOperation operation = SceneManager.LoadSceneAsync(1);
                break;
            case 2:
                List<string> cardDeck = Resources.Load<PublicData>("PublicData").cardDeck;
                if (cardDeck.Count > 0)
                {
                    List<string> cardDeckCopy = new List<string>();
                    foreach (string card in cardDeck)
                    {
                        cardDeckCopy.Add(card);
                    }
                    foreach (string card in cardDeckCopy)
                    {
                        cardDeck.Remove(card);
                    }
                }

                if (GameObject.Find("Scripts").GetComponent<SelectDeck>().cardsChosen == 10)
                {

                    bool canPlayGame = true;

                    GameObject cards = GameObject.Find("Cards").gameObject;
                    if (cards.transform.childCount != 10)
                    {
                        canPlayGame = false;
                    }
                    else
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            string name = cards.transform.GetChild(i).name;
                            cardDeck.Add(name);
                        }
                    }

                    if (canPlayGame)
                    {
                        operation = SceneManager.LoadSceneAsync(2);
                    }
                }
            break;
            case 3:
                AsyncOperation collection = SceneManager.LoadSceneAsync(3);
            break;
                default:
                    break; 
        }
    }
}
