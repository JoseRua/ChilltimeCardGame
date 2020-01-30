using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible keeping the scores of the game 
/// and for all UI events in the score panel
/// </summary>
public class UI_ScorePanel : MonoBehaviour
{
    private SortedDictionary<int, json_score> _elementsList = new SortedDictionary<int, json_score>();  // List which keeps all the scores so far

    [SerializeField]
    private GameObject _scrollViewContainer;
    [SerializeField]
    private GameObject _elementPrefab;
    
    public void AddScores(json_score[] scores) {
        foreach(json_score score in scores) {
            AddScore(score);
        }
    }

    public void AddScore(json_score jScore) {
        _elementsList.Add(jScore.Score, jScore);

        GameObject element = Instantiate(_elementPrefab, _scrollViewContainer.transform);

        int i = 0;
        foreach(KeyValuePair<int, json_score> pair in _elementsList) {
            GameObject go = _scrollViewContainer.transform.GetChild(i).gameObject;
            UI_ScoreElement ui_element = go.GetComponent<UI_ScoreElement>();
            ui_element.PlaceText.text = (i+1).ToString();
            ui_element.PlayerText.text = pair.Value.PlayerName;
            ui_element.ScoreText.text = pair.Value.Score.ToString();
            ui_element.TimeText.text = pair.Value.Time;
            i++;
        }
        
    }
}
