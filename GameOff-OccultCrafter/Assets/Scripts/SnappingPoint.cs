using UnityEditor;
using UnityEngine;

public class SnappingPoint : MonoBehaviour
{

    [SerializeField]
    private float radius = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, Vector3.back, radius);
    }
}
