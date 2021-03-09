// static class whose attributes can be called from anywhere
// allows to reach important objects (handlers, score, list of current enemies...) from anywhere

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameValuesContainer {

	public static GameValuesContainer container;

	public MainGameCamera mainCamera;

	public Stage currentStage;

	// The main handlers object of the scenes
	public ShooterEventsHandler shooterHandler;
	public RhythmEventsHandler rhythmHandler;
	public AudioHandler audioHandler;
	public MenuWrapper menuWrapper;
	public LabelsLayer labelsLayer;

	// To access all the moving objects of the scene (used to destroy all enemies, or all bullets/lasers when respawning the player...)
	public MovingObjectsContainer movingObjectsContainers;
	public GameObject particlesContainer;
	public GameObject enemiesContainer;
	public GameObject bubblesContainer;

	public float stageDuration;

	// Information values about the shooting game
	public int shootingScore = 0;
	public int lives = 3;
	public Armor[] possibleArmors;
	public int currentArmor = -1;
	public Weapon[] possibleWeapons;
	public int currentWeapon = -1;
	public int spawnedEnemies = 0;
	public int killedEnemies = 0;

	// Information values about the rhythm game
	public int combo = 0; // current combo
	public int maxCombo = 0; // biggest combo the player has done in the play
	public int[] rhythmScores = new int[6] {0, 0, 0, 0, 0, 0}; // number of Miss, Bad, Almost, Good, Great and Fantastic respectively

}
