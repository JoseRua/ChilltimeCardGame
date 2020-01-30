using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script for each card that appears in game
/// </summary>
public class CardScript : MonoBehaviour
{
    //Editor Variables
    [SerializeField]
    private Sprite _defaultImage;
    [SerializeField]
    private GameObject _successImage;
    [SerializeField]
    private Animator _animator;

    // Card variables
    private Sprite _cardSprite;
    private int _cardID, _cardIndex;
    private Image _currentImage;
    private bool _turned = false;
    private bool _locked = false;

    //TODO: turn this into a json_card
    public Sprite Image { get => _cardSprite; set => _cardSprite = value; }
    public int CardID { get => _cardID; set => _cardID = value; }
    public int CardIndex { get => _cardIndex; set => _cardIndex = value; }
    public bool Turned { get => _turned; set => _turned = value; }
    public bool Locked { get => _locked; set => _locked = value; }

    public void InitCard(int id, int index, Sprite image) {
        _cardID = id;
        _cardIndex = index;
        _cardSprite = image;
        _currentImage = GetComponent<Image>();
        _successImage.SetActive(false);
    }

    public void LoadCard(int id, int index, Sprite image, bool turned, bool locked) {
        _cardID = id;
        _cardIndex = index;
        _cardSprite = image;
        _currentImage = GetComponent<Image>();
        _turned = turned;
        _locked = locked;
        if (_turned) {
            _currentImage.sprite = _cardSprite;
        }
        if (_locked) {
            _successImage.SetActive(true);
        }
    }

    public void LockCard() {
        _successImage.SetActive(true);
        _locked = true;
    }

    public void OnClick() {
        if (!_turned) {
            _turned = true;
            _animator.SetTrigger("flip");
            MainController._instance.OnCardClick(this);
        }
    }

    public void FlipCard() {
        if (_turned) {
            _currentImage.sprite = _cardSprite;
        } else {
            
            _currentImage.sprite = _defaultImage;
        }
    }

    public void FlipBack() {
        _turned = false;
        _animator.SetTrigger("flip");
    }
}
