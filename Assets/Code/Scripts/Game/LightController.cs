
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class LightController : MonoBehaviour
{
    [SerializeField] MeshRenderer _lightRenderer;
    [SerializeField] Color _onColor;
    [SerializeField] Color _offColor;
    [SerializeField] Light _light;

    [SerializeField] float offIntensity = 1;
    [SerializeField] float onIntensity = 5;

    [SerializeField] Material _materialOff;
    [SerializeField] Material _materialOn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    public void SwitchLight(bool onOff)
    {
        Debug.Log("MATS " + _lightRenderer.materials[0].name);
        if(onOff)
        {
            _lightRenderer.material = _materialOn;
            _light.color = _onColor;
            _light.intensity = onIntensity;
            Debug.Log("MATS " + _lightRenderer.materials[0].name);

        }
        else
        {
            _lightRenderer.material = _materialOff;
            _light.color = _offColor;
            _light.intensity = offIntensity;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(LightController))]
    public class LightControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            LightController lightCOntrollerEditor = (LightController)target;
            if (GUILayout.Button("Switch On"))
            {
                lightCOntrollerEditor.SwitchLight(true);
            }

            if (GUILayout.Button("Switch Off"))
            {
                lightCOntrollerEditor.SwitchLight(false);
            }
        }
    }
#endif

}
