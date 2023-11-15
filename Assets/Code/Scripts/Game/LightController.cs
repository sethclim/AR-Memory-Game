using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ARMG
{
    public class LightController : MonoBehaviour
    {
        [SerializeField] Color m_lightColor;
        [SerializeField] MeshRenderer m_meshRenderer;
        [SerializeField] Light m_light;

        Color m_tranparentColor = new Color { r = 1.0f, g = 1.0f, b = 1.0f, a = 0.5f };

        public void ToggleLight(bool onOff) => ApplyLightSettings(onOff, m_lightColor);

        public void ToggleLight(bool onOff, Color color) => ApplyLightSettings(onOff, color);

        void ApplyLightSettings(bool onOff, Color color)
        {
            m_meshRenderer.material.color = onOff ? color : m_tranparentColor;
            m_light.color = color;
            m_light.enabled = onOff;
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
                    lightCOntrollerEditor.ToggleLight(true);
                }

                if (GUILayout.Button("Switch Off"))
                {
                    lightCOntrollerEditor.ToggleLight(false);
                }
            }
        }
#endif

    }
}