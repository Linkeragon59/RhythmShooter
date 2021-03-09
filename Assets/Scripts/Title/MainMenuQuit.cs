// definition of the MenuChoice abstract class (MenuChoice.cs)
// used for the item of the Main Menu of the Title scene to quit the application

using UnityEngine;
using System.Collections;

public class MainMenuQuit : MenuChoice {

	override protected void Effect(){
		Application.Quit ();
	}
}
