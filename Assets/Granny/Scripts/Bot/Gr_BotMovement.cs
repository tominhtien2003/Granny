using System;
using UnityEngine;
using UnityEngine.AI;

public class Gr_BotMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 destination, float moveSpeed)
    {
        agent.isStopped = false;
        agent.speed = moveSpeed;
        agent.SetDestination(destination);
    }

    public void StopMoving()
    {
        if (!agent.isOnNavMesh) return;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        if (agent.hasPath)
        {
            agent.ResetPath();
        } 
    }

    public void FacePlayer(Vector3 position)
    {
        Vector3 dir = (position - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public bool HasReachedDestination(float stoppingDistance = 0.5f)
    {
        if (agent.pathPending) return false;
        return agent.remainingDistance <= stoppingDistance;
    }
}
