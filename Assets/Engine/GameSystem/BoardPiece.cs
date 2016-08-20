using UnityEngine;
using System.Collections;
using System;

[Serializable]
[RequireComponent(typeof(Navigatable))]
public class BoardPiece : MonoBehaviour
{
    public Player Owner;
    private Character _baseCharacter;
    private Navigatable _nav;
    private Renderer _renderer;
    private Material[] _startingMats;
    public bool IsSelected { get; private set; }

    public virtual void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _startingMats = _renderer.materials;
        _nav = GetComponent<Navigatable>();
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
    /// Call at end of turn to trigger any character actions.
    /// </summary>
    public virtual void OnTurnEnd()
    {
        if (_baseCharacter == null)
            return;
    }

    /// <summary>
    /// Call at start of turn to reset moves and trigger any character actions.
    /// </summary>
    public virtual void OnTurnStart()
    {
        if (_nav != null)
            _nav.ResetMoves();
    }

    public virtual void SetCharacter(Character character)
    {
        _baseCharacter = character;
        if(_nav != null)
        {
            if (_baseCharacter != null)
            {
                _nav.Moves = _baseCharacter.Moves;
            }
            else
            {
                _nav.Moves = 0;
            }
        }
    }

    public virtual void SetSelected(bool selected)
    {
        if (selected == IsSelected)
            return;

        IsSelected = selected;

        if (_nav != null)
            _nav.CanMove = IsSelected;
        if (IsSelected)
        {
            if (Owner != null)
                Owner.SetSelectedPiece(this);

            _renderer.material = new Material(_startingMats[0]);
            _renderer.material.color = Color.red;
        }
        else
        {
            _renderer.materials = _startingMats;
        }
    }

    public virtual void Start()
    {

    }
}
