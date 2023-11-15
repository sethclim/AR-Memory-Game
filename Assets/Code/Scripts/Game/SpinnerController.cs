using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using static UnityEngine.GraphicsBuffer;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpinnerController : MonoBehaviour
{
    [SerializeField] float Duration = 10;

    public void Spin()
    {
        StartCoroutine(nameof(Rotate));
    }

    IEnumerator Rotate()
    {
        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + 90.0f;
        float t = 0.0f;

        while (t < Duration)
        {
            t += Time.deltaTime;

            float yRotation = Mathf.Lerp(startRotation, endRotation, t / Duration) % 360.0f;

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation,
            transform.eulerAngles.z);

            yield return null;


        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SpinnerController))]
    public class SpinnerControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SpinnerController spinner = (SpinnerController)target;
            if (GUILayout.Button("Spin"))
            {
                spinner.Spin();
            }
        }
    }
#endif
}
