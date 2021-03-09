// definition of the MenuChoice abstract class (MenuChoice.cs)
// used for the item of the pauseMenu of the Main game to quit the application

using UnityEngine;
using System.Collections;

public class PauseMenuQuit : MenuChoice {

	override protected void Effect(){
		Application.Quit();
	}
}
