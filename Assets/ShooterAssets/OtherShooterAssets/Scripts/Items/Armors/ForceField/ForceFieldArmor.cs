// defines the ForceFieldArmor (that heritates from abstract Item class) class
// attached to every ForceField (armor) objects

using UnityEngine;
using System.Collections;

public class ForceFieldArmor : Armor {

	override public bool CanBeSelected(){ // define the condition for the armor to be selected
		return (GameValuesContainer.container.combo >= comboThreshold);
	}
}
