using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //Score of player
    [SerializeField]
    private Text PlayerText;   
    //Error 
    [SerializeField]
    private Text NotExistText;
    [SerializeField]
    private Text DeleteText;
    [SerializeField]
    private Text ChangeLetterText;
    [SerializeField]
    private Text WrongTileText;
    [SerializeField]
    private Text StartText;
    [SerializeField]
    private Text ZeroTilesText;
    [SerializeField]
    private Button NextTurnButton;
   [SerializeField]
    private Button ReturnAllButton;

    private  GameObject _currentObject;
 
    private void Start()
    {
        _currentObject = StartText.gameObject;
    }
   
    public void InvalidatePlayer( int score)
    {              
         PlayerText.text = score.ToString();        
        _currentObject.SetActive(false);
        _currentObject = StartText.gameObject;
        _currentObject.SetActive(true);
    }

    #region Error showing

    public void ShowNotExistError()
    {
        _currentObject.SetActive(false);
        _currentObject = NotExistText.gameObject;
        _currentObject.SetActive(true);
    }

    public void ShowDeleteError()
    {
        _currentObject.SetActive(false);
        _currentObject = DeleteText.gameObject;
        _currentObject.SetActive(true);
    }

    public void ShowChangeLetterError()
    {
        _currentObject.SetActive(false);
        _currentObject = ChangeLetterText.gameObject;
        _currentObject.SetActive(true);
    }

    public void ShowWrongTileError()
    {
        _currentObject.SetActive(false);
        _currentObject = WrongTileText.gameObject;
        _currentObject.SetActive(true);
    }

    public void ShowZeroTilesError()
    {
        _currentObject.SetActive(false);
        _currentObject = ZeroTilesText.gameObject;
        _currentObject.SetActive(true);
    }

    #endregion Error showing     
}