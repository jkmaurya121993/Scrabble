using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//Todo: Add leters only in the end of the turn
public class LetterBoxH : MonoBehaviour
{
    #region Letters and scores

    public static Dictionary<string, int> PointsDictionary =
        new Dictionary<string, int>
        {
            {"A", 1},
            {"B", 2},
            {"C", 1},
            {"D", 1},
            {"E", 1},
            {"F", 2},
            {"G", 6},
            {"H", 6},
            {"I", 1},
            {"J", 1},
            {"K", 1},
            {"L", 8},
            {"M", 2},
            {"N", 6},
            {"O", 2},
            {"P", 1},
            {"Q", 3},
            {"R", 2},
            {"S", 4},
            {"T", 1},
            {"U", 5},
            {"V", 2},
            {"W", 7},
            {"X", 4},
            {"Y", 4},
            {"Z", 8}
           
        };

    private static List<string> _allLetters;

    #endregion Letters and scores

    public List<Vector3> FreeCoordinates;
    public List<LetterH> CurrentLetters;
    public int Score = 0;  
    public LetterH LetterHPrefab;
    public bool CanChangeLetters = true;
    public byte NumberOfLetters = 12;
    public float DistanceBetweenLetters = 1.2f;
    public Vector2 LetterSize;

    private Vector3 _pos;
    private float _xOffset = 0;
    private FieldH _currentFieldH;
    
   // public Text NumberOfLettersText;//for testing only

    private void Start()
    {
        
        if (_allLetters == null)
            _allLetters = new List<string>
    {
        "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U",
        "V","W","X","Y","Z"
    };
        CurrentLetters = new List<LetterH>();

        _allLetters = _allLetters.OrderBy(letter => letter).ToList();

        FreeCoordinates = new List<Vector3>();

        _currentFieldH = GameObject.FindGameObjectWithTag("Field").GetComponent<FieldH>();

        DistanceBetweenLetters = LetterSize.x;

        LetterHPrefab.gameObject.GetComponent<RectTransform>().sizeDelta = LetterSize;

        _xOffset = gameObject.transform.position.x - 2 * DistanceBetweenLetters;

        var yOffset = gameObject.transform.position.y + DistanceBetweenLetters;

        _pos = new Vector3(_xOffset, yOffset);

        ChangeBox(NumberOfLetters);
    }

    //Activates/deactivates ChangeLetters button
    private void Update()
    {
        if (_allLetters == null || _allLetters.Count == 0)
            CanChangeLetters = false;
        
    }

    //Adds letters to the hand of player
    public void ChangeBox(int numberOfLetters, string letter = null)
    {
        if (String.IsNullOrEmpty(letter) && numberOfLetters > _allLetters.Count)
        {
            numberOfLetters = _allLetters.Count;
        }
        if (FreeCoordinates.Count == 0)//If there is no free space create new letter in unused space
        {
            for (var i = 0; i < numberOfLetters; i++)
            {
                AddLetter(_pos, letter);

                _pos.x += DistanceBetweenLetters;

                if (i % 4 == 3)
                {
                    _pos.x = _xOffset;

                    _pos.y -= DistanceBetweenLetters;
                }
            }
        }
        else
        {
            for (var j = 0; j < numberOfLetters; j++)
            {
                AddLetter(FreeCoordinates[FreeCoordinates.Count - 1], letter);

                FreeCoordinates.RemoveAt(FreeCoordinates.Count - 1);
            }
        }
       
    }

    //Crates new LetterH on field
    private void AddLetter(Vector3 position, string letter)
    {
        var newLetter = Instantiate(LetterHPrefab, position,
            transform.rotation) as LetterH;

        newLetter.transform.SetParent(gameObject.transform);

        if (String.IsNullOrEmpty(letter))//if letter is returned from Field
        {
            var current = _allLetters[UnityEngine.Random.Range(0, _allLetters.Count)];

            newLetter.ChangeLetter(current);

            _allLetters.Remove(current);

            CurrentLetters.Add(newLetter);
        }
        else//if new letter is created
        {
            newLetter.ChangeLetter(letter);

            CurrentLetters.Add(newLetter);
        }
    }

    //Removes letter from hand when it is dropped on grid
    public void RemoveLetter()
    {
        LetterH currentObject = DragHandler.ObjectDragged.GetComponent<LetterH>();

        int currentIndex = FindIndex(currentObject);

        Vector3 previousCoordinates = DragHandler.StartPosition;

        for (int j = currentIndex + 1; j < CurrentLetters.Count; j++)//shifts all letters
        {
            Vector3 tempCoordinates = CurrentLetters[j].gameObject.transform.position;

            CurrentLetters[j].gameObject.transform.position = previousCoordinates;

            previousCoordinates = tempCoordinates;
        }
        FreeCoordinates.Add(previousCoordinates);

        CurrentLetters.Remove(currentObject);
    }

    //Changes letters for Letters player checked
    public bool ChangeLetters()
    {
        var successful = false;
        foreach (LetterH t in CurrentLetters)
        {
            if (t.isChecked)
            {
                string text = t.LetterText.text;

                t.LetterText.text = _allLetters[UnityEngine.Random.Range(0, _allLetters.Count)];

                _allLetters.Add(text);

                _allLetters.Remove(t.LetterText.text);

                t.isChecked = false;

                successful = true;
            }
        }
        return successful;
    }

    //Finds the index of LetterH in CurrentLetters
    public int FindIndex(LetterH input)
    {
        int j = 0;
        for (; j < CurrentLetters.Count; j++)
        {
            if (CurrentLetters[j] == input)
                return j;
        }
        return -1;
    }

}