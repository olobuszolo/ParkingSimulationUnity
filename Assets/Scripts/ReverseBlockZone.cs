using UnityEngine;

public class ReverseBlockZone : MonoBehaviour
{
    private int carsInside = 0;

    public static bool IsBlocked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            carsInside++;
            IsBlocked = true;
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
                IsBlocked = false;
            }
        }
    }
}