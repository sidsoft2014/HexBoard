using UnityEngine;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public HexGrid grid;


    // Use this for initialization
    void Start()
    {
        if (grid == null)
            grid = FindObjectOfType<HexGrid>();
        if (grid == null)
            throw new System.Exception("No grid found");
    }

    // Update is called once per frame
    void Update()
    {

    }

}
