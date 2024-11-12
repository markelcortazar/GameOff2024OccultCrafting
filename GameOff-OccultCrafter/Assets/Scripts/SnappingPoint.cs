using Shapes;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class SnappingPoint : MonoBehaviour
{

    [SerializeField]
    private float radius = 1f;
    [SerializeField]
    private bool canAdd = true;
    [SerializeField]
    private bool canRemove = true;
    public bool CanRemove { 
        get { return canRemove; } 
        set 
        { 
            canRemove = value; 
            if (canRemove && heldItem != null) heldItem.GetComponent<Collider2D>().enabled = true;
            else if (!canRemove && heldItem != null) heldItem.GetComponent<Collider2D>().enabled = false; 
        }
    }
    private float deviationTolerance;
    private Transform heldItem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deviationTolerance = radius / 100f;
        GrabManager.OnDropElement += CheckForDroppedElementRange;
        GrabManager.OnGrabElement += CheckForGrabbedElementIsHeld;
    }


    void RemoveHeldItem() 
    {
        if (!canRemove) return;
        heldItem.GetComponent<Collider2D>().enabled = true;
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
        if ( Vector2.Distance(args.element.transform.position, this.transform.position) <= radius)
        {
            heldItem = args.element;
            if(!canRemove) args.element.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(SnapElement());
        }
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
}
