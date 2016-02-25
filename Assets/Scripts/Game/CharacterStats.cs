using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System;

/// <summary>
/// Keeps track of global score and score by level.
/// Implements saving and loading stats.
/// Keeps three save profiles.
/// </summary>
public class CharacterStats : MonoBehaviour {
	
	/*
	 * Player Profile Structure Definition
	 * 
	 * The player profile settings are stored as a string with comma separated values
	 * that in turn are separated by ":".  In some cases additional separations are done
	 * with "|"
	 * 
	 * <SettingName 01>:<SettingValue 01>,<SettingName 02>:<SettingValue 02,01>|<SettingValue 02,02>,<SettingName 03>:<SettingValue 03>
	 * 
	 * */
	
	public enum NonBasicCharacterSkill
	{
		SPRINT = 0,
		DOUBLE_JUMP = 1,
		SLIDE = 2,
		WALL_SLIDE = 3,
		ATTACK = 4
	}
	
	
	public static Vector3 LastPosition;
	public static Quaternion LastRotation;
	public static long GlobalScore;
	public static string CurrentLevelName;
	public static long CurrentLevelScore;
	public static int CurrentCreditCount;
	public static int CreditCount;
	public static int CurrentBugCount;
	public static int BugCount;
	public static int MaxLivesCount;
	public static int LivesCount;
	
	public static string CurrentProfileName;
	public static Dictionary<string, long> LevelScores;
	public static Dictionary<string, long> LevelCredits;
	public static Dictionary<string, long> LevelBugs;
	public static List<NonBasicCharacterSkill> Skills;
	public static string LastProfileName;
	public static float MaxLife;
	public static float CurrentLife;

	public static States CurrentState;	
	public static States oldState;
	public static bool HasValue = false;
	
	/////// We need this variable to know if we are coming from or to andy
	public static int fromAndy = 0;
	
	///////
	#region Score Methods
	
	/// <summary>
	/// Adds points to the current level.
	/// </summary>
	/// <param name='points'>
	/// The point amount to add.
	/// </param>
	public static void AddPointsToLevel(long points) {
		CurrentLevelScore += points;		
	}
	
	/// <summary>
	/// Sets the current level score to 0
	/// </summary>
	public static void ClearLevelScore() {
		CurrentLevelScore = 0;	
	}
	
	/// <summary>
	/// Returns if the level is clear.
	/// </summary>
	public static bool ClearedLevel() {
		if(LevelCredits != null) {
			if(LevelCredits.ContainsKey(CurrentLevelName)) {
				return true;
			}
			else {
				return false;
			}
		}
		else {
			return false;
		}
	}
	
	public static bool ClearedLevel(string RequestedLevelName) {
		if(LevelCredits.ContainsKey(RequestedLevelName)) {
			return true;
		}
		else {
			return false;
		}
	}
	
	/// <summary>
	/// Applies the level score to the global score and resets it to 0
	/// </summary>
	public static void ApplyLevelScore() {

		if(LevelScores == null) {
			LevelScores = new Dictionary<string, long>();	
		}

		//Adds the level score only if it is greater than the previous level score.
		if(!LevelScores.ContainsKey(CurrentLevelName) || CurrentLevelScore > LevelScores[CurrentLevelName]) {
			
			if(LevelScores.ContainsKey(CurrentLevelName)) {
				if(LevelScores[CurrentLevelName] > GlobalScore) {
					GlobalScore = 0;
				} else {
					//Substract previous score	
					GlobalScore -= LevelScores[CurrentLevelName];
				}
			}
			
			//Updates global score
			GlobalScore += CurrentLevelScore;
			LevelScores[CurrentLevelName] = CurrentLevelScore;
		}
		
		ClearLevelScore();
	}
	
	#endregion
	
	#region Life Methods
	
	public delegate void ReducedLifeHandler();
	public static event ReducedLifeHandler OnReducedLife;
	
	/// <summary>
	/// Resets the 0 to 100 life count.
	/// </summary>
	public static void ResetLife() {
		CurrentLife = MaxLife;	
	}
	
	/// <summary>
	/// Reduces the 0 to 100 life by an amount.
	/// </summary>
	/// <param name='amount'>
	/// The amount to reduce from the life meter
	/// </param>
	public static void ReduceLife(float amount) {
		if(amount >= CurrentLife) {
			CurrentLife = 0;	
			RemoveSingleLife();
		} else {
			CurrentLife -= amount;
		}
		
		EventContext.AddEvent("ReducedLife");
		if(OnReducedLife != null) {
			OnReducedLife();	
		}
		
		if(CurrentLife == 0) {
			EventContext.AddEvent("Dead");
		}
	}
	
	public delegate void LifeLostHandler(int remaining);
	public static event LifeLostHandler OnLifeLost;
	
	
	public static void SetMaxLifeCount(int maxLifeCount) {
		MaxLivesCount = maxLifeCount;	
	}
	
	/// <summary>
	/// Resets the discrete life count.
	/// </summary>
	public static void ResetLifeCount() {
		LivesCount = MaxLivesCount;
	}
	public static void RemoveSingleLife() {
		if(LivesCount == 0) {
			EventContext.AddEvent("NoLivesRemaining");
		}

		if(LivesCount > 0) {
			LivesCount--;
		}
		if(OnLifeLost != null) {
			OnLifeLost(LivesCount);	
		}
		
		
	}
	
	#endregion
	
	#region Profile Handling Methods
	
	/// <summary>
	/// Determines whether this instance is first run.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is first run; otherwise, <c>false</c>.
	/// </returns>
	public static bool IsFirstRun() {
		return !PlayerPrefs.HasKey("Profiles");
	}
	
	/// <summary>
	/// Saves the stats for the current profile. Uses Unity's multiplatform PlayerPrefs class.
	/// </summary>
	public static void SaveStats() {
		StringBuilder collection = new StringBuilder();
		collection.Append("LastPosition:" + LastPosition.x.ToString()
			+ "|" + LastPosition.y.ToString()
			+ "|" + LastPosition.z.ToString());
		collection.Append(",LastRotation:" + LastRotation.x.ToString()
			+ "|" + LastRotation.y.ToString()
			+ "|" + LastRotation.z.ToString()
			+ "|" + LastRotation.w.ToString());
		collection.Append(",GlobalScore:" + GlobalScore.ToString());
		collection.Append(",CreditCount:" + CreditCount.ToString());
		collection.Append(",MaxLife:" + CreditCount.ToString());
		collection.Append(",CurrentLife:" + CreditCount.ToString());
		if(LevelCredits != null) {
			string levelCreditInfo = string.Empty;
			foreach(string levelName in LevelCredits.Keys) {
				levelCreditInfo += (levelCreditInfo.Length > 0 ? "|" : string.Empty) 
					+ levelName + "-" + LevelCredits[levelName].ToString();
			}
			collection.Append(",LevelCredits:" + levelCreditInfo);
		}
		if(LevelBugs != null) {
			string levelBugInfo = string.Empty;
			foreach(string levelName in LevelBugs.Keys) {
				levelBugInfo += (levelBugInfo.Length > 0 ? "|" : string.Empty) 
					+ levelName + "-" + LevelBugs[levelName].ToString();
			}
			collection.Append(",LevelBugs:" + levelBugInfo);
		}
		if(Skills != null) {
			collection.Append(",Skills:");
			string skillString = string.Empty;
			foreach(NonBasicCharacterSkill skill in Skills) {
				skillString += (skillString.Length > 0 ? "|" : string.Empty)
					+ skill.ToString();
			}
			collection.Append (skillString);
		}
		if(LevelScores != null) {
			foreach(string levelName in LevelScores.Keys) {
				collection.Append("," + levelName + ":" + LevelScores[levelName].ToString());
			}
		}

		string stringValue = collection.ToString();
		PlayerPrefs.SetString(CurrentProfileName, stringValue);
		PlayerPrefs.Save();
	}
	
	/// <summary>
	/// Clears the stats for the current profile
	/// </summary>
	public static void ClearStats() {
		LastPosition = Vector3.zero;
		LastRotation = Quaternion.identity;
		
		GlobalScore = 0;
		CreditCount = 0;
		BugCount = 0;
		MaxLife = 100;
		MaxLivesCount = 2;
		CurrentLife = 100;
		
		if(LevelBugs != null)
			LevelBugs.Clear();
		if(LevelCredits != null)
			LevelCredits.Clear();
		if(LevelScores != null)
			LevelScores.Clear();
		if(Skills != null)
			Skills.Clear();
		
	}
	
	/// <summary>
	/// Loads the stats for the current profile. Uses Unity's multiplatform PlayerPrefs class.
	/// </summary>
	public static void LoadStats() {
		if(!PlayerPrefs.HasKey(CurrentProfileName)) {
			ClearStats();
		} else {
			string playerStats = PlayerPrefs.GetString(CurrentProfileName);
			string[] settings = playerStats.Split(',');
			
			foreach(string setting in settings) {
				string settingName = setting.Split(':')[0];
				if(!string.IsNullOrEmpty(settingName)) {
					
					string settingValue = setting.Split(':')[1];
					
					switch(settingName) {
						case "LastPosition":
							LastPosition = new Vector3(
								float.Parse(settingValue.Split('|')[0]), 
								float.Parse(settingValue.Split('|')[1]), 
								float.Parse(settingValue.Split('|')[2]));
							break;
						case "LastRotation":
							LastRotation = new Quaternion(
								float.Parse(settingValue.Split('|')[0]), 
								float.Parse(settingValue.Split('|')[1]), 
								float.Parse(settingValue.Split('|')[2]), 
								float.Parse(settingValue.Split('|')[3]));
							break;
						case "GlobalScore":
							GlobalScore = long.Parse(settingValue);
							break;
						case "CreditCount":
							CreditCount = int.Parse(settingValue);
							break;
						case "MaxLife":
							MaxLife = float.Parse(settingValue);
							break;
						case "CurrentLife":
							CurrentLife = float.Parse(settingValue);
							break;
						case "LevelCredits":
							LevelCredits = new Dictionary<string, long>();
							if(!string.IsNullOrEmpty(settingValue)) {
								string[] creditData = settingValue.Split ('|');
								foreach(string creditSetting in creditData) {
									string levelName = creditSetting.Split('-')[0];
									long levelCreditCount = long.Parse(creditSetting.Split('-')[1]);
									LevelCredits[levelName] = levelCreditCount;
								}
							}
							break;
						case "LevelBugs":
							LevelBugs = new Dictionary<string, long>();
							if(!string.IsNullOrEmpty(settingValue)) {
								string[] bugData = settingValue.Split ('|');
								foreach(string bugSetting in bugData) {
									string levelName = bugSetting.Split('-')[0];
									long levelBugCount = long.Parse(bugSetting.Split('-')[1]);
									LevelBugs[levelName] = levelBugCount;
								}
							}
							break;
						case "Skills":
							Skills = new List<NonBasicCharacterSkill>();
							if(!string.IsNullOrEmpty(settingValue))
							{
								string[] skillStrings = settingValue.Split('|');
								foreach(string skillName in skillStrings) {
									NonBasicCharacterSkill skill = (NonBasicCharacterSkill)Enum.Parse (typeof(NonBasicCharacterSkill), skillName);
									Skills.Add(skill);
								}
							}
							break;
						default:
							LevelScores = new Dictionary<string, long>();
							LevelScores[settingName] = long.Parse(settingValue);
							break;
					}
				}
			}
			
		}
	}
	
	/// <summary>
	/// Initializes the three player profiles.
	/// </summary>
	public static void InitProfiles() {
		PlayerPrefs.SetString("Profiles", "1,2,3");	
		PlayerPrefs.Save();
	}
	
	/// <summary>
	/// Gets the profiles.
	/// </summary>
	/// <returns>
	/// A list of strings that contain the profile names (1,2,3)
	/// </returns>
	public static List<string> GetProfiles() {
		if(!PlayerPrefs.HasKey("Profiles")) {
			InitProfiles();	
		}
		
		List<string> result = new List<string>();
		result.AddRange(PlayerPrefs.GetString("Profiles").Split(','));
		
		return result;
	}
	
	/// <summary>
	/// Saves a profile and sets it as the current one.
	/// </summary>
	/// <param name='name'>
	/// Profile name, should be "1", "2" or "3"
	/// </param>
	/// <param name='position'>
	/// Position of the profile, 0, 1, 2
	/// </param>
	public static void SetProfile(string name, int position) {
		List<string> profiles = GetProfiles();
		
		string previousName = profiles[position];
		if(!string.IsNullOrEmpty(previousName)){
			if(PlayerPrefs.HasKey(previousName)) {
				PlayerPrefs.DeleteKey(previousName);
			}
		}
		
		profiles[position] = name;
		
		string profileString = GetProfileString(profiles);
		
		PlayerPrefs.SetString("Profiles", profileString);
		PlayerPrefs.SetString(name, string.Empty);
		CurrentProfileName = name;
		PlayerPrefs.Save();
	}
	
	/// <summary>
	/// Sets the current level name.
	/// </summary>
	/// <param name='levelName'>
	/// Level name.
	/// </param>
	public static void SetCurrentLevel(string levelName) {
		CurrentLevelName = levelName;	
	}
	
	/// <summary>
	/// Sets the current profile as the last one used, for the next time it's played.
	/// </summary>
	public static void SetCurrentAsLastProfile() {
		PlayerPrefs.SetString("LastProfile", CurrentProfileName);
		PlayerPrefs.Save();
	}
	
	/// <summary>
	/// Gets the last profile used.
	/// </summary>
	/// <returns>
	/// The last profile used.
	/// </returns>
	public static string GetLastProfile() {
		if(!PlayerPrefs.HasKey("LastProfile")) {
			InitProfiles();
			SetProfile("1",0);
			SetCurrentAsLastProfile();
		}
		CurrentProfileName = PlayerPrefs.GetString("LastProfile");
		return CurrentProfileName;
	}
	
	/// <summary>
	/// Selects a profile.
	/// </summary>
	/// <returns>
	/// The profile.
	/// </returns>
	/// <param name='index'>
	/// Index of the profile to select: 0, 1, 2.
	/// </param>
	public static string SelectProfile(int index) {
		List<string> profiles = GetProfiles();
		CurrentProfileName = profiles[index];		
		return CurrentProfileName;
	}
	
	private static string GetProfileString(List<string> profiles) {
		string profileString = string.Empty;
		profiles.ForEach((profile) => {profileString += (profileString.Length > 0 ? "," : string.Empty) + profile; } );
		
		return profileString;
	}
	
	#endregion
	
	#region State Change
	
	public delegate void StateChangeHandler(States previousState, States newState);
	public static event StateChangeHandler OnStateChange;
	
	public static void SetState(States newState) {
		if(newState != CurrentState) {
			//print(newState);
			oldState = CurrentState;
			CurrentState = newState;

			
			if(OnStateChange != null) {
				OnStateChange(oldState, newState);	
			}
		}
	}

	public static States GetOldState() {
		return oldState ;
	}
	
	#endregion
	
	#region Credits Methods
	
	public delegate void CreditEventHandler(int creditAmount);
	public static event CreditEventHandler OnCreditChange;
	
	public static void AddCredits(int amount) {
		CurrentCreditCount += amount;
		
		for(int i=0; i<amount; i++) {
			EventContext.AddEvent("Credit");
		}
		
		if(OnCreditChange != null) {
			OnCreditChange(amount);
		}
	}
	
	public static void ApplyCreditCount() {
		if(LevelCredits == null) {
			LevelCredits = new Dictionary<string, long>();	
		}
		
		//Adds the level credits only if not already added
		if(!LevelCredits.ContainsKey(CurrentLevelName)) {
			
			//Updates global score
			CreditCount += CurrentCreditCount;
			LevelCredits[CurrentLevelName] = CurrentCreditCount;
		}
		
		ResetLevelCredits();
	}
	public static void ResetLevelCredits() {
		CurrentCreditCount = 0;
	}
	
	
	#endregion
	
	#region Skills Methods
	
	/// <summary>
	/// Agrega o quita un skill de la lista
	/// </summary>
	/// <param name='skill'>
	/// El skill por habilitar/deshabilitar
	/// </param>
	/// <param name='skillEnabled'>
	/// True si se quiere habilitar el skill, falso si no.
	/// </param>
	public static void SetSkill(NonBasicCharacterSkill skill, bool skillEnabled) {
		if(Skills == null) {
			Skills = new List<NonBasicCharacterSkill>();	
		}
		
		if(Skills.Contains (skill)) {
			if(!skillEnabled) {
				Skills.Remove (skill);	
			}
		} else if(skillEnabled) {
			Skills.Add(skill);
		}
	}
	
	/// <summary>
	/// Devuelve el estado de un skill
	/// </summary>
	/// <returns>
	/// Verdadero si el skill existe, falso si no.
	/// </returns>
	/// <param name='skill'>
	/// El skill por determinar
	/// </param>
	public static bool GetSkill(NonBasicCharacterSkill skill) {
		return (Skills != null ? (Skills.Contains (skill) ? true : false) : false);
	}
	
	#endregion
	
	#region Bugs Methods

	public delegate void BugEventHandler(int bugCount);
	public static event BugEventHandler OnBugCountChange;
	
	public static void AddBugs(int amount) {
		CurrentBugCount += amount;
		
		for(int i=0; i<amount; i++) {
			EventContext.AddEvent("Bug");
		}
		
		if(OnBugCountChange != null) {
			OnBugCountChange(amount);
		}
	}
	public static void ApplyBugCount() {
		if(LevelBugs == null) {
			LevelBugs = new Dictionary<string, long>();	
		}
		
		//Adds the level credits only if not already added
		if(!LevelBugs.ContainsKey(CurrentLevelName)) {
			
			//Updates global score
			BugCount += CurrentBugCount;
			LevelBugs[CurrentLevelName] = CurrentBugCount;
		}
		
		ResetLevelBugs();
	}
	public static void ResetLevelBugs() {
		CurrentBugCount = 0;
	}
	
	#endregion
	
}
