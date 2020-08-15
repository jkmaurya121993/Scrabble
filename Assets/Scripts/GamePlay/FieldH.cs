
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

//[0,0] - lower left angle
//Todo: test
public class FieldH : MonoBehaviour
{
    public enum Direction
    {
        Horizontal, Vertical, None
    }

    #region Prefabs
    [SerializeField]
    private TileH TileHPrefab;      
    #endregion Prefabs

    public UIController Controller;
    public Direction CurrentDirection = Direction.None;
    public int CurrentTurn = 1;
    public bool isFirstTurn = true;  
    public byte NumberOfRows = 12;
    public byte NumberOfColumns = 12;
    public byte CurrentPlayer = 1;
    public LetterBoxH Player;
    public TileH[,] Field;
    public List<TileH> CurrentTiles;

    [SerializeField]
    private UIGrid FieldGrid;
    private List<TileH> _wordsFound;   
    private List<TileH> _asterixTiles = new List<TileH>();

    private void Start()
    {
        CurrentTiles = new List<TileH>();
      
        _wordsFound = new List<TileH>();
               
        FieldGrid.Initialize();

        float letterSize = FieldGrid.Items[0, 0].gameObject.GetComponent<RectTransform>().rect.width;

        Player.LetterSize = new Vector2(letterSize, letterSize);
      
        CreateField();    
    }

  
    private void CreateField()
    {
        Field = new TileH[NumberOfRows, NumberOfColumns];

        Color standardColor = new Color(0, 0.8f, 0);

        for (int i = 0; i < NumberOfRows; i++)
        {
            for (int j = 0; j < NumberOfColumns; j++)
            {
                TileH newTile = Instantiate(TileHPrefab);

                newTile.transform.SetParent(gameObject.transform);

                newTile.Column = j;    
                
                newTile.GetComponent<Image>().color = standardColor;  
                
                newTile.Row = i;

                Field[i, j] = newTile;

                FieldGrid.AddElement(i, j, newTile.gameObject);
            }
        }
        Field[5, 5].CanDrop = true;       

        Field[5, 5].GetComponent<Image>().color = new Color(1,0,0);

    }

    public void OnEndTurn()
    {
        if (CurrentTiles.Count > 0)
        {
            if (CheckWords())
            {            
                CurrentTurn++;

                int points = CountPoints();   
                
                    Player.ChangeBox(Player.NumberOfLetters - Player.CurrentLetters.Count);

                    Player.Score += points;

                    if (Player.CurrentLetters.Count == 0)
                    {
                        EndGame(Player);
                    }     
                    
                    CurrentTiles.Clear();

                    CurrentDirection = Direction.None;   
                
                    Controller.InvalidatePlayer( Player.Score);             
            }
            else 
                Controller.ShowNotExistError();
        }
        else 
            Controller.ShowZeroTilesError();

        _wordsFound = new List<TileH>();
    }  

    public void OnRemoveAll()
    {
        for (int i = CurrentTiles.Count - 1; i >= 0; i--)
        {
            CurrentTiles[i].RemoveTile();
        }
        CurrentTiles.Clear();

        CurrentDirection = Direction.None;
    }

    #region Word checking

    private bool CheckWords()
    {
        List<TileH> words = CreateWords();

        _wordsFound = words;

            string word = GetWord(words[0], words[1]);       
        
            bool successful = CheckWord(word);

            int i = 3;

            SwitchDirection();

            while (successful && i < words.Count)
            {
                word = GetWord(words[i - 1], words[i]);

                successful = CheckWord(word);
                i += 2;
            }
            SwitchDirection();

            return successful;
        
    }

    private List<TileH> CreateWords()
    {     

        List<TileH> res = new List<TileH>();

        if (CurrentDirection == Direction.None)
            CurrentDirection = Direction.Horizontal;

        TileH start, end;

        CreateWord(CurrentTiles[0], out start, out end);

        if (start == end)
        {
            SwitchDirection();
            CreateWord(CurrentTiles[0], out start, out end);
        }

        res.Add(start);

        res.Add(end);

        SwitchDirection();

        foreach (TileH tile in CurrentTiles)
        {
            CreateWord(tile, out start, out end);

            if (start != end)
            {
                res.Add(start);

                res.Add(end);
            }
        }       
        SwitchDirection();

        return res;
    }

    private void CreateWord(TileH start, out TileH wordStart, out TileH wordEnd)
    {
        if (CurrentDirection == Direction.Vertical)
        {
            int j = start.Row;
            while (j < NumberOfRows && Field[j, start.Column].HasLetter)
            {             
                j++;
            }

            wordStart = Field[j - 1, start.Column];

            j = start.Row;

            while (j >= 0 && Field[j, start.Column].HasLetter)
            {               
                j--;
            }
            wordEnd = Field[j + 1, start.Column];
        }
        else
        {
            int j = start.Column;
            while (j >= 0 && Field[start.Row, j].HasLetter)
            {              
                j--;
            }
            wordStart = Field[start.Row, j + 1];
            j = start.Column;
            while (j < NumberOfColumns && Field[start.Row, j].HasLetter)
            {              
                j++;
            }
            wordEnd = Field[start.Row, j - 1];
        }
    }

    private int CountPoints()
    {
        int result = 0;
       
        int[] score = new int[_wordsFound.Count / 2];

        for (int i = 0; i < _wordsFound.Count; i += 2)
        {
            int tempRes = 0;
            if (_wordsFound[i].Row == _wordsFound[i + 1].Row)
                for (int j = _wordsFound[i].Column; j <= _wordsFound[i + 1].Column; j++)
                {
                    TileH tile = Field[_wordsFound[i].Row, j];
                    tempRes += LetterBoxH.PointsDictionary[tile.CurrentLetter.text];                  
                }
            else
            {
                for (int j = _wordsFound[i].Row; j >= _wordsFound[i + 1].Row; j--)
                {
                    TileH tile = Field[j, _wordsFound[i].Column];
                    tempRes += LetterBoxH.PointsDictionary[tile.CurrentLetter.text];                   
                }
            }
            result += tempRes;
            score[i / 2] = tempRes;
        }
        int start = 7 + _wordsFound.Count / 2;
        foreach (int i in score)
        {
            Field[start, 0].SetPoints(i);
            start--;
        }

        return result;
    }



    private void SwitchDirection()
    {
        CurrentDirection = CurrentDirection == Direction.Horizontal ? Direction.Vertical : Direction.Horizontal;
    }

    private string GetWord(TileH begin, TileH end)
    {
        if (CurrentDirection == Direction.Vertical)
        {
            var sb = new StringBuilder();
            for (int j = begin.Row; j >= end.Row; j--)
            {              
                sb.Append(Field[j, begin.Column].CurrentLetter.text);
            }
            return sb.ToString();
        }
        else
        {
            StringBuilder sb = new StringBuilder();

            for (int j = begin.Column; j <= end.Column; j++)
            {              
                sb.Append(Field[begin.Row, j].CurrentLetter.text);
            }
            return sb.ToString();
        }
    }

  

    private bool CheckWord(string word)
    {
        
        NetSpell.SpellChecker.Dictionary.WordDictionary oDict = new NetSpell.SpellChecker.Dictionary.WordDictionary();
        oDict.DictionaryFile =   Application.streamingAssetsPath + @"/en-UK.dic";     

        oDict.Initialize();

        string wordToCheck = word;

        NetSpell.SpellChecker.Spelling oSpell = new NetSpell.SpellChecker.Spelling();

        oSpell.Dictionary = oDict;
        if (!oSpell.TestWord(wordToCheck))
        {
            #if DEBUG_LOG
            Debug.LogError("Not Match");
            #endif

            return false;
        }
        #if DEBUG_LOG
        Debug.LogError(" Match");
        #endif
        return true;
    }

    #endregion Word checking

    private void EndGame(LetterBoxH playerOut)//Player ran out of letters is passed
    {
        #if DEBUG_LOG
        Debug.Log("End Game!!");
        #endif
       
    }
}