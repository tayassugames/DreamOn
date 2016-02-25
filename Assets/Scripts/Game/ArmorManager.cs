using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmorManager : MonoBehaviour {
	
	public enum ArmorType {
		Helmet = 0,
		Chest = 1,
		KneeL = 2,
		KneeR = 3,
		Hip = 4,
		Andy = 5
	}
	
	public bool HelmetArmor = true;
	public bool ChestArmor = true;
	public bool KneeArmor = true;
	public bool HipArmor = true;
	public bool AndyBackPack = true;
	
	private GameObject _helmetArmor;
	private GameObject _chestArmor;
	private GameObject _kneeArmorR;
	private GameObject _kneeArmorL;
	private GameObject _hipArmor;
	
	private Dictionary<ArmorType ,GameObject> _gameObjects;
	
	// Use this for initialization
	void Start () {
		_gameObjects = new Dictionary<ArmorType, GameObject>();
		
		_gameObjects.Add(ArmorType.Helmet, GameObject.Find("HelmetArmor"));
		_gameObjects.Add(ArmorType.Chest, GameObject.Find("ChestArmor"));
		_gameObjects.Add(ArmorType.KneeR, GameObject.Find("KneeArmorR"));
		_gameObjects.Add(ArmorType.KneeL, GameObject.Find("KneeArmorL"));
		_gameObjects.Add(ArmorType.Hip, GameObject.Find("HipArmor"));
		_gameObjects.Add(ArmorType.Andy, GameObject.Find("AndyBackPack"));
		
		_gameObjects[ArmorType.Helmet].GetComponent<Rigidbody>().isKinematic=false;
		_gameObjects[ArmorType.Chest].GetComponent<Rigidbody>().isKinematic=false;
		_gameObjects[ArmorType.KneeR].GetComponent<Rigidbody>().isKinematic=false;
		_gameObjects[ArmorType.KneeL].GetComponent<Rigidbody>().isKinematic=false;
		_gameObjects[ArmorType.Hip].GetComponent<Rigidbody>().isKinematic=false;
		
		if (AndyBackPack) 
		{ 
			_gameObjects[ArmorType.Andy].GetComponent<Renderer>().enabled = true;
		}
		else 
		{
			_gameObjects[ArmorType.Andy].GetComponent<Renderer>().enabled = false;
		}
		
		if (HelmetArmor) 
		{ 
			_gameObjects[ArmorType.Helmet].GetComponent<Renderer>().enabled = true;
		}
		else 
		{
			_gameObjects[ArmorType.Helmet].GetComponent<Renderer>().enabled = false;
		}
		
		if (ChestArmor) 
		{ 
			_gameObjects[ArmorType.Chest].GetComponent<Renderer>().enabled = true;
		}
		else 
		{
			_gameObjects[ArmorType.Chest].GetComponent<Renderer>().enabled = false;
		}
		
		if (HipArmor) 
		{ 
			_gameObjects[ArmorType.Hip].GetComponent<Renderer>().enabled = true;
		}
		else 
		{
			_gameObjects[ArmorType.Hip].GetComponent<Renderer>().enabled = false;
		}
		
		if (KneeArmor) 
		{ 
			_gameObjects[ArmorType.KneeR].GetComponent<Renderer>().enabled = true;
			_gameObjects[ArmorType.KneeL].GetComponent<Renderer>().enabled = true;
		}
		else 
		{
			_gameObjects[ArmorType.KneeR].GetComponent<Renderer>().enabled = false;
			_gameObjects[ArmorType.KneeL].GetComponent<Renderer>().enabled = false;
		}
		
	}

	/// <summary>
	/// Llamar este metodo para ocultar/mostrar armadura.
	/// No usar update por razones de performance
	/// </summary>
	/// <param name='armorType'>
	/// Armor type.
	/// </param>
	/// <param name='isVisible'>
	/// Is visible.
	/// </param>
	public void SetVisibility(ArmorType armorType, bool isVisible)
	{
		_gameObjects[armorType].GetComponent<Renderer>().enabled = isVisible;
			
		switch(armorType) {
		case ArmorType.Andy:
			AndyBackPack = isVisible;
			break;
		case ArmorType.Helmet:
			HelmetArmor = isVisible;
			break;
		case ArmorType.Chest:
			ChestArmor = isVisible;
			break;
		case ArmorType.Hip:
			HipArmor = isVisible;
			break;
		case ArmorType.KneeR:
		case ArmorType.KneeL:
			KneeArmor = isVisible;
			break;
		default:
			break;
		}
	}
	
	
	
}
