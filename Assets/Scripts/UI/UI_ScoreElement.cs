using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script for each score element which showes up in the score panel
/// </summary>
public class UI_ScoreElement : MonoBehaviour
{
    [SerializeField]
    private Text _placeText;
    [SerializeField]
    private Text _playerText;
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _timeText;
    
    public Text PlaceText { get => _placeText; set => _placeText = value; }
    public Text PlayerText { get => _playerText; set => _playerText = value; }
    public Text ScoreText { get => _scoreText; set => _scoreText = value; }
    public Text TimeText { get => _timeText; set => _timeText = value; }

}
