using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for all events in the end panel
/// </summary>
public class UI_EndPanel : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    public Text ScoreText { get => _scoreText; set => _scoreText = value; }
}
