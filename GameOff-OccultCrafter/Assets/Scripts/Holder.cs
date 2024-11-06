using NUnit.Framework;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Holder : MonoBehaviour
{
    private Collider2D collider;
    [SerializeField]
    private List<HoldingPoint> holdingPoints;

    [Serializable]
    private class HoldingPoint 
    {
        public Vector2 offset;
        public bool isOccupied;
    }

    [SerializeField,EnumToggleButtons]
    private TypeToHold typeToHold;

    public List<Transform> heldItems;

    public enum TypeToHold
    {
        Material,
        Tool
    }

    void Start()
    {
        collider = GetComponent<Collider2D>();
        heldItems = new List<Transform>();
    }
    void Update()
    {
        foreach (var item in heldItems)
        {
            item.transform.Rotate(new Vector3(0, 0, 5) * Time.deltaTime);
        }
    }

    public void AddItem(Transform item)
    {
        if (heldItems.Count == holdingPoints.Count) return;
        item.gameObject.GetComponent<Collider2D>().enabled = false;
        HoldingPoint hP = holdingPoints[FindClosestHoldingPoint(item.position)];
        hP.isOccupied = true;
        StartCoroutine(Slide(item, (Vector3)hP.offset + this.transform.position)) ;
        heldItems.Add(item);
    }

    public void RemoveItem(Transform item)
    {
        if (heldItems.Contains(item))
        {
            heldItems.Remove(item);
            item.rotation = Quaternion.identity;
            item.GetComponent<Collider2D>().enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Here filter
        AddItem(collision.gameObject.transform);
    }

    private int FindClosestHoldingPoint(Vector3 pos)
    {
        int current = 0;
        float distance = Vector3.Distance(this.transform.position + (Vector3)holdingPoints[0].offset, pos);
        for (int i = 0; i < holdingPoints.Count; i++)
        {
            // Skip occupied holding spots
            if (holdingPoints[i].isOccupied) continue;

            float candidateDistance = Vector3.Distance(this.transform.position + (Vector3)holdingPoints[i].offset, pos);
            if (candidateDistance < distance)
            {
                current = i;
                distance = candidateDistance;
            }
        }
        return current;
    }

    IEnumerator Slide(Transform pos, Vector3 target)
    {
        while (pos.position != target) 
        {
            Debug.DrawLine(target, pos.position, Color.white, 0.05f);
            pos.position = Vector3.MoveTowards(pos.position, target, 75 * Time.deltaTime);
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }

    private void OnDrawGizmosSelected()
    {
        if (holdingPoints == null || holdingPoints.Count == 0) return;
        foreach(var point in holdingPoints)
        {
            Gizmos.DrawWireSphere(this.transform.position + (Vector3)point.offset, 0.3f);
        }

    }

}
