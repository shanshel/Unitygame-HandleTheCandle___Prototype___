﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine.EventSystems;

namespace MoreMountains.TopDownEngine
{	
	/// <summary>
	/// Handles all GUI effects and changes
	/// </summary>
	public class GUIManager : Singleton<GUIManager> 
	{
		/// the main canvas
		public Canvas MainCanvas;
		/// the game object that contains the heads up display (avatar, health, points...)
		public GameObject HUD;
		/// the jetpack bar
		public MMProgressBar[] HealthBars;
		/// the jetpack bar
		public MMRadialProgressBar[] DashBars;
		/// the panels and bars used to display current weapon ammo
		/// the pause screen game object
		public GameObject PauseScreen;
        /// the death screen
        public GameObject DeathScreen;
		/// The mobile buttons
		public CanvasGroup Buttons;
		/// The mobile arrows
		public CanvasGroup Arrows;
		/// The mobile movement joystick
		public CanvasGroup Joystick;
		/// the points counter
		public Text PointsText;
		/// the level display
		public Text LevelText;

		protected float _initialJoystickAlpha;
		protected float _initialButtonsAlpha;

		/// <summary>
		/// Initialization
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			if (Joystick!=null)
			{
				_initialJoystickAlpha=Joystick.alpha;
			}
			if (Buttons!=null)
			{
				_initialButtonsAlpha=Buttons.alpha;
			}
		}

	    /// <summary>
	    /// Initialization
	    /// </summary>
	    protected virtual void Start()
		{
            SetPauseScreen(false);
            SetDeathScreen(false);
		}

	    /// <summary>
	    /// Sets the HUD active or inactive
	    /// </summary>
	    /// <param name="state">If set to <c>true</c> turns the HUD active, turns it off otherwise.</param>
	    public virtual void SetHUDActive(bool state)
	    {
	        if (HUD!= null)
	        { 
	            HUD.SetActive(state);
	        }
	        if (PointsText!= null)
	        { 
	            PointsText.enabled = state;
	        }
	        if (LevelText!= null)
	        { 
	            LevelText.enabled = state;
	        }
	    }

	    /// <summary>
	    /// Sets the avatar active or inactive
	    /// </summary>
	    /// <param name="state">If set to <c>true</c> turns the HUD active, turns it off otherwise.</param>
	    public virtual void SetAvatarActive(bool state)
	    {
	        if (HUD != null)
	        {
	            HUD.SetActive(state);
	        }
	    }

        /// <summary>
        /// Sets the pause screen on or off.
        /// </summary>
        /// <param name="state">If set to <c>true</c>, sets the pause.</param>
        public virtual void SetPauseScreen(bool state)
        {
            if (PauseScreen != null)
            {
                PauseScreen.SetActive(state);
                EventSystem.current.sendNavigationEvents = state;
            }
        }

        /// <summary>
        /// Sets the death screen on or off.
        /// </summary>
        /// <param name="state">If set to <c>true</c>, sets the pause.</param>
        public virtual void SetDeathScreen(bool state)
        {
            if (DeathScreen != null)
            {
                DeathScreen.SetActive(state);
                EventSystem.current.sendNavigationEvents = state;
            }
        }

        /// <summary>
        /// Sets the jetpackbar active or not.
        /// </summary>
        /// <param name="state">If set to <c>true</c>, sets the pause.</param>
        public virtual void SetDashBar(bool state, string playerID)
		{
			if (DashBars == null)
			{
				return;
			}

			foreach (MMRadialProgressBar jetpackBar in DashBars)
			{
				if (jetpackBar != null)
		        { 
		        	if (jetpackBar.PlayerID == playerID)
		        	{
						jetpackBar.gameObject.SetActive(state);
		        	}					
		        }
			}	        
	    }

		/// <summary>
		/// Sets the ammo displays active or not
		/// </summary>
		/// <param name="state">If set to <c>true</c> state.</param>
		/// <param name="playerID">Player I.</param>


	    /// <summary>
	    /// Updates the health bar.
	    /// </summary>
	    /// <param name="currentHealth">Current health.</param>
	    /// <param name="minHealth">Minimum health.</param>
	    /// <param name="maxHealth">Max health.</param>
	    /// <param name="playerID">Player I.</param>
	    public virtual void UpdateHealthBar(float currentHealth,float minHealth,float maxHealth,string playerID)
	    {
			if (HealthBars == null) { return; }
			if (HealthBars.Length <= 0)	{ return; }

	    	foreach (MMProgressBar healthBar in HealthBars)
	    	{
				if (healthBar == null) { continue; }
				if (healthBar.PlayerID == playerID)
				{
					healthBar.UpdateBar(currentHealth,minHealth,maxHealth);
				}
	    	}

	    }

	    /// <summary>
	    /// Updates the jetpack bar.
	    /// </summary>
	    /// <param name="currentFuel">Current fuel.</param>
	    /// <param name="minFuel">Minimum fuel.</param>
	    /// <param name="maxFuel">Max fuel.</param>
	    /// <param name="playerID">Player I.</param>
		public virtual void UpdateDashBars(float currentFuel, float minFuel, float maxFuel,string playerID)
		{
			if (DashBars == null)
			{
				return;
			}

			foreach (MMRadialProgressBar dashbar in DashBars)
	    	{
				if (dashbar == null) { return; }
				if (dashbar.PlayerID == playerID)
				{
					dashbar.UpdateBar(currentFuel,minFuel,maxFuel);	
		    	}    
			}
	    }

		/// <summary>
		/// Updates the (optional) ammo displays.
		/// </summary>
		/// <param name="magazineBased">If set to <c>true</c> magazine based.</param>
		/// <param name="totalAmmo">Total ammo.</param>
		/// <param name="maxAmmo">Max ammo.</param>
		/// <param name="ammoInMagazine">Ammo in magazine.</param>
		/// <param name="magazineSize">Magazine size.</param>
		/// <param name="playerID">Player I.</param>
		/// <param name="displayTotal">If set to <c>true</c> display total.</param>
	
		
		/// <summary>
		/// Sets the level name in the HUD
		/// </summary>
		public virtual void SetLevelName(string name)
		{
	        if (LevelText!= null)
	        { 
	    		LevelText.text=name;
	        }
	    }
	}
}