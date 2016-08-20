using Photon;
using System.Collections.Generic;
using UnityEngine;

public class Player : PunBehaviour
{
    public List<Character> PlayerPieces;

    private GameManager _gm;

    public void Awake()
    {
        PlayerPieces = new List<Character>();
        for (int idx = 0; idx < 5; idx++)
        {
            PlayerPieces.Add(new Character(this));
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

        int idx = 0;
        foreach (var character in PlayerPieces)
        {
            try
            {
                string path = "characters/" + character.prefabName;
                var prefab = Resources.Load(path, typeof(GameObject));
                if(prefab == null)
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

                var nav = obj.GetComponent<Navigatable>();
                if (nav == null)
                {
                    Debug.LogError("No navigatable component found.");
                    continue;
                }

                nav.character = character;

                obj.transform.position = new Vector3(idx, 2.5f, 0);
                idx += 15;
            }
            catch(System.Exception ex)
            {
                Debug.LogError(ex.Message);
                continue;
            }
        }
    }

    public void Update()
    {

    }

    public void EndTurn()
    {

    }
}