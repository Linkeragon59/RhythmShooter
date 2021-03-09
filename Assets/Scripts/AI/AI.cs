// AI that control the ship of the shooting game.
// It is a neural network-based AI. It is trained during the game, when the player move the ship.
// It becomes able to aim automatically at enemies, however it cannot see the enemies' bullets/laser, so it can't avoid it yet (to do).
// The AI is activated/unactivated by pressing 'o' key during the game.

// The factors matrixes, hyper-parameters, and training set of the Neural Network are saved in the Game class (see Game.cs)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour {

	public GameObject boardShooter;

	private PlayerShip player;
	private Transform enemies;

	void Start(){ // Game objects that are needed for calculation
		player = GameValuesContainer.container.shooterHandler.playerShip;
		enemies = GameValuesContainer.container.movingObjectsContainers.enemiesContainer.transform;
	}

	void Update () {
		Matrix inLayer = GetEnemiesPositions ().ToLine (); // What the AI can 'see' (enemies on the field)

		AddToTrainSet(inLayer); // The current situation may become an example of the training set for machine learning
		SaveLoad.Save ();

		// We apply the neural network (1 hidden layer of size 10, logistic gate at hidden layer and out layer)
		Matrix hiddenLayer = inLayer.Multiply (Game.current.AIMatrix1).ToLogistic ();
		Matrix outLayer = hiddenLayer.Multiply (Game.current.AIMatrix2).ToLogistic ();

		// May move automatically (if AI is activated) whith the result of the Neural Network calculation
		player.AutoMove (outLayer.Get (0,0), outLayer.Get (0,1));

		// Train 1 epoch of the Neural Network with the current examples of the train set
		Train (1);
	}

	void AddToTrainSet(Matrix inLayer){
		int rand = (int)Random.Range(0, 20); // 5% of chance to add the current situation (enemies' position and player's action) to the train set.
		if (enemies.childCount > 0 && (Input.GetAxis ("Vertical")!=0 || Input.GetAxis ("Horizontal")!=0) && rand == 0) {
			Matrix move = new Matrix (1, 2);
			move.Set (0, 0, Input.GetAxis ("Vertical"));
			move.Set (0, 1, Input.GetAxis ("Horizontal"));
			Pair<Matrix,Matrix> newSample = new Pair<Matrix,Matrix>(inLayer, move);
			if (Game.current.trainSet.Count >= Game.current.AIMaxSamples) {
				int index = (int)Random.Range (1, Game.current.AIMaxSamples);
				Game.current.trainSet [index] = newSample;
			} else {
				Game.current.trainSet.Add (newSample);
			}
			Debug.Log ("Size of the traing set : "+Game.current.trainSet.Count); // Display the number of examples in the training set (developper version)
		}
	}

	void Train(int nbIteration){ // Machine Learning
		for (int i = 0; i < nbIteration; i++) {
			for (int j=0;j<Game.current.trainSet.Count;j++){
				TrainOneStep (Game.current.trainSet [j]);
			}
		}
	}

	void TrainOneStep(Pair<Matrix,Matrix> sample){ // Gradient descent
		float change;
		Matrix hiddenLayer = sample.First.Multiply (Game.current.AIMatrix1).ToLogistic ();
		Matrix outLayer = hiddenLayer.Multiply (Game.current.AIMatrix2).ToLogistic ();
		for (int i = 0; i < Game.current.AIMatrix2.xSize; i++) {
			for (int j = 0; j < Game.current.AIMatrix2.ySize; j++) {
				change = Game.current.AIStep * 0.5f * (outLayer.Get (0, j) - sample.Second.Get (0, j)) * (1 + outLayer.Get (0, j)) * (1 - outLayer.Get (0, j)) * hiddenLayer.Get (0, i);
				Game.current.AIMatrix2.Set (i, j, Game.current.AIMatrix2.Get (i, j) - change);
			}
		}
		for (int k = 0; k < Game.current.AIMatrix1.xSize; k++) {
			for (int i = 0; i < Game.current.AIMatrix1.ySize; i++) {
				change = 0;
				for (int j = 0; j < Game.current.AIMatrix2.ySize; j++) {
					change += (outLayer.Get (0, j) - sample.Second.Get (0, j)) * (1 + outLayer.Get (0, j)) * (1 - outLayer.Get (0, j)) * Game.current.AIMatrix2.Get (i, j);
				}
				change = Game.current.AIStep * 0.25f * change * (1 + hiddenLayer.Get (0, i)) * (1 - hiddenLayer.Get (0, i)) * sample.First.Get (0, k);
				Game.current.AIMatrix1.Set (k, i, Game.current.AIMatrix1.Get (k, i) - change);
			}
		}
	}

	Matrix GetPlayerPosition(){ // Not used, give the player position (could be used as information by the AI)
		Matrix playerPosition = new Matrix (1, 2);
		Vector3 fieldSize = boardShooter.GetComponent<Renderer> ().bounds.size;
		Vector3 distance = boardShooter.transform.position - player.transform.position;
		playerPosition.Set (0, 0, -distance.x / fieldSize.x + 0.5f);
		playerPosition.Set (0, 1, distance.z / fieldSize.z + 0.5f);
		return playerPosition;
	}

	Matrix GetEnemiesPositions(){ // Gives enemies' position on the field, this is what the AI can 'see'
		int precision = Game.current.AIPrecision;
		Matrix enemiesPositions = new Matrix(precision,precision);
		Vector3 fieldSize = boardShooter.GetComponent<Renderer> ().bounds.size;
		foreach (Transform child in enemies) {
			Vector3 distance = child.transform.position - player.transform.position;
			int x = precision - 1 - (int)Mathf.Floor((distance.x + fieldSize.x) / (2 * fieldSize.x / precision));
			int y = precision - 1 - (int)Mathf.Floor((distance.z + fieldSize.z) / (2 * fieldSize.z / precision));
			x = Mathf.Max (0, Mathf.Min (precision-1,x));
			y = Mathf.Max (0, Mathf.Min (precision-1,y));
			enemiesPositions.Set (x,y,enemiesPositions.Get(x,y)+1);
		}
		return enemiesPositions;
	}
}
