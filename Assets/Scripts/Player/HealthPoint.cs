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
	private bool regenerating;

	void Awake ()
	{
		healthBar = GetComponent<Slider> ();

		healthBar.minValue = 0;
		healthBar.maxValue = maxHP;
		healthBar.value = maxHP;

		cGroup = GetComponent<CanvasGroup> ();
		cGroup.alpha = 0;

		playerSFX = GameObject.FindWithTag ("Player").GetComponent<PlayerSFXManager> ();
		VFX = GameObject.FindWithTag ("VisualEffect").GetComponent<VFXManager> ();

		regenerating = false;
	}

	void Update ()
	{
		timer += Time.deltaTime;

		/**
		 *  Set visibility when HP is not full. Regenerate HP after healWait.
		 */
		if (regenerating)
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
		TakeDamage (damage);

		SplatterScreen (fromDirection);
	}

	int AdjustDamage (int damage)
	{
		/**
		 *  Reduce more damage the lower your health, but no more than 30%.
		 */
		float percent = healthBar.value / healthBar.maxValue;
		float modifier = Mathf.Max(0.3f, percent);


		/**
		 *  Prevent one hit KO from above 40% HP.
		 */
		int newDamage = Mathf.CeilToInt (modifier * damage);
		if (percent >= 0.4 && newDamage >= healthBar.value)
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
		healthBar.value += Mathf.CeilToInt(healSpeed * Time.deltaTime);

		if (IsMaxHP())
		{
			regenerating = false;
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

	void ChangeColor ()
	{
		float diminish = healthBar.value / healthBar.maxValue;

		fill.color = new Color(1f, diminish, diminish, opacity);
	}

	void HeartBeat ()
	{
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
}
