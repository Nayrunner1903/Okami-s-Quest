using UnityEngine;

public class CheckIfCollide : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player prefab entered the sphere trigger.");
        }
    }
}
