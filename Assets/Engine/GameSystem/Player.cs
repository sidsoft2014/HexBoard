using Photon;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : PunBehaviour
{
    public List<Character> PlayerPieces;
    public List<BoardPiece> ActivePieces;
    public bool isPlayerTurn;
    public int playerNumber;
    private GameManager _gm;

    public BoardPiece SelectedPiece { get; private set; }

    public void Awake()
    {
        PlayerPieces = new List<Character>();
        for (int idx = 0; idx < 5; idx++)
        {
            PlayerPieces.Add(new Character());
        }
    }

    public void FixedUpdate()
    {

    }

    public void OnGUI()
    {

    }

    public void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        if (_gm == null)
            Debug.LogError("No game manager found in scene");
        else
        {
            _gm.TurnChanged += On_TurnChanged;
        }

        int idx = 0;
        foreach (var character in PlayerPieces)
        {
            try
            {
                string path = "characters/" + character.prefabName;
                var prefab = Resources.Load(path, typeof(GameObject));
                if (prefab == null)
                {
                    Debug.LogError("Resource not found: " + path);
                    continue;
                }

                var obj = Instantiate(prefab) as GameObject;
                if (obj == null)
                {
                    Debug.LogError("Could not load prefab: " + character.prefabName);
                    continue;
                }

                obj.transform.position = new Vector3(idx, 2.5f, 0);
                idx += 15;

                var piece = obj.GetComponent<BoardPiece>();
                if (piece == null)
                {
                    Debug.LogError("No board piece component found.");
                    continue;
                }

                piece.Owner = this;
                piece.SetCharacter(character);
                ActivePieces.Add(piece);
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
                continue;
            }
        }
    }

    internal void SetSelectedPiece(BoardPiece boardPiece)
    {
        if (boardPiece.Owner != this)
            return;

        foreach (var piece in ActivePieces)
        {
            bool selected = piece == boardPiece;
            piece.SetSelected(selected);
        }

        SelectedPiece = boardPiece;
    }

    public void Update()
    {
        if (!isPlayerTurn)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.X))
            DeclareEndTurn();
    }

    public void DeclareEndTurn()
    {
        _gm.EndTurn(this);
    }

    private void On_TurnChanged(object sender, TurnChangeEvent e)
    {
        isPlayerTurn = e.StartingPlayerIdx == playerNumber;
        if (isPlayerTurn)
        {
            foreach (var piece in ActivePieces)
            {
                piece.OnTurnStart();
            }
        }
        else if (e.EndingPlayerIdx == playerNumber)
        {
            foreach (var piece in ActivePieces)
            {
                piece.OnTurnEnd();
            }
        }
    }
}