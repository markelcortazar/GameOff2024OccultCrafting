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
        {
            //Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            //Ray ray = new Ray(cam.transform.position, Vector3.Reflect(cam.ScreenToWorldPoint(Input.mousePosition).normalized, Vector3.forward));
            //Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            //RaycastHit hit;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero);
            if ( hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Grabbable"))
                    Debug.Log("Cowagunga it is " + hit.collider.gameObject.name);

            }
            else Debug.Log("Waaaaaaah . . .");
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
        
    }
}
