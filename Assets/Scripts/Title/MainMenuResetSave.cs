using UnityEngine;
using System.Collections;

public class MainMenuResetSave : MenuChoice {

	override protected void Effect(){
		SaveLoad.Reset ();
	}
}
