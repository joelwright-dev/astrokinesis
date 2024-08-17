using UnityEngine;
using UnityEngine.UI;

public class TelekinesisUI : MonoBehaviour
{
    public TelekinesisController telekinesisController;
    public Slider telekinesisSlider;
    public Slider cooldownSlider;

    private Color originalColor;

    void Start() {
        originalColor = telekinesisSlider.gameObject.transform.Find("Background").GetComponent<Image>().color;
    }

    void Update()
    {
        // Update telekinesis fill
        telekinesisSlider.value = telekinesisController.TelekinesisTimeLeftNormalized;

        // Update cooldown fill
        cooldownSlider.value = telekinesisController.CooldownTimeLeftNormalized;

        // Optional: You can also change colors or activate/deactivate UI elements based on the state
        if(telekinesisSlider.value < 0.4f) {
            telekinesisSlider.gameObject.transform.Find("Background").GetComponent<Image>().color = Color.yellow;
            telekinesisSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.yellow;
        } else {
telekinesisSlider.gameObject.transform.Find("Background").GetComponent<Image>().color = originalColor;
            telekinesisSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = originalColor;
        }

        cooldownSlider.gameObject.SetActive(telekinesisController.IsInCooldown);
    }
}