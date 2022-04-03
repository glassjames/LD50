using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(BoxCollider2D))]
public class CollidableObject : MonoBehaviour
{
    public ContactFilter2D filter;
    private Collider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];

    protected virtual void Start()
    {
        hitbox = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        hitbox.OverlapCollider(filter, hits);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
            {
                continue;
            }

            OnCollide(hits[i]);

            hits[i] = null;
        }
    }

    protected virtual void OnCollide(Collider2D hit)
    {
        Debug.Log(hit.name);
    }
}
