using UnityEngine;

public class TelekinesisController : MonoBehaviour
{
    public float maxTelekinesisDuration = 5f;
    public float cooldownDuration = 3f;
    public float rechargeRate = 1f; // How fast the ability recharges when not in use

    private float currentTelekinesisDuration;
    private float cooldownTimeLeft;
    private bool isCoolingDown = false;

    private MoveableObject currentObject;
    private Vector2 initialOffset;

    [SerializeField] private AudioClip telekinesisClip;
    [SerializeField] private AudioClip cooldownClip;

    void Start()
    {
        currentTelekinesisDuration = maxTelekinesisDuration;
        cooldownTimeLeft = 0f;
    }

    void Update()
    {
        HandleInput();
        UpdateTimers();
    }

    void HandleInput()
    {
        if (isCoolingDown) return;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (Input.GetKey(KeyCode.Space) && currentTelekinesisDuration > 0)
        {
            if (currentObject == null)
            {
                RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
                if (hit.collider != null)
                {
                    MoveableObject hitObject = hit.collider.GetComponent<MoveableObject>();
                    if (hitObject != null)
                    {
                        StartTelekinesis(hitObject, worldPosition);
                    }
                }
            }
        }
        else if (currentObject != null)
        {
            EndTelekinesis();
        }
    }

    void UpdateTimers()
    {
        if (isCoolingDown)
        {
            cooldownTimeLeft -= Time.deltaTime;
            if (cooldownTimeLeft <= 0)
            {
                SoundFXManager.instance.StopSoundFX(cooldownClip.name);
                isCoolingDown = false;
                cooldownTimeLeft = 0f;
                // Don't reset telekinesis duration here, it will recharge gradually
            }
        }
        else if (currentObject != null)
        {
            currentTelekinesisDuration -= Time.deltaTime;
            if (currentTelekinesisDuration <= 0)
            {
                EndTelekinesis();
            }
        }
        else
        {
            // Recharge the ability when not in use and not in cooldown
            currentTelekinesisDuration = Mathf.Min(currentTelekinesisDuration + rechargeRate * Time.deltaTime, maxTelekinesisDuration);
        }
    }

    void StartTelekinesis(MoveableObject obj, Vector2 worldPosition)
    {
        SoundFXManager.instance.PlaySoundFXClip(telekinesisClip, obj.transform, 1f);
        currentObject = obj;
        initialOffset = (Vector2)obj.transform.position - worldPosition;
        currentObject.transform.Find("Particle System").gameObject.SetActive(true);
    }

    void EndTelekinesis()
    {
        SoundFXManager.instance.StopSoundFX(telekinesisClip.name);
        currentObject.transform.Find("Particle System").gameObject.SetActive(false);
        currentObject = null;
        if (currentTelekinesisDuration <= 0)
        {
            isCoolingDown = true;
            cooldownTimeLeft = cooldownDuration;
            SoundFXManager.instance.PlaySoundFXClip(cooldownClip, Camera.main.transform, 1f);
        }
    }

    void FixedUpdate()
    {
        if (currentObject != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane;
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector2 targetPosition = worldPosition + initialOffset;
            currentObject.Move(targetPosition);
        }
    }

    // Properties for UI
    public float TelekinesisTimeLeftNormalized => currentTelekinesisDuration / maxTelekinesisDuration;
    public float CooldownTimeLeftNormalized => isCoolingDown ? cooldownTimeLeft / cooldownDuration : 0f;
    public bool IsInCooldown => isCoolingDown;
    public bool IsTelekinesisActive => currentObject != null;
}