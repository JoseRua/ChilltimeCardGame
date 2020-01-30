using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Main class with all the game's logic
/// </summary>
public class MainController : MonoBehaviour
{
    public static MainController _instance;

    // Game session variables
    private List<CardScript> _allCards = new List<CardScript>();       // contains all cards in-game
    private HashSet<int> _pair = new HashSet<int>();                // used to check if pair is
    private List<CardScript> _pairCards = new List<CardScript>();   // selected cards of a pair
    private int _cardClicks = 0;
    private int _pairTotal = 3;
    private int _totalPairPossible = 0, _successfulPairs = 0;
    private bool _sessionFinished = false;
    private float _timer;
    private string _playerName;

    //Resources
    private Sprite[] _unitCards;
    private Sprite[] _unitCardsLock;

    // Editor variables
    [SerializeField]
    private List<GameObject> _cardsGO;
    [SerializeField]
    private UIController _uiController;

    // Scores variables
    int _movesNumber = 0;
    int _totalTimeElapsed;

    // Load & Save variables
    private List<json_score> _savedScores = new List<json_score>();
    string SCORES_PATH, SESSION_PATH;

    #region Monobehaviour functions
    void Awake() {
        _instance = this;
    }

    void OnDestroy() {
        SaveSession();    
    }

    void Start()
    {
        SCORES_PATH =  Application.dataPath + "scores";

        _unitCards = Resources.LoadAll<Sprite>("unit_cards");
        _unitCardsLock = Resources.LoadAll<Sprite>("unit_cards_lock");

        foreach(GameObject go in _cardsGO) {
            _allCards.Add(go.GetComponent<CardScript>());
        }

        LoadScores();
    }
    #endregion

    #region Game Session Functions
    /// <summary>
    /// Initializes a game session
    /// </summary>
    /// <param name="playerName">Session player's name</param>
    public void InitGameSession(string playerName) {
        SESSION_PATH = Application.dataPath + playerName;
        //1. Set the player's name 
        _playerName = playerName;

        //2. Load player's last session
        // else create new deck
        if (File.Exists(SESSION_PATH)) {
            LoadSession();
        } else {
            CreateDeck();
            
        }

        //3. UI changes
        _uiController.InitGameSession();
    }

    /// <summary>
    /// Creates a new deck of cards taking into account the number of cards of each pair
    /// </summary>
    private void CreateDeck() {
        //1. Decide which imageID (units) will appear: total cards = _pairTotal * numMaxPairs + (restCards)
        List<int> allCardsIDs = new List<int>();
        for(int i = 0; i < _unitCards.Length; i++) {
            allCardsIDs.Add(i);
        }

        List<int> possibleIDs = new List<int>();
        int numMaxPairs = (int)Mathf.Floor(_cardsGO.Count / _pairTotal);
        int restCards = _cardsGO.Count - numMaxPairs*_pairTotal;
        for (int i = 0; i < (numMaxPairs+ restCards); i++) {
            int id = allCardsIDs[Random.Range(0, allCardsIDs.Count-i)];
            if (i < numMaxPairs) {
                possibleIDs.AddRange(new int[] { id, id ,id});
            } else {
                for (int j = 0; j < restCards; j++) {
                    possibleIDs.Add(id);
                }
            }
            allCardsIDs.Remove(id);
        }

        //2. Shuffle
        ShuffleDeck(ref possibleIDs);

        //3. Present deck
        for(int i = 0; i < possibleIDs.Count; i++) {
            _allCards[i].InitCard(possibleIDs[i], i, _unitCards[possibleIDs[i]]);
        }

        //4. Set total pair possible
        _totalPairPossible = (int)Mathf.Floor(_cardsGO.Count / _pairTotal); 

        //5. Start timer
        _timer = Time.time;
        StartCoroutine(CR_Time());

    }

    /// <summary>
    /// Shuffles randomly a deck of cards
    /// </summary>
    /// <param name="list">Reference to the deck of cards</param>
    private void ShuffleDeck(ref List<int> list) {
        System.Random random = new System.Random();
        int n = list.Count;

        for (int i = list.Count - 1; i > 1; i--) {
            int rnd = random.Next(i + 1);

            int value = list[rnd];
            list[rnd] = list[i];
            list[i] = value;
        }
    }

    /// <summary>
    /// Called when a card is clicked, checks for a pair of equal cards
    /// and if all possible pair where found
    /// </summary>
    /// <param name="card"></param>
    public void OnCardClick(CardScript card) {
        _pairCards.Add(card);
        _pair.Add(card.CardID);
        _cardClicks++;
        if (_cardClicks == _pairTotal) {
            //1. Check for successful pair
            if (_pair.Count == 1) {
                foreach(CardScript c in _pairCards) {
                    _allCards[c.CardIndex].LockCard();
                }
                
                _successfulPairs++;
                
                ResetMove();
            } else {
                StartCoroutine(CR_FinishCheckinPair());
            }
            // 2. Check for end game
            if (_successfulPairs < _totalPairPossible) {
                AddMove();
                _uiController.PlayerPanel.SetTries(_movesNumber);
            } else {
                EndSession();
            }
        }
    }

    /// <summary>
    /// Resets every variable for a new session and starts a new session (by creating a deck)
    /// </summary>
    public void ResetSession() {
        foreach (CardScript cs in _allCards) {
            cs.FlipBack();
        }
        ResetMove();
        _successfulPairs = 0;
        _movesNumber = 0;
        _totalTimeElapsed = 0;
        _sessionFinished = false;
        CreateDeck();
    }

    /// <summary>
    /// Called when every possible pair was found. 
    /// Calculates score and add it to the leaderboards
    /// </summary>
    public void EndSession() {
        _sessionFinished = true;
        StopCoroutine(CR_Time());

        // 1. Calculate score and send it
        int score = _movesNumber * 5 + _totalTimeElapsed;

        json_score jScore = new json_score(_playerName, score, TimeElapsedToString());

        _uiController.ScorePanel.AddScore(jScore);

        //2. Files changes
        SaveScore(jScore);
        File.Delete(SESSION_PATH); // if player has finished the game there no need to save the session

        //3. UI 
        _uiController.EndSession(score);
    }

    #endregion

    #region Coroutines
    /// <summary>
    /// Coroutine called to flip back the card of an unsuccessful pair
    /// </summary>
    /// <returns></returns>
    IEnumerator CR_FinishCheckinPair() {
        yield return new WaitForSeconds(0.5f);
        foreach (CardScript c in _pairCards) {
            _allCards[c.CardIndex].FlipBack();
        }

        //3. Reset
        ResetMove();
    }

    /// <summary>
    /// Coroutine called every second to calculate and display the total time passed.
    /// </summary>
    /// <returns></returns>
    IEnumerator CR_Time() {
        while (!_sessionFinished) {
            _totalTimeElapsed = ((int)(Time.time - _timer));
            _uiController.PlayerPanel.SetTime(TimeElapsedToString());
            yield return new WaitForSeconds(1f);
            
        }
    }
    #endregion

    #region Save & Load
    /// <summary>
    /// Loads a game session froma  file based on the player last move
    /// </summary>
    public void LoadSession() {
        //1. Load file
        string json = File.ReadAllText(SESSION_PATH);
        json_gameSession session = JsonConvert.DeserializeObject<json_gameSession>(json);

        //2. Load toalPair possible (this is before loading cards because of total size)
        _totalPairPossible = (int)Mathf.Floor(session.Cards.Length / _pairTotal);

        //3. Load each card
        HashSet<int> cardIDsLocked = new HashSet<int>();
        for (int i = 0; i < session.Cards.Length; i++) {
            _allCards[i].LoadCard(session.Cards[i].CardID, i, _unitCards[session.Cards[i].CardID], session.Cards[i].Turned, session.Cards[i].Locked);
            if (session.Cards[i].Turned){
                if (!session.Cards[i].Locked) {
                    OnCardClick(_allCards[i]);
                } else {
                    cardIDsLocked.Add(_allCards[i].CardID);
                }
            }
        }

        //4. Load session variables
        _timer = -session.TimeElapsed;
        _movesNumber = session.NumMoves;
        _successfulPairs = session.NumPairs;

        //5. Load UI variables

        _uiController.PlayerPanel.SetTries(_movesNumber);
        foreach (int i in cardIDsLocked) {
            _uiController.ResultsPanel.SetResultCard(_unitCards[i]);
        }
        
        StartCoroutine(CR_Time());
    }

    /// <summary>
    /// Saves a game session and player's last move to a file.
    /// </summary>
    public void SaveSession() {
        List<json_card> cards = new List<json_card>();
        foreach(CardScript cs in _allCards) {
            json_card jc = new json_card(cs.CardID, cs.CardIndex, cs.Turned, cs.Locked);
            cards.Add(jc);
        }

        json_gameSession gameSession = new json_gameSession(_playerName,_movesNumber, _successfulPairs, _totalTimeElapsed, cards.ToArray());

        string json = JsonConvert.SerializeObject(gameSession);
        File.Delete(SESSION_PATH);
        Debug.Log(SESSION_PATH);
        File.WriteAllText(SESSION_PATH, json);
    }

    /// <summary>
    /// Saves a score to a file
    /// </summary>
    /// <param name="score"></param>
    public void SaveScore(json_score score) {
        
        _savedScores.Add(score);
        json_scoreArray array = new json_scoreArray();
        array.Scores = _savedScores.ToArray();
        string json = JsonConvert.SerializeObject(array);
        File.Delete(SCORES_PATH);
        File.WriteAllText(SCORES_PATH, json);
    }

    /// <summary>
    /// Loads all scores previously saved
    /// </summary>
    public void LoadScores() {
        if (!File.Exists(SCORES_PATH))
            return;
        string json = File.ReadAllText(SCORES_PATH);
        json_scoreArray array = JsonConvert.DeserializeObject<json_scoreArray>(json);

        _savedScores.AddRange(array.Scores);
        _uiController.ScorePanel.AddScores(array.Scores);

    }
    #endregion

    #region Auxiliars
    public void AddMove() {
        _movesNumber++;
    }

    private void ResetMove() {
        _cardClicks = 0;
        _pair.Clear();
        _pairCards.Clear();
    }

    /// <summary>
    /// Converts the total time elasped into a minute:seconds string
    /// </summary>
    /// <returns>minutes:seconds string</returns>
    private string TimeElapsedToString() {
        int minutes = Mathf.FloorToInt(_totalTimeElapsed / 60);
        int seconds = _totalTimeElapsed - (minutes * 60);
        if (seconds < 10) {
            return minutes + ":0" + seconds;
        }
        return minutes + ":" + seconds;
    }

    public void QuitGame() {
        Application.Quit();
    }
    #endregion
}
