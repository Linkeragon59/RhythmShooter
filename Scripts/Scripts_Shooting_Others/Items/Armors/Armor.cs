// abstract class from which all the armors heritates (no use in the current state, but can be usefull if all armors
// have a common property that weapons don't have)

using UnityEngine;
using System.Collections;

abstract public class Armor : Item {

	public int TakenDamage(){
		return damage;
	}

	public void TakeDamage(int damage){
		this.damage += damage;
	}

	public bool IsBroken(){
		if (breakResistance < 0) {
			return false;
		}
		else {
			return (damage > breakResistance);
		}
	}

}
