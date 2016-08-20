using UnityEngine;
using System.Collections;
using System;

[Serializable]
[RequireComponent(typeof(Navigatable))]
public class BoardPiece : MonoBehaviour
{
    private Character _baseCharacter;
    private Navigatable _nav;
    private Renderer _renderer;
    private Material[] _startingMats;

    public Player Owner;

    public bool IsSelected { get; private set; }

    public virtual void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _startingMats = _renderer.materials;
        _nav = GetComponent<Navigatable>();
    }

    public virtual void Start()
    {
        if (_baseCharacter != null)
            _nav.Moves = _baseCharacter.Moves;
        else _nav.Moves = 1;
    }

    public virtual void OnMouseDown()
    {
        SetSelected(!IsSelected);
    }

    public virtual void OnMouseEnter()
    {
        if (IsSelected)
            return;

        _renderer.material = new Material(_startingMats[0]);
        _renderer.material.color = Color.yellow;
    }

    public virtual void OnMouseExit()
    {
        if (IsSelected)
            return;

        _renderer.materials = _startingMats;
    }


    /// <summary>
    /// Call at start of turn to reset moves and trigger any character actions.
    /// </summary>
    public virtual void OnTurnStart()
    {
        if (_nav != null)
            _nav.ResetMoves();
    }

    /// <summary>
    /// Call at end of turn to trigger any character actions.
    /// </summary>
    public void OnTurnEnd()
    {
        if (_baseCharacter == null)
            return;
    }

    public void SetCharacter(Character character)
    {
        this._baseCharacter = character;
    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected == IsSelected)
            return;

        IsSelected = isSelected;
        if (_nav != null)
            _nav.CanMove = IsSelected;
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
}
