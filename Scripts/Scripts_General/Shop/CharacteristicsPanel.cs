// attached to the panel where the characteristics of items is displayed
// necessary to adapt the size and position of the sub Panel objects containing the characteristics

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacteristicsPanel : MonoBehaviour {

	public Characteristic charactericticSubPanel; // prefab used to instantiate sub Panel, see Characteristic.cs

	public struct ItemData{
		public ItemData(string label, int value, int maxValue, bool reverse){
			this.label = label;
			this.value = value;
			this.maxValue = maxValue;
			this.reverse = reverse;
		}
		public string label;
		public int value;
		public int maxValue;
		public bool reverse;
	}

	public void CreateContent (Item item){

		// Create sub Panel from the charactericticSubPanel Prefab and fill them with the characteristics of the Item
		List< ItemData > listCharacteristics = new List< ItemData >();
		ItemData level = new ItemData("Level : ",item.GetComponent<Item> ().level,3,false);
		listCharacteristics.Add (level);
		ItemData combo = new ItemData("Threshold : ",item.GetComponent<Item> ().comboThreshold,100,true);
		listCharacteristics.Add (combo);
		if(item.GetComponent<Armor>()!=null){
			ItemData resistance = new ItemData("Resistance : ",item.GetComponent<Item> ().breakResistance,4,false);
			listCharacteristics.Add (resistance);
		}
		else if(item.GetComponent<Weapon>()!=null){
			ItemData resistance = new ItemData("Resistance : ",item.GetComponent<Item> ().breakResistance,10,false);
			listCharacteristics.Add (resistance);
		}

		// Position and size the sub Panels

		RectTransform transform = GetComponent<RectTransform> ();
		float height = transform.rect.height;
		float step = height/listCharacteristics.Count;
		Vector2 offset = new Vector2(transform.rect.center.x,transform.rect.center.y);
		for (int i=0;i<listCharacteristics.Count;i++) {
			Characteristic sub = (Characteristic)Instantiate(charactericticSubPanel);
			sub.transform.SetParent (transform);
			Vector2 sizeSub = new Vector2(transform.rect.width,step);
			sub.GetComponent<RectTransform> ().sizeDelta = sizeSub;
			sub.GetComponent<RectTransform> ().localScale = Vector3.one;
			sub.GetComponent<RectTransform> ().localRotation = Quaternion.Euler(Vector3.zero);
			Vector2 positionPanel = new Vector2 (0, (height-step) / 2 - i*step) + offset;
			sub.GetComponent<RectTransform> ().localPosition = positionPanel;
			sub.UpdateContent (listCharacteristics[i].label, listCharacteristics[i].value, listCharacteristics[i].maxValue, listCharacteristics[i].reverse);
		}
	}

	public void Clear(){ // Clear the panel
		foreach (Transform child in this.transform) {
			Destroy(child.gameObject);
		}
	}
}
