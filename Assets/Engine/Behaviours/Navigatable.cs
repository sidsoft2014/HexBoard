using Photon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Navigatable : PunBehaviour
{
    private HexGrid _grid;
    private GameManager _gm;

    private Vector3 _currentPosition;
    private Vector3 _nextPosition;
    private Renderer _renderer;
    private Material[] _startingMats;
    private bool _isMoving;
    private bool _canMove;
    private float _step;

    public Navigatable() { }

    public float speed = 20;

    public Vector3 CurrentPosition
    {
        get
        {
            return _currentPosition;
        }
        set
        {
            if (_currentPosition != default(Vector3))
                PreviousPosition = _currentPosition;
            _currentPosition = value;
        }
    }
    public Vector3 PreviousPosition { get; private set; }
    public HexCoordinates CurrentCoordinates { get; private set; }
    public bool IsSelected { get; private set; }

    #region Unity Overrides
    // Awake is called when the script instance is being loaded
    public virtual void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _startingMats = _renderer.materials;
        _step = speed * Time.deltaTime;
    }

    // Start is called just before any of the Update methods is called the first time
    public virtual void Start()
    {
        _grid = FindObjectOfType<HexGrid>();
        if (_grid == null)
            Debug.LogError("No hex grid object found in scene.");

        _gm = FindObjectOfType<GameManager>();
        if (_gm == null)
            Debug.LogError("No game manager found in scene.");

        CurrentCoordinates = new HexCoordinates(0, 0);
        _currentPosition = _grid.GetCell(CurrentCoordinates).transform.position;
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    public virtual void Update()
    {
        if (_isMoving)
        {
            if (transform.position == _nextPosition)
            {
                _isMoving = false;
                CurrentPosition = _nextPosition;
                _nextPosition = default(Vector3);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _nextPosition, _step);
            }
        }
        else
        {
            if (_canMove)
            {
                HandleInput();
            }
        }
    }

    // OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider
    public virtual void OnMouseDown()
    {
        SetSelected(!IsSelected);
    }

    // OnMouseEnter is called when the mouse entered the GUIElement or Collider
    public virtual void OnMouseEnter()
    {
        if (IsSelected)
            return;

        _renderer.material = new Material(_startingMats[0]);
        _renderer.material.color = Color.yellow;
    }


    // OnMouseExit is called when the mouse is not any longer over the GUIElement or Collider
    public virtual void OnMouseExit()
    {
        if (IsSelected)
            return;

        _renderer.materials = _startingMats;
    }

    #endregion

    public void SetSelected(bool isSelected)
    {
        IsSelected = _canMove = isSelected;
        if (IsSelected)
        {
            _renderer.material = new Material(_startingMats[0]);
            _renderer.material.color = Color.red;
        }
        else
        {
            _renderer.materials = _startingMats;
        }
    }

    public void HandleInput()
    {
        HexCoordinates? hex = null;

        // Left
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            hex = new HexCoordinates(CurrentCoordinates.X - 1, CurrentCoordinates.Z);
        }
        // Up Left
        else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Keypad7))
        {
            hex = new HexCoordinates(CurrentCoordinates.X - 1, CurrentCoordinates.Z + 1);
        }
        // Up Right
        else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Keypad9))
        {
            hex = new HexCoordinates(CurrentCoordinates.X, CurrentCoordinates.Z + 1);
        }
        // Right
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            hex = new HexCoordinates(CurrentCoordinates.X + 1, CurrentCoordinates.Z);
        }
        // Down Right
        else if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            hex = new HexCoordinates(CurrentCoordinates.X + 1, CurrentCoordinates.Z - 1);
        }
        // Down Left
        else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            hex = new HexCoordinates(CurrentCoordinates.X, CurrentCoordinates.Z - 1);
        }

        if (!hex.HasValue)
            return;

        if (((HexCoordinates)hex).XZ == CurrentCoordinates.XZ)
            return;

        //var vec = _grid.ParseMove((HexCoordinates)hex);
        Vector3? pos;
        var mType = _gm.GetMoveType(this, (HexCoordinates)hex, out pos);
        if(mType == MoveType.Invalid)
        {
            return;
        }

        CurrentCoordinates = (HexCoordinates)hex;
        _nextPosition = (Vector3)pos;
        _isMoving = true;
    }
}

