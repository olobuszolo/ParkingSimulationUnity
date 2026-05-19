using UnityEngine;

public class EntryBlockZone : MonoBehaviour
{
    private int carsInside = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            carsInside++;

            GameManager.Instance.OccupyEntry();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            carsInside--;

            if (carsInside <= 0)
            {
                carsInside = 0;

                GameManager.Instance.FreeEntry();
            }
        }
    }
}