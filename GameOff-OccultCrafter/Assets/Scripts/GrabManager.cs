using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class GrabManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Transform grabbedElement;
    [SerializeField]
    private Camera cam;

    public void Grab(InputAction.CallbackContext context)
    {
        if (context.started)
            TryFindAndGrabObject();
        else if (context.canceled)
            TryReleaseObject();
    }

    private void TryReleaseObject()
    {
        if (grabbedElement == null) return;
        grabbedElement.GetComponent<Collider2D>().enabled = true;
        grabbedElement = null;
    }

    private void TryFollowMouseWhilePressed()
    {
        if (grabbedElement == null) return;
        Vector3 newPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        newPos.z = grabbedElement.position.z;
        Debug.Log(newPos);
        grabbedElement.position = newPos;
    }

    private void TryFindAndGrabObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Grabbable"))
            {
                grabbedElement = hit.collider.gameObject.transform;
                grabbedElement.gameObject.GetComponent<Collider2D>().enabled = false;
            }

        }
    }

    //    Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    //    mousePos.z = 0;
    //    if (boundsCollider.bounds.Contains(mousePos))
    //    {
    //        isGrabbed = true;
    //        boundsCollider.enabled = false;
    //    }
    //}
    //else if (context.canceled && isGrabbed)
    //{
    //    boundsCollider.enabled = true;
    //    isGrabbed = false;

    //}


    // Update is called once per frame
    void Update()
    {
        TryFollowMouseWhilePressed();
    }
}
