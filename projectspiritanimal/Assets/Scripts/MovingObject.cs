using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    protected Transform t;
    protected Animation ani;
    public float speed;
    public float damage;

    protected Vector2 Position
    {
        get { return t.position; }
        set { t.position = value; }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        t = transform;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
