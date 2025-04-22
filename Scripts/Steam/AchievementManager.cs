using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Steamworks;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
	public static AchievementEventRepository AchievementEventRepository;
	public static bool SimpleMindedAchieved => m_Achievements[Achievement.ACH_WIN_ONE_GAME].m_bAchieved;
    private enum Achievement : int {
		ACH_WIN_ONE_GAME, // Simple minded
		ACH_WIN_100_GAMES, // into the vacuumverse
		ACH_HEAVY_FIRE, // unplugged
		ACH_TRAVEL_FAR_ACCUM, // tomato OnTomato
		ACH_TRAVEL_FAR_SINGLE,
	};
	
	private static readonly Dictionary<Achievement, Achievement_t> m_Achievements = new Dictionary<Achievement, Achievement_t>
	{
		{ Achievement.ACH_WIN_ONE_GAME, new Achievement_t(Achievement.ACH_WIN_ONE_GAME, "Simple Minded", "We get it, you don't like to read.") },
		{ Achievement.ACH_WIN_100_GAMES, new Achievement_t(Achievement.ACH_WIN_100_GAMES, "Into the Vacuum-Verse", "Get sucked.") },
		{ Achievement.ACH_HEAVY_FIRE, new Achievement_t(Achievement.ACH_HEAVY_FIRE, "Are We the Baddies?", "That could have been a good guy.") },
		{ Achievement.ACH_TRAVEL_FAR_ACCUM, new Achievement_t(Achievement.ACH_TRAVEL_FAR_ACCUM, "They really didn't like you.", "Get hit with a tomato") },
	};

	// Our GameID
	private CGameID m_GameID;

	// Did we get the stats from Steam?
	private bool m_bRequestedStats;
	private bool m_bStatsValid;

	// Should we store stats this frame?
	private bool m_bStoreStats;

	// Current Stat details
	private float m_flGameFeetTraveled;
	private float m_ulTickCountGameStart;
	private double m_flGameDurationSeconds;

	// Persisted Stat details
	private int m_nTotalGamesPlayed;
	private int m_nTotalNumWins;
	private int m_nTotalNumLosses;
	private float m_flTotalFeetTraveled;
	private float m_flMaxFeetTraveled;
	private float m_flAverageSpeed;
	
	private bool subscribedToEvents = false;

	protected Callback<UserStatsReceived_t> m_UserStatsReceived;
	protected Callback<UserStatsStored_t> m_UserStatsStored;
	protected Callback<UserAchievementStored_t> m_UserAchievementStored;

	void OnEnable() {
		if (AchievementEventRepository == null)
			AchievementEventRepository = new AchievementEventRepository();
		
		if (!SteamManager.Initialized)
			return;

		// Cache the GameID for use in the Callbacks
		m_GameID = new CGameID(SteamUtils.GetAppID());

		m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
		m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
		m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);

		// These need to be reset to get the stats upon an Assembly reload in the Editor.
		m_bRequestedStats = false;
		m_bStatsValid = false;
	}

	[SerializeField]
	private Achievement AchievementToClear;
	[Button]
	public void CleanAchievementButton()
	{
		ResetAchievement(m_Achievements[AchievementToClear]);
	}

	private void Update() {
		if (!SteamManager.Initialized)
			return;

		if (!m_bRequestedStats) {
			// Is Steam Loaded? if no, can't get stats, done
			if (!SteamManager.Initialized) {
				m_bRequestedStats = true;
				return;
			}
			
			// If yes, request our stats
			bool bSuccess = SteamUserStats.RequestCurrentStats();

			// This function should only return false if we weren't logged in, and we already checked that.
			// But handle it being false again anyway, just ask again later.
			m_bRequestedStats = bSuccess;
		}
		
		if (!m_bStatsValid)
			return;
		
		// we got stats, so subscribe
		if (!subscribedToEvents)
		{
			SubscribeToEvents();
		}

		//Store stats in the Steam database if necessary
		if (m_bStoreStats) {
			// already set any achievements in UnlockAchievement

			// set stats
			/*SteamUserStats.SetStat("NumGames", m_nTotalGamesPlayed);
			SteamUserStats.SetStat("NumWins", m_nTotalNumWins);
			SteamUserStats.SetStat("NumLosses", m_nTotalNumLosses);
			SteamUserStats.SetStat("FeetTraveled", m_flTotalFeetTraveled);
			SteamUserStats.SetStat("MaxFeetTraveled", m_flMaxFeetTraveled);*/
			// Update average feet / second stat
			// SteamUserStats.UpdateAvgRateStat("AverageSpeed", m_flGameFeetTraveled, m_flGameDurationSeconds);
			// The averaged result is calculated for us
			// SteamUserStats.GetStat("AverageSpeed", out m_flAverageSpeed);
			bool bSuccess = SteamUserStats.StoreStats();
			// If this failed, we never sent anything to the server, try
			// again later.
			m_bStoreStats = !bSuccess;
		}
	}

	//-----------------------------------------------------------------------------
	// Purpose: Unlock this achievement
	//-----------------------------------------------------------------------------
	private void UnlockAchievement(Achievement_t achievement)
	{
		if (!SteamManager.Initialized)
			return;
		
		achievement.m_bAchieved = true;

		// the icon may change once it's unlocked
		//achievement.m_iIconImage = 0;

		// mark it down
		SteamUserStats.SetAchievement(achievement.m_eAchievementID.ToString());

		// Store stats end of frame
		m_bStoreStats = true;
	}
	
	private void ResetAchievement(Achievement_t achievement)
	{
		achievement.m_bAchieved = false;
        
		SteamUserStats.ClearAchievement(achievement.m_eAchievementID.ToString());
        
		m_bStoreStats = true;
	}
	
	//-----------------------------------------------------------------------------
	// Purpose: We have stats data from Steam. It is authoritative, so update
	//			our data with those results now.
	//-----------------------------------------------------------------------------
	private void OnUserStatsReceived(UserStatsReceived_t pCallback) {
		if (!SteamManager.Initialized)
			return;

		// we may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID) {
			if (EResult.k_EResultOK == pCallback.m_eResult) {
				Debug.Log("Received stats and achievements from Steam\n");

				m_bStatsValid = true;

				// load achievements
				foreach (Achievement_t ach in m_Achievements.Values) {
					bool ret = SteamUserStats.GetAchievement(ach.m_eAchievementID.ToString(), out ach.m_bAchieved);
					if (ret) {
						ach.m_strName = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "name");
						ach.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "desc");
					}
					else {
						Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + ach.m_eAchievementID + "\nIs it registered in the Steam Partner site?");
					}
				}
			}
			else {
				Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
			}
		}
	}

	//-----------------------------------------------------------------------------
	// Purpose: Our stats data was stored!
	//-----------------------------------------------------------------------------
	private void OnUserStatsStored(UserStatsStored_t pCallback) {
		// we may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID) {
			if (EResult.k_EResultOK == pCallback.m_eResult) {
				Debug.Log("StoreStats - success");
			}
			else if (EResult.k_EResultInvalidParam == pCallback.m_eResult) {
				// One or more stats we set broke a constraint. They've been reverted,
				// and we should re-iterate the values now to keep in sync.
				Debug.Log("StoreStats - some failed to validate");
				// Fake up a callback here so that we re-load the values.
				UserStatsReceived_t callback = new UserStatsReceived_t();
				callback.m_eResult = EResult.k_EResultOK;
				callback.m_nGameID = (ulong)m_GameID;
				OnUserStatsReceived(callback);
			}
			else {
				Debug.Log("StoreStats - failed, " + pCallback.m_eResult);
			}
		}
	}

	//-----------------------------------------------------------------------------
	// Purpose: An achievement was stored
	//-----------------------------------------------------------------------------
	private void OnAchievementStored(UserAchievementStored_t pCallback) {
		// We may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID) {
			if (0 == pCallback.m_nMaxProgress) {
				Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
			}
			else {
				Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
			}
		}
	}
	
	#region Achievement Handlers
    private void SubscribeToEvents()
    {
        if (!m_Achievements[Achievement.ACH_WIN_ONE_GAME].m_bAchieved)
            AchievementEventRepository.OnSimpleMinded += SimpleMinded;
        if (!m_Achievements[Achievement.ACH_WIN_100_GAMES].m_bAchieved)
	        AchievementEventRepository.OnVacuumVerse += VacuumVerse;
        if (!m_Achievements[Achievement.ACH_HEAVY_FIRE].m_bAchieved)
	        AchievementEventRepository.OnUnPlug += Unplug;
        if (!m_Achievements[Achievement.ACH_TRAVEL_FAR_ACCUM].m_bAchieved)
	        AchievementEventRepository.OnTomato += Tomato;
        
        subscribedToEvents = true;
    }

    private void UnSubAll()
    {
        AchievementEventRepository.OnSimpleMinded -= SimpleMinded;
        AchievementEventRepository.OnVacuumVerse -= VacuumVerse;
        AchievementEventRepository.OnUnPlug -= Unplug;
        AchievementEventRepository.OnTomato -= Tomato;
    }

    private void OnDisable()
    {
        UnSubAll();
    }

    private void SimpleMinded()
    {
        UnlockAchievement(m_Achievements[Achievement.ACH_WIN_ONE_GAME]);
        AchievementEventRepository.OnSimpleMinded -= SimpleMinded;
    }

    private void VacuumVerse()
    {
	    UnlockAchievement(m_Achievements[Achievement.ACH_WIN_100_GAMES]);
	    AchievementEventRepository.OnVacuumVerse -= VacuumVerse;
    }

    private void Unplug()
    {
	    UnlockAchievement(m_Achievements[Achievement.ACH_HEAVY_FIRE]);
	    AchievementEventRepository.OnUnPlug -= Unplug;
    }

    private void Tomato()
    {
	    UnlockAchievement(m_Achievements[Achievement.ACH_TRAVEL_FAR_ACCUM]);
	    AchievementEventRepository.OnTomato -= Tomato;
    }
    
    #endregion

	private class Achievement_t {
		public Achievement m_eAchievementID;
		public string m_strName;
		public string m_strDescription;
		public bool m_bAchieved;

		/// <summary>
		/// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
		/// </summary>
		/// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
		/// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
		/// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
		public Achievement_t(Achievement achievementID, string name, string desc) {
			m_eAchievementID = achievementID;
			m_strName = name;
			m_strDescription = desc;
			m_bAchieved = false;
		}
	}
}


