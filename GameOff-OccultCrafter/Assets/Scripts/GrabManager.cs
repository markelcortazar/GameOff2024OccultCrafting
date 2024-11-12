using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class GrabManager : MonoBehaviour
{
    public static event EventHandler<OnElementGrabOrDropEventArgs> OnDropElement;
    public static event EventHandler<OnElementGrabOrDropEventArgs> OnGrabElement;
    public class OnElementGrabOrDropEventArgs : EventArgs
    {
        public Transform element;
    }

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
        OnDropElement?.Invoke(this, new OnElementGrabOrDropEventArgs {element = grabbedElement});
        grabbedElement = null;
    }

    private void TryFollowMouseWhilePressed()
    {
        if (grabbedElement == null) return;
        Vector3 newPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        newPos.z = grabbedElement.position.z;
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
                OnGrabElement?.Invoke(this, new OnElementGrabOrDropEventArgs { element = grabbedElement});
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        TryFollowMouseWhilePressed();
    }
}
