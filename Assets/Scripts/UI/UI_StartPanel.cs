using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for all events in the start panel
/// </summary>
public class UI_StartPanel : MonoBehaviour
{

    [SerializeField]
    private InputField _playerNameIF;

    public void OnBtnClick_Start() {
        
        MainController._instance.InitGameSession(_playerNameIF.text);
    }
}
