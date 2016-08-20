using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Player[] players;
    private Navigatable[] navObjs;

    public HexGrid grid;

    // Use this for initialization
    private void Start()
    {
        if (grid == null)
            grid = FindObjectOfType<HexGrid>();
        if (grid == null)
            Debug.LogError("No grid found");

        navObjs = FindObjectsOfType<Navigatable>();
        if (navObjs == null || navObjs.Length < 1)
            Debug.LogError("No navigatable objects found in scene.");

        players = FindObjectsOfType<Player>();
        if (players == null || players.Length < 1)
            Debug.LogError("No players found in scene.");
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public MoveType GetMoveType(Navigatable mover, HexCoordinates destination, out Vector3? pos)
    {
        pos = grid.ParseMove(destination);
        if (!pos.HasValue)
            return MoveType.Invalid;

        foreach (var item in navObjs)
        {
            if (item.CurrentPosition == pos)
            {
                return MoveType.Attacking;
            }
        }

        return MoveType.Movement;
    }
}