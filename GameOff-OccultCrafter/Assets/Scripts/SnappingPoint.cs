using Shapes;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class SnappingPoint : MonoBehaviour
{
    private static float minDistanceFound = float.MaxValue;
    private static SnappingPoint currentClosest = null;

    [SerializeField]
    private float radius = 1f;
    [SerializeField]
    private bool canAdd = true;
    [SerializeField, OnValueChanged("ValidateCanRemove")]
    private bool canRemove = true;
    public bool CanRemove { 
        get { return canRemove; } 
        set
        {
            canRemove = value;
            ValidateCanRemove();
        }
    }
    [SerializeField]
    private bool canBeDisplaced = false;
    private float deviationTolerance;
    private Transform heldItem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deviationTolerance = radius / 100f;
        GrabManager.OnDropElement += CheckForDroppedElementRange;
        GrabManager.OnGrabElement += CheckForGrabbedElementIsHeld;
    }

    void AddHeldItem(Transform item)
    {
        heldItem = item;
        if (!canRemove) item.GetComponent<Collider2D>().enabled = false;
        if (!canBeDisplaced) item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        StartCoroutine(SnapElement());
    }

    void RemoveHeldItem() 
    {
        if (!canRemove) return;
        heldItem.GetComponent<Collider2D>().enabled = true;
        heldItem.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        heldItem = null;
        TickManager.OnTick -= CheckForHeldElementRemains;
    }

    void CheckForGrabbedElementIsHeld(object sender, GrabManager.OnElementGrabOrDropEventArgs args) 
    {
        if (heldItem == args.element)
            RemoveHeldItem();
    }

    void CheckForHeldElementRemains(object sender, TickManager.OnTickEventArgs args)
    {
        if ((Vector2.Distance(heldItem.position, this.transform.position) > deviationTolerance))
            RemoveHeldItem();
    }

    void CheckForDroppedElementRange(object sender, GrabManager.OnElementGrabOrDropEventArgs args)
    {
        if (!canAdd || heldItem != null) return;
        float distance = Vector2.Distance(args.element.transform.position, this.transform.position);
        if ( distance <= radius)
        {
            StartCoroutine(CheckForClosestSnappingPoint(distance, args.element));
        }
    }

    private IEnumerator CheckForClosestSnappingPoint(float dist, Transform element)
    {
        if (dist < minDistanceFound)
        {
            minDistanceFound = dist;
            currentClosest = this;
        }
        yield return null;
        if (this != currentClosest) yield break;
        AddHeldItem(element);
        minDistanceFound = float.MaxValue;
        currentClosest = null;
    }

    private IEnumerator SnapElement()
    {
        while ((Vector2.Distance(heldItem.position, this.transform.position) > deviationTolerance))
        {
            Vector3 pos = Vector3.MoveTowards(heldItem.position, this.transform.position, 25 * Time.deltaTime);
            pos.z = heldItem.position.z;
            heldItem.position = pos;
            yield return null;
        }
        TickManager.OnTick += CheckForHeldElementRemains;
    }

    private void OnDrawGizmos()
    {
        Draw.UseDashes = true;
        Draw.DashSpace = DashSpace.Meters;
        Draw.DashSize = 0.2f;
        Draw.DashSpacing = 0.1f;
        Draw.DashType = DashType.Basic;
        Draw.Color = Color.white;
        Draw.Ring(transform.position, radius, 0.02f);
        Draw.Disc(transform.position, radius/50);
    }

    private void ValidateCanRemove()
    {
        if (canRemove && heldItem != null) heldItem.GetComponent<Collider2D>().enabled = true;
        else if (!canRemove && heldItem != null) heldItem.GetComponent<Collider2D>().enabled = false;
    }
}
