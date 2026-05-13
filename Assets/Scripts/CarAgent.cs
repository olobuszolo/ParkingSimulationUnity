using UnityEngine;
using UnityEngine.AI;

public class CarAgent : MonoBehaviour
{
    public Transform parkingEntryPoint;

    public Transform[] circlePoints;

    private NavMeshAgent agent;

    private int currentCircleIndex = 0;

    private bool isCircling = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(parkingEntryPoint.position);
    }

    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (!isCircling)
            {
                StartCircling();
            }
            else
            {
                GoToNextCirclePoint();
            }
        }
    }

    private void StartCircling()
    {
        isCircling = true;

        currentCircleIndex = 0;

        agent.SetDestination(circlePoints[currentCircleIndex].position);
    }

    private void GoToNextCirclePoint()
    {
        currentCircleIndex++;

        if (currentCircleIndex >= circlePoints.Length)
        {
            currentCircleIndex = 0;
        }

        agent.SetDestination(circlePoints[currentCircleIndex].position);
    }
}