using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for all events in the results panel
/// </summary>
public class UI_ResultsPanel : MonoBehaviour
{
    [SerializeField]
    private Sprite _defaultImage; 

    public List<GameObject> _resultCardsGO;

    private int _slotIndex = 0;


    public void SetResultCard(Sprite cardImage) {
        _resultCardsGO[_slotIndex].GetComponent<Image>().sprite = cardImage;
        _slotIndex++;
    }

    public void ResetResultCards() {
        foreach(GameObject go in _resultCardsGO) {
            go.GetComponent<Image>().sprite = _defaultImage;
        }
    }
}
