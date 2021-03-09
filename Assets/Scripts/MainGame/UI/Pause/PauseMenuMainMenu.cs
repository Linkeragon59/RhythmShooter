// definition of the MenuChoice abstract class (MenuChoice.cs)
// used for the item of the pauseMenu of the Main game to go back to the main menu (title)

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenuMainMenu : MenuChoice {

	override protected void Effect(){
		SceneManager.LoadSceneAsync ("Title");
	}
}
