using TMPro;
using UnityEngine;
using UnityEngine.UI;


class ToolPathSimulatorUI : MonoBehaviour
{
    [SerializeField] private ToolPathSimulator _toolPathSimulator;

    [SerializeField] private Button _startButton;
    [SerializeField] private Slider _slider;
    [SerializeField] private Slider _sliderSpeed;
    private TextMeshProUGUI _sliderText;
    private TextMeshProUGUI _sliderSpeedText;

    private void Start()
    {
        _startButton.onClick.AddListener(_toolPathSimulator.Execute);

        _slider.maxValue = 100; // Percentage of path complete
        _sliderText = _slider.GetComponentInChildren<TextMeshProUGUI>();
        _sliderSpeedText = _sliderSpeed.GetComponentInChildren<TextMeshProUGUI>();

        _sliderSpeed.maxValue = _toolPathSimulator.MaxSpeed; // MAX ROBOT SPEED
        _sliderSpeed.value = _toolPathSimulator.MovementSpeed;
        _sliderSpeed.onValueChanged.AddListener(GUISpeedChange);

        _toolPathSimulator.ToolPathSimulating.AddListener(OnContinueSimulation);
        GUISpeedChange(_sliderSpeed.value);
    }

    private void OnContinueSimulation()
    {
        _slider.maxValue = _toolPathSimulator.TotalTravelDistance;
        _slider.value = _toolPathSimulator.DistanceTraveled;
        _sliderText.text = _slider.value.ToString() + " m";
    }

    private void GUISpeedChange(float newVal)
    {
        _toolPathSimulator.MovementSpeed = newVal;
        _sliderSpeedText.text = newVal.ToString();
    }
}

