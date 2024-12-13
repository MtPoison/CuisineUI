using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderToParticles : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private ParticleSystem particleSystem; 
     [SerializeField] private GameObject button;
    [SerializeField] private TMP_Text count;
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.MainModule mainModule;

    void Start()
    {
        if (slider != null && particleSystem != null)
        {
            emissionModule = particleSystem.emission;
            mainModule = particleSystem.main;

            emissionModule.enabled = false;

            slider.onValueChanged.AddListener(OnSliderValueChanged);

            ApplySliderValue(slider.value);
        }
        else
        {
            Debug.LogWarning("Assurez-vous d'assigner le Slider et le ParticleSystem dans l'inspecteur.");
        }
    }

    void OnSliderValueChanged(float value)
    {
        ApplySliderValue(value);
    }

    void ApplySliderValue(float value)
    {
        if (value > 0)
        {
            emissionModule.enabled = true;
            float particleCount = Mathf.Lerp(5, 100, value);
            emissionModule.rateOverTime = particleCount;
            mainModule.startSpeed = Mathf.Lerp(1f, 10f, value);
            mainModule.startSize = Mathf.Lerp(0.1f, 2f, value);
            Vector3 currentRotation = button.transform.rotation.eulerAngles;
            button.transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, Mathf.Lerp(-90f, 90f, value));
        }
        else
        {
            emissionModule.enabled = false;
        }

    }
}
