using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for all events in the player panel
/// </summary>
public class UI_PlayerPanel : MonoBehaviour
{
    [SerializeField]
    private Text _playerNameText;
    [SerializeField]
    private Text _triesText;
    [SerializeField]
    private Text _timeElapsedText;

    public void SetTries(int num) {
        _triesText.text = num.ToString();
    }

    public void SetTime(string time) {
        _timeElapsedText.text = time;
    }
}
