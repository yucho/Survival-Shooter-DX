using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthPoint : MonoBehaviour
{
	public int maxHP = 100;
	public float healWait = 4f;
	public int healSpeed = 8; // per second
	public float waitBeforeFadeOut = 1f;
	public float opacity = 0.5f;
	public Image fill;

	private Slider healthBar;
	private CanvasGroup cGroup;
	private PlayerSFXManager playerSFX;
	private VFXManager VFX;
	private float timer;
	private float regenerateProgress;
	private bool regenerating;
	private bool invincible;
	private bool gameover;

	void Awake ()
	{
		healthBar = GetComponent<Slider> ();

		healthBar.minValue = 0;
		healthBar.maxValue = maxHP;

		cGroup = GetComponent<CanvasGroup> ();

		playerSFX = GameObject.FindWithTag ("Player").GetComponent<PlayerSFXManager> ();
		VFX = GameObject.FindWithTag ("VisualEffect").GetComponent<VFXManager> ();

		// Add invincibility. Player is invincible during event.
		NotificationCentre.AddObserver (this, "OnEventEnter");
		NotificationCentre.AddObserver (this, "OnEventExit");
		NotificationCentre.AddObserver (this, "OnResumeFromCheckpoint");
		invincible = false;

		Initialize ();
	}


	void Initialize ()
	{
		healthBar.value = maxHP;
		cGroup.alpha = 0;
		regenerateProgress = 0;
		regenerating = false;
		gameover = false;
	}


	void OnEventEnter ()
	{
		invincible = true;
	}
	
	void OnEventExit ()
	{
		invincible = false;
	}


	void Update ()
	{
		timer += Time.deltaTime;

		/**
		 *  Do nothing if game is over. Process game over if HP is zero.
		 *  Set visibility when HP is not full. Regenerate HP after healWait.
		 */
		if (gameover)
		{
			return;
		}
		else if (IsDead ())
		{
			GameOver ();
			return;
		}
		else if (regenerating)
		{
			Regenerate ();
		}
		else if (! IsMaxHP() && timer >= healWait)
		{
			Regenerate ();
			playerSFX.PlayerHealSound ();
		}
		else if (IsMaxHP() && timer >= waitBeforeFadeOut)
		{
			FadeInOut (false);
		}

		/**
		 *  Redden the bar when low in health.
		 */
		ChangeColor ();

		/**
		 *  Heart beat effect when low in health.
		 */
		HeartBeat ();
	}

	public void TakeDamage (int damage)
	{
		// Exit if player is invincible.
		if (invincible)
			return;

		/**
		 *  Player gets tougher when low in health.
		 */
		healthBar.value -= AdjustDamage (damage);

		timer = 0f;
		regenerating = false;

		/**
		 *  Display HP bar.
		 */
		FadeInOut (true);

		/**
		 *  Play player hurt voice.
		 */
		playerSFX.PlayerHurtVoice (0.1f);

		/**
		 *  Flash the screen opaque red.
		 */
		VFX.HurtFlash ();
	}

	public void TakeDamage (int damage, Vector3 fromDirection)
	{
		if (invincible)
			return;

		TakeDamage (damage);

		SplatterScreen (fromDirection);
	}

	int AdjustDamage (int damage)
	{
		/**
		 *  Reduce more damage the lower your health, but no more than 40%.
		 */
		float percent = healthBar.value / healthBar.maxValue;
		float modifier = percent >= 0.8f ? 1f : percent + 0.2f;

		/**
		 *  Prevent one hit KO from above 50% HP.
		 */
		int newDamage = Mathf.CeilToInt (modifier * damage);
		if (percent >= 0.5 && newDamage >= healthBar.value)
		{
			return (int) healthBar.value - 1;
		}
		else
		{
			return newDamage;
		}
	}

	void SplatterScreen (Vector3 fromDirection)
	{
		Color color = new Color (1f, 0f, 0f, 0.5f);

		VFX.DisplaySplatter (color, fromDirection);
	}

	void Regenerate ()
	{
		regenerating = true;
		regenerateProgress += healSpeed * Time.deltaTime;

		int healValue = regenerateProgress >= 1 ? Mathf.FloorToInt (regenerateProgress) : 0;
		healthBar.value += healValue;
		regenerateProgress -= healValue;

		if (IsMaxHP())
		{
			regenerating = false;
			regenerateProgress = 0;
			timer = 0f;
		}
	}

	public void FadeInOut (bool inOut)
	{
		if (inOut)
		{
			cGroup.alpha = 1f;
		}
		else
		{
			cGroup.alpha = Mathf.MoveTowards(cGroup.alpha, 0f, 0.5f * Time.deltaTime);
		}
	}

	bool IsMaxHP ()
	{
		return healthBar.value >= healthBar.maxValue;
	}

	bool IsDead ()
	{
		return healthBar.value <= 0;
	}

	void ChangeColor ()
	{
		float diminish = healthBar.value / healthBar.maxValue;

		fill.color = new Color(1f, diminish, diminish, opacity);
	}

	void HeartBeat ()
	{
		if (! playerSFX)
		{
			playerSFX = GameObject.FindWithTag ("Player").GetComponent<PlayerSFXManager> ();
		}

		if (! playerSFX)
			return;

		/**
		 *  Heart beat effect when HP <= 25%.
		 */
		if (healthBar.value / healthBar.maxValue <= 0.25f)
		{
			playerSFX.PlayHeartBeat (true);
			VFX.ToggleHeartBeatFlash (true);
		}
		else
		{
			playerSFX.PlayHeartBeat (false);
			VFX.ToggleHeartBeatFlash (false);
		}
	}


	void GameOver ()
	{
		//
		// Hide all visual and sound effects.
		//
		playerSFX.PlayHeartBeat (false);
		VFX.ToggleHeartBeatFlash (false);
		cGroup.alpha = 0;

		gameover = true;

		NotificationCentre.PostNotification (this, "OnGameOver");

		NotificationCentre.PostNotification (this, "OnPlayerDeath");
	}


	void OnResumeFromCheckpoint ()
	{
		Initialize ();
		gameover = false;
	}
}
