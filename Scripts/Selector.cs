using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


public class Selector : MonoBehaviour
{
    [Header("For Raycast")]
    public Camera cam;
    public LayerMask mask;

    [Header("Gameobject for selection box")]
    public GameObject selectorBox;

    [Header("Toggle Gizmos")]
    public bool showGizmos;


    private Vector3 movetoPos = Vector3.zero;

    //[Header("Selection Box start/end points")]
    private Vector3 startPoint;
    private Vector3 endPoint;

    //[Header("Selection Box Info")]
    private RectangleF selectionRect;
    private Vector3 rectCenter;
    private Vector3 rectSize;
    private Vector3 halfExtents;

    //[Header("All of the currently Selected Units")]
    private List<GameObject> selectedUnits = new List<GameObject>();



    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = DoRay();
            selectorBox.SetActive(true);
        }

        if (Input.GetMouseButton(0))
        {
            endPoint = DoRay();
            HandleRectangle();
            selectorBox.transform.position = rectCenter;
            selectorBox.transform.localScale = rectSize + new Vector3(0f, 1f, 0f);
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectorBox.SetActive(false);
            endPoint = DoRay();
            HandleRectangle();
            SelectAllUnits();
        }

        if (Input.GetMouseButtonDown(1))
        {
            MoveAllUnits(DoRay());
        }
    }

    private void HandleRectangle()
    {
        //found at : https://forum.unity.com/threads/creating-a-rect-from-two-vectors.375447/
        //just changed the names to fit my own needs


        rectSize = startPoint - endPoint;
        rectSize.x = Mathf.Abs(rectSize.x);
        rectSize.y = Mathf.Abs(rectSize.y);
        rectSize.z = Mathf.Abs(rectSize.z);

        rectCenter = (startPoint + endPoint) * 0.5f;

        halfExtents = rectSize * 0.5f;
    }

    private void ClearAllUnits()
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            Unit unit = selectedUnits[i].GetComponent<Unit>();
            unit.DeselectUnit();
        }

        selectedUnits.Clear();
    }

    private void SelectAllUnits()
    {
        ClearAllUnits();

        RaycastHit[] check = Physics.BoxCastAll(rectCenter, halfExtents, Vector3.up);

        for (int i = 0; i < check.Length; i++)
        {
            if (check[i].collider.CompareTag("Unit"))
            {
                Unit unit = check[i].collider.GetComponent<Unit>();
                unit.SelectUnit();
                unit.CalculateOffset(rectCenter);
                selectedUnits.Add(unit.gameObject);
            }
        }
    }

    private void MoveAllUnits(Vector3 _pos)
    {
        movetoPos = _pos;

        for (int i = 0; i < selectedUnits.Count; i++)
        {
            Unit unit = selectedUnits[i].GetComponent<Unit>();
            unit.MoveToSpot(_pos);
        }
    }

    private Vector3 DoRay()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 1000f, mask))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                return hit.point;
            }
        }

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = UnityEngine.Color.green;
            Gizmos.DrawWireCube(rectCenter, rectSize);

            Gizmos.color = UnityEngine.Color.yellow;
            Gizmos.DrawWireSphere(startPoint, 0.5f);

            Gizmos.color = UnityEngine.Color.red;
            Gizmos.DrawWireSphere(endPoint, 0.5f);

            Gizmos.color = UnityEngine.Color.cyan;
            Gizmos.DrawSphere(movetoPos, 0.5f);

            Gizmos.color = UnityEngine.Color.gray;
            Gizmos.DrawSphere(rectCenter, 0.5f);
        }
    }

}
