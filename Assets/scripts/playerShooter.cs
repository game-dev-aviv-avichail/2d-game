
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PlayerShooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Bullet bulletPrefab;

    [Header("Shooting Settings")]
    [SerializeField] private float shotCooldown = 0.2f;
    [SerializeField] private float spawnOffset = 0.6f;

    [Header("Auto Shooting")]
    [SerializeField] private bool autoShootEnabled = false;

    private Vector2 facingDirection = Vector2.down;
    private Vector3 previousPosition;
    private float nextShotTime;

    void Start()
    {
        previousPosition = transform.position;
    }

    void LateUpdate()
    {
        // Detect the direction the player is walking
        Vector2 movement =
            (Vector2)(transform.position - previousPosition);

        if (movement.sqrMagnitude > 0.000001f)
        {
            facingDirection = movement.normalized;
        }

        previousPosition = transform.position;

        // Press E to toggle automatic shooting
        if (TogglePressed())
        {
            autoShootEnabled = !autoShootEnabled;

            if (autoShootEnabled)
                nextShotTime = Time.time;

            Debug.Log("Auto Shooting: " +
                (autoShootEnabled ? "ON" : "OFF"));
        }

        if (bulletPrefab == null)
            return;

        // Manual shooting (Space or left-click)
        bool manualShoot = ShootPressed();

        // Automatic shooting
        bool automaticShoot =
            autoShootEnabled && Time.time >= nextShotTime;

        if ((manualShoot || automaticShoot) &&
            Time.time >= nextShotTime)
        {
            Shoot();

            nextShotTime =
                Time.time + Mathf.Max(0.05f, shotCooldown);
        }
    }

    private bool TogglePressed()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null &&
               Keyboard.current.eKey.wasPressedThisFrame;

#elif ENABLE_LEGACY_INPUT_MANAGER
        return Input.GetKeyDown(KeyCode.E);
#else
        return false;
#endif
    }

    private bool ShootPressed()
    {
#if ENABLE_INPUT_SYSTEM
        bool space = Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame;

        bool click = Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame;

        return space || click;

#elif ENABLE_LEGACY_INPUT_MANAGER
        return Input.GetKeyDown(KeyCode.Space) ||
               Input.GetMouseButtonDown(0);
#else
        return false;
#endif
    }

    private void Shoot()
    {
        Vector3 spawnPosition =
            transform.position +
            (Vector3)(facingDirection * spawnOffset);

        Bullet bullet = Instantiate(
            bulletPrefab,
            spawnPosition,
            Quaternion.identity
        );

        bullet.Launch(facingDirection);
    }
}
