using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //Score of player
    [SerializeField]
    private Text PlayerText;
    //public Material PlayerGlowMaterial;
   // public Material PlayerIdleMaterial;

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

    //Endgame fields
   // [SerializeField]
   // private Canvas EndGameCanvas;
   // [SerializeField]
   // private Text Player1Points;
   // [SerializeField]
   // private Text Player1Name;
   //[SerializeField]
   // private Text Winner;

    private static GameObject _currentObject;
 

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

    #region Button activation  
    public void SetNextButtonActive(bool active)
    {
      
        {
            NextTurnButton.interactable = active;
            ReturnAllButton.interactable = active;
        }
    }

    #endregion Button activation

    //Creates endgame field with player names and scores
    //public void SetWinner(int winner, int player1Score, int player2Score, string player1Name, string player2Name)
    //{
    //    GameObject.FindGameObjectWithTag("Pause").GetComponent<PauseBehaviour>().GameOver = true;
    //    EndGameCanvas.gameObject.SetActive(true);
    //    if (winner == 1)
    //        Winner.text = player1Name;
    //    else Winner.text = player2Name;
    //    Player1Name.text = player1Name;
    //    Player1Points.text = player1Score.ToString();       
    //    gameObject.GetComponent<Canvas>().enabled = false;
    //}
}