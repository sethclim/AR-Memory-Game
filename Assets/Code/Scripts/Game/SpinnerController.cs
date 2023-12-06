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
    public int testIndex = 0;

    Dictionary<int, int> rotations = new()
    {
        {0, 90 },
        {1, 180 },
        {2, 270 },
        {3, 0 }
    };


    float EndRotation = 0;
    Quaternion _lookRotation;
    int currentIndex = -1;

    int prevIndex = 0;

    public void Spin(int index)
    {
        //CalcEndRotation(index);
        prevIndex = currentIndex;
        currentIndex = index;
        StartCoroutine(nameof(Rotate));
    }

    public void SpinLocal(int index)
    {
        currentIndex = index;
        Debug.Log("index " + index);
        //CalcEndRotation(index);
        StartCoroutine(nameof(Rotate));
    }


    //void CalcEndRotation(int index)
    //{
    //    var loc = baseLocations[index].position;
    //    var direction = (loc - transform.position).normalized;
    //    var lookRot = Quaternion.LookRotation(direction).eulerAngles;
    //    _lookRotation.eulerAngles = new Vector3(0, lookRot.y, 0);

    //    Debug.Log(transform.rotation.eulerAngles + " " + _lookRotation.eulerAngles);
    //}   

    IEnumerator Rotate()
    {
        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + 90.0f;
        float t = 0.0f;

        while (t < Duration)
        {
            t += Time.deltaTime;

            int targetRot = rotations[currentIndex];
            var diff = targetRot - rotations[prevIndex];

            if (diff < 0)
                targetRot += 360;

            float yRotation = Mathf.Lerp(rotations[prevIndex], targetRot, Duration / t); //% 360.0f;

            Debug.Log("yRotation " + yRotation);

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);

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
                spinner.Spin(spinner.testIndex);
            }
        }
    }
#endif
}
