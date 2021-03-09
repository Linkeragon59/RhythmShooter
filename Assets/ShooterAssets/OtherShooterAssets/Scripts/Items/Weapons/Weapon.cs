// abstract class from which all the weapons heritates

using UnityEngine;
using System.Collections;

abstract public class Weapon : Item {

	abstract public void Initialize ();

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
