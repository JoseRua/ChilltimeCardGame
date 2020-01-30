using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classes which manages all the UI elements in the game.
/// Whenever there is a change in the UI, it is clled by this class
/// </summary>
public class UIController : MonoBehaviour
{
    #region Variables
    // Editor variables
    [SerializeField]
    private UI_ResultsPanel _resultsPanel;
    [SerializeField]
    private UI_PlayerPanel _playerPanel;
    [SerializeField]
    private UI_ScorePanel _scorePanel;
    [SerializeField]
    private GameObject _gamePanel;
    [SerializeField]
    private GameObject _startPanel;
    [SerializeField]
    private UI_EndPanel _endPanel;
    [SerializeField]
    private GameObject _gameTab;
    [SerializeField]
    private GameObject _scoresTab;
    #endregion

    #region Properties
    public UI_ResultsPanel ResultsPanel { get => _resultsPanel; set => _resultsPanel = value; }
    public UI_PlayerPanel PlayerPanel { get => _playerPanel; set => _playerPanel = value; }
    public UI_ScorePanel ScorePanel { get => _scorePanel; set => _scorePanel = value; }
    public GameObject GamePanel { get => _gamePanel; set => _gamePanel = value; }
    public GameObject StartPanel { get => _startPanel; set => _startPanel = value; }

    #endregion

    public void OnCLick_GameTab() {
        SetVisibleGamePanel(true);
        SetVisibleScorePanel(false);
    }

    public void OnCLick_ScoreTab() {
        SetVisibleGamePanel(false);
        SetVisibleScorePanel(true);
    }

    public void InitGameSession() {
        _startPanel.SetActive(false);
        SetVisibleGamePanel(true);
        _gameTab.SetActive(true);
        _scoresTab.SetActive(true);
    }

    public void SetVisibleGamePanel(bool b) {
        _gamePanel.SetActive(b);
    }
    public void SetVisibleScorePanel(bool b) {
        _scorePanel.gameObject.SetActive(b);
    }
    public void SetVisibleStartPanel(bool b) {
        _startPanel.SetActive(b);
    }

    public void EndSession(int score) {
        _endPanel.gameObject.SetActive(true);
        _endPanel.ScoreText.text = "You scored " + score + "points!";
    }

    public void ResetSession() {
        _endPanel.gameObject.SetActive(false);
        _resultsPanel.ResetResultCards();
        _playerPanel.SetTries(0);
        _playerPanel.SetTime("0:00");
        MainController._instance.ResetSession();
    }

    public void QuitGame() {
        MainController._instance.QuitGame();
    }
}
