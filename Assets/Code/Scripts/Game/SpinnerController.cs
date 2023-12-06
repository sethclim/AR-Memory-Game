using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpinnerController : MonoBehaviour
{
    [SerializeField] float Duration = 10;
    [SerializeField] List<Transform> baseLocations = new();

    float EndRotation = 0;

    public void Spin(int index)
    {
        CalcEndRotation(index);
        StartCoroutine(nameof(Rotate));
    }

    public void SpinLocal(int index)
    {
        CalcEndRotation(index);
        StartCoroutine(nameof(Rotate));
    }

    Quaternion _lookRotation;

    void CalcEndRotation(int index)
    {
        var loc = baseLocations[index].position;
        var direction = (loc - transform.position).normalized;
        var lookRot = Quaternion.LookRotation(direction).eulerAngles;
        _lookRotation.eulerAngles = new Vector3(0, lookRot.y, 0);

        Debug.Log(transform.rotation.eulerAngles + " " + _lookRotation.eulerAngles);
    }   

    IEnumerator Rotate()
    {
        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + 90.0f;
        float t = 0.0f;

        while (t < Duration)
        {
            t += Time.deltaTime;

            //float yRotation = Mathf.Lerp(startRotation, to - startRotation, t / Duration) % 360.0f;

            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation,
            //transform.eulerAngles.z);

            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime  / Duration);

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
                spinner.Spin(1);
            }
        }
    }
#endif
}
