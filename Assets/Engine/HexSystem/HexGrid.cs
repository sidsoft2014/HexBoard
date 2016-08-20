using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    Canvas gridCanvas;
    HexMesh hexMesh;
    HexCell[] cells;

    public int width = 6;
    public int height = 6;

    public HexCell cellPrefab;
    public Text cellLabelPrefab;
    public Color defaultColor = Color.cyan;

    void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        cells = new HexCell[height * width];

        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    void Start()
    {
        hexMesh.Triangulate(cells);
    }
    
    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;

        // Set Cell Label
        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }

    public void ColorCell(Vector3 position, Color color)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = cells[index];
        cell.color = color;
        hexMesh.Triangulate(cells);
    }

    public IEnumerable<HexCell> GetAllCells()
    {
        return cells;
    }

    public HexCell GetCell(Vector3 pos)
    {
        foreach (var item in cells)
        {
            if (item.IsPosition(pos))
            {
                return item;
            }
        }
        return null;
    }

    public HexCell GetCell(HexCoordinates coords)
    {
        foreach (var item in cells)
        {
            if (coords.Equals(item.coordinates))
            {
                return item;
            }
        }
        return null;
    }

    public HexCell GetCell(int x, int z)
    {
        foreach (var cell in cells)
        {
            if (cell.coordinates.X == x && cell.coordinates.Z == z)
                return cell;
        }
        return null;
    }

    public Vector3? ParseMove(Vector3 pos)
    {
        var cell = GetCell(pos);

        if (cell != null)
            return cell.transform.position;
        return null;
    }

    public Vector3? ParseMove(HexCoordinates coords)
    {
        var cell = GetCell(coords);

        if (cell != null)
            return cell.transform.position;
        return null;
    }

    public Vector3? ParseMove(int x, int y)
    {
        var cell = GetCell(x, y);

        if (cell != null)
            return cell.transform.position;
        return null;
    }
}