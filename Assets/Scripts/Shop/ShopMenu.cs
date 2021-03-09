// Script that defines the functionning of the menu of the Shop
// attached to the panel that contains the menu
// as most of the objects contained in the menu changes in function of the user actions, most of them cannot be placed and/or
// with the graphic interface of Unity, so they are instantiate, positionned and resized in this script, which also uses
// ChoicesPanel.cs, CharacteristicsPanel.cs and DescriptionPanel.cs

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;


public class ShopMenu : MonoBehaviour {

	public ItemsContainer allItems; // Items to be displayed
	public GameObject testShip;

	public ChoicesPanel typePanel; // panel displaying the categories of the items
	public ChoicesPanel itemPanel; // panel displaying the list of the items in the category selected in typePanel
	public GameObject detailsPanel; // panel containing the characteristicsPanel and descriptionPanel, necessary to position these two panels correctly
	public CharacteristicsPanel characteristicsPanel; // panel where the characteristics can be written
	public DescriptionPanel descriptionPanel; // panel where the descriptions can be written

	public AudioClip choiceChange;
	public AudioClip choiceChosen;

	private AudioSource audioSource;

	private Dictionary< string,Item[] > objects; // dictionnary used to classify items by categories

	private int selectedMenu = 0; // 0 if the player is focusing on the typePanel (chosing categories), 1 for itemPanel (choosing items)

	private Item testItem; // Item instance used to show the weapon working on the screen (demonstration) while choosing

	// Used to sort the items in the menus
	private class SorterText : IComparer<Text> {
		int IComparer<Text>.Compare(Text a, Text b){ 
			return String.Compare(a.text,b.text);
		}
	}

	// Used to sort the items in the menus
	private class SorterItem : IComparer<Item> {
		int IComparer<Item>.Compare(Item a, Item b){
			int levelA = a.level;
			int levelB = b.level;
			if (levelA > levelB) {
				return 1;
			}
			else if (levelA < levelB) {
				return -1;
			}
			else {
				return String.Compare(a.name,b.name);
			}
		}
	}

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		objects = new Dictionary<string,Item[]>();
		objects ["Armors"] = allItems.armors;
		objects ["Weapons"] = allItems.weapons;
		foreach (string key in objects.Keys) {
			Array.Sort(objects[key],(IComparer<Item>)new SorterItem());
		}

		typePanel.SetSize(5);
		foreach (string key in objects.Keys) {
			typePanel.CreateChoice (key,key,false,false);
		}
		// Adds a choice to go back to the main menu
		objects ["Main Menu"] = new Item[0];
		typePanel.CreateChoice ("Main Menu","Main Menu",false,false);
		typePanel.Display(0);
		typePanel.SetFocus (true);

		SelectType (typePanel.GetSelectedChoice());
	}

	// Defines the behaviour of the menu when the user select an item or press an arrow or return/enter
	// selection of an item : displays characteristics, description and demonstration item
	// up / down naviguates vertically
	// left / right alternates between the typePanel and the itemPanel (categories and items selection)
	// enter on Main Menu goes back to main menu
	// enter on an item menu equips or desequips the item (ToggleItem function)
	void Update () {
		if (Input.GetKeyDown("down")) {
			if (audioSource != null && choiceChange!=null) {
				audioSource.PlayOneShot (choiceChange);
			}
			if (selectedMenu == 0) {
				SelectType (typePanel.ChangeChoice (1));
				itemPanel.ResetSelection ();
			}
			else if (selectedMenu == 1) {
				itemPanel.ChangeChoice (1);
				DisplayCharacteristics ();
			}
		}
		else if (Input.GetKeyDown("up")) {
			if (audioSource != null && choiceChange!=null) {
				audioSource.PlayOneShot (choiceChange);
			}
			if (selectedMenu == 0) {
				SelectType (typePanel.ChangeChoice (-1));
				itemPanel.ResetSelection ();
			}
			else if (selectedMenu == 1) {
				itemPanel.ChangeChoice (-1);
				DisplayCharacteristics ();
			}
		}
		else if (Input.GetKeyDown("right")) {
			if (audioSource != null && choiceChange!=null) {
				audioSource.PlayOneShot (choiceChange);
			}
			if (itemPanel.HasChoices()) {
				itemPanel.SetFocus (true);
				selectedMenu = 1;
				DisplayCharacteristics ();
			}
		}
		else if (Input.GetKeyDown("left")) {
			if (audioSource != null && choiceChange!=null) {
				audioSource.PlayOneShot (choiceChange);
			}
			itemPanel.SetFocus (false);
			selectedMenu = 0;
			RemoveCharacteristics ();
		}
		if (Input.GetKeyDown("return")) {
			if (audioSource != null && choiceChosen!=null) {
				audioSource.PlayOneShot (choiceChosen);
			}
			string selectedType = typePanel.GetSelectedChoice ();
			if (selectedMenu == 0 && selectedType == "Main Menu") {
				SceneManager.LoadSceneAsync ("Title");
			}
			if (selectedMenu == 1) {
				ToggleItem (selectedType,objects [selectedType] [itemPanel.GetIndexSelectedChoice()]);
				SelectType (selectedType);
			}
		}
	}

	// Update the content of the itemPanel in function of the category choosen in the typePanel
	void SelectType(string type){
		itemPanel.Clear ();
		foreach (Item item in objects[type]) {
			itemPanel.CreateChoice (item.name,item.itemName,true,Game.IsSaved (item.name));
		}
		itemPanel.Display (0);
	}

	// displays the characteristics of the currently selected item
	void DisplayCharacteristics(){
		RemoveCharacteristics ();
		Item selectedObject = objects [typePanel.GetSelectedChoice()] [itemPanel.GetIndexSelectedChoice()];
		Vector3 testItemPosition;
		testItemPosition = testShip.transform.position;
		testItem = (Item)Instantiate(selectedObject,testItemPosition,Quaternion.identity);
		if (testItem.GetComponent<Weapon> () != null) {
			testItem.GetComponent<Weapon> ().Initialize ();
		}

		characteristicsPanel.CreateContent (selectedObject);

		descriptionPanel.SetDescription (selectedObject.GetComponent<Item>().description);
	}
		
	// Empties the characteristicsPanel
	void RemoveCharacteristics(){
		if (testItem != null) {
			Destroy (testItem.gameObject);
		}
		characteristicsPanel.Clear();
		descriptionPanel.Clear();
	}

	// Equips or Desequips an item and save
	void ToggleItem(string type,Item item){
		if (type == "Weapons") {
			if (Game.current.weapons [item.level] == item.name) {
				Game.current.weapons [item.level] = "";
			}
			else {
				Game.current.weapons [item.level] = item.name;
			}
		}
		else if (type == "Armors") {
			if (Game.current.armors [item.level] == item.name) {
				Game.current.armors [item.level] = "";
			}
			else {
				Game.current.armors [item.level] = item.name;
			}
		}
		SaveLoad.Save();
	}
}
