using UnityEngine;

public class HexCell : MonoBehaviour
{
    public Vector3 FullPosition
    {
        get
        {
            if (ReferenceEquals(null, coordinates) || coordinates.Equals(default(HexCoordinates)))
            {
                return Vector3.up;
            }
            else
            {
                return new Vector3(coordinates.X, coordinates.Y, coordinates.Z);
            }
        }
    }

    public HexCoordinates coordinates;
    public Color color;

    public override string ToString()
    {
        return string.Format("HexCell [{0},{1},{2}]", FullPosition.x, FullPosition.y, FullPosition.z);
    }

    public bool IsPosition(Vector3 pos)
    {
        var comp = HexCoordinates.FromPosition(pos);
        return coordinates.Equals(comp);
    }
}