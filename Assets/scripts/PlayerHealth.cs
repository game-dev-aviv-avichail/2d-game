
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float invulnerabilityTime = 0.75f;

    private int currentHealth;
    private float nextDamageTime;
    private bool isDead = false;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    void Start()
    {
        maxHealth = Mathf.Max(1, maxHealth);
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead || damage <= 0)
            return;

        // Prevent taking damage too frequently
        if (Time.time < nextDamageTime)
            return;

        currentHealth = Mathf.Max(
            0,
            currentHealth - damage
        );

        nextDamageTime =
            Time.time + invulnerabilityTime;

        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead || amount <= 0)
            return;

        currentHealth = Mathf.Min(
            maxHealth,
            currentHealth + amount
        );
    }

    private void Die()
    {
        isDead = true;

        Debug.Log("GAME OVER!");

        Time.timeScale = 0f;
    }

    void OnGUI()
    {
        float x = 20f;
        float y = 20f;
        float width = 220f;
        float height = 25f;

        float healthPercent =
            (float)currentHealth / maxHealth;

        // Health bar background
        GUI.Box(
            new Rect(x, y, width, height),
            GUIContent.none
        );

        // Health bar fill
        Color oldColor = GUI.color;

        GUI.color = Color.green;

        GUI.DrawTexture(
            new Rect(
                x + 3,
                y + 3,
                (width - 6) * healthPercent,
                height - 6
            ),
            Texture2D.whiteTexture
        );

        GUI.color = oldColor;

        // Health text
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;

        GUI.Label(
            new Rect(x, y, width, height),
            currentHealth + " / " + maxHealth,
            style
        );

        if (isDead)
        {
            GUIStyle gameOverStyle =
                new GUIStyle(GUI.skin.box);

            gameOverStyle.fontSize = 28;
            gameOverStyle.alignment =
                TextAnchor.MiddleCenter;

            GUI.Box(
                new Rect(
                    Screen.width / 2f - 150,
                    Screen.height / 2f - 50,
                    300,
                    100
                ),
                "GAME OVER",
                gameOverStyle
            );
        }
    }
}
