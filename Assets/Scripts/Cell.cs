using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Cell : MonoBehaviour
{
    public Color occupied;
    public Color free;

    private int _value;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public int value
    {
        get
        {
            return _value;
        }

        set
        {
            _value = value;
            _renderer.color = _value == 0 ? free : occupied;
        }
    }
}
