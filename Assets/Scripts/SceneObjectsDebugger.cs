using System.Text;
using UnityEngine;

public class SceneObjectsDebugger : MonoBehaviour
{
    void Start()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("=== SCENE OBJECTS ===");

        foreach (GameObject obj in allObjects)
        {
            Transform t = obj.transform;

            sb.AppendLine(
                $"{obj.name}:\n" +
                $"Position -> X={t.position.x}, Y={t.position.y}, Z={t.position.z}\n" +
                $"Scale -> X={t.localScale.x}, Y={t.localScale.y}, Z={t.localScale.z}\n"
            );
        }

        Debug.Log(sb.ToString());
    }
}