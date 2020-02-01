using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private Collider lajaColl = null;

    [SerializeField] private float fillSpeed = 0.5f;
    [SerializeField] private float growthSpeed = 0.5f;
    [SerializeField] private float offsetY = 1f;
    [SerializeField] private GameObject laja = null;

    private void Start()
    {
        lajaColl = laja.GetComponent<Collider>();

        // TODO: Move this
        lajaColl.enabled = true;
        laja.GetComponent<MeshRenderer>().enabled = true;

        laja.transform.localPosition -= Vector3.up * offsetY;
        laja.transform.localScale = new Vector3(laja.transform.localScale.x,
            0.001f, laja.transform.localScale.z);
    }

    public void Fill()
    {
        if (transform.position.y > laja.transform.position.y)
        {
            laja.transform.localPosition += Vector3.up * fillSpeed * Time.deltaTime;
        }
        else if (laja.transform.localScale.y < laja.transform.localScale.x + laja.transform.localScale.z)
        {
            laja.transform.localScale = new Vector3(laja.transform.localScale.x,
                laja.transform.localScale.y + growthSpeed * Time.deltaTime,
                laja.transform.localScale.z);
        }
        else
        {

            laja.transform.localScale = new Vector3(laja.transform.localScale.x + growthSpeed * 0.033f * Time.fixedDeltaTime,
                laja.transform.localScale.y + growthSpeed * 0.033f * Time.deltaTime,
                laja.transform.localScale.z + growthSpeed * 0.033f * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Line to hole bottom;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * offsetY);
    }
}
