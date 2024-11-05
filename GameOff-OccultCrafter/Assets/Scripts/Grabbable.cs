using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(Collider2D)),RequireComponent (typeof(PlayerInput))]
public class Grabable : MonoBehaviour
{

    private bool isGrabbed = false;
    private Collider2D boundsCollider;
    private static Camera cam;
    void Start()
    {
        if (cam == null) cam = Camera.main;
        boundsCollider = GetComponent<Collider2D>();
    }

    public void Grab(InputAction.CallbackContext context)
    {
        Debug.Log("Getting grabbed :)");
        if (context.started)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            if (boundsCollider.bounds.Contains(mousePos)) 
            { 
                isGrabbed = true;
                boundsCollider.enabled = false;
            } 
        }
        else if (context.canceled && isGrabbed) {
            boundsCollider.enabled = true;  
            isGrabbed = false;
        
        } 
            
    }

    public void Release()
    {
        isGrabbed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbed)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            this.transform.position = mousePos;
        }
        // TODO: Consider ungrabbed items snapping to integer world positions ?
        //else
        //{
        //    if (transform.position.x != Mathf.Floor(transform.position.x)) transform.position
        //}
    }
}
