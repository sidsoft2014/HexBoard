using Photon;
using UnityEngine;

public class Navigatable : PunBehaviour
{
    public float speed = 20;

    private Vector3 _currentPosition;
    private Vector3 _nextPosition;
    private GameManager _gm;
    private HexGrid _grid;
    private bool _isMoving;
    private float _step;
    private int _spacesMoved;
    private int _totalSpacesMoved;

    public int Moves { get; set; }

    public bool CanMove { get; set; }

    public HexCoordinates Coordinates { get; private set; }

    public Vector3 Position
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

    #region Unity Overrides

    // Awake is called when the script instance is being loaded
    public virtual void Awake()
    {
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

        Coordinates = new HexCoordinates(0, 0);
        _currentPosition = _grid.GetCell(Coordinates).transform.position;
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    public virtual void Update()
    {
        if (_isMoving)
        {
            if (transform.position == _nextPosition)
            {
                _isMoving = false;
                Position = _nextPosition;
                _nextPosition = default(Vector3);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _nextPosition, _step);
            }
        }
        else
        {
            if (CanMove)
            {
                HandleInput();
            }
        }
    }

    #endregion Unity Overrides

    public void HandleInput()
    {
        if (Moves - _spacesMoved <= 0)
            return;

        HexCoordinates? hex = null;

        // Left
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            hex = new HexCoordinates(Coordinates.X - 1, Coordinates.Z);
        }
        // Up Left
        else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Keypad7))
        {
            hex = new HexCoordinates(Coordinates.X - 1, Coordinates.Z + 1);
        }
        // Up Right
        else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Keypad9))
        {
            hex = new HexCoordinates(Coordinates.X, Coordinates.Z + 1);
        }
        // Right
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            hex = new HexCoordinates(Coordinates.X + 1, Coordinates.Z);
        }
        // Down Right
        else if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            hex = new HexCoordinates(Coordinates.X + 1, Coordinates.Z - 1);
        }
        // Down Left
        else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            hex = new HexCoordinates(Coordinates.X, Coordinates.Z - 1);
        }

        if (!hex.HasValue)
            return;

        if (((HexCoordinates)hex).XZ == Coordinates.XZ)
            return;

        Vector3? pos;
        var mType = _gm.GetMoveType((HexCoordinates)hex, out pos);
        if (mType == MoveType.Invalid)
        {
            return;
        }

        _spacesMoved++;
        _totalSpacesMoved++;

        Coordinates = (HexCoordinates)hex;

        _nextPosition = (Vector3)pos;
        _isMoving = true;
    }

    public void ResetMoves()
    {
        _spacesMoved = 0;
    }
}