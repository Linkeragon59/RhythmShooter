// My own implementation of the Matrix type in C#
// Is used for calculation of the Neural Network

using UnityEngine;
using System.Collections;

[System.Serializable]
public class Matrix {

	private float[] values;
	public int xSize;
	public int ySize;

	public Matrix(int x, int y){
		xSize = x;
		ySize = y;
		values = new float[x * y];
		for (int i = 0; i < x * y; i++) {
			values [i] = 0;
		}
	}

	public void Set(int x, int y, float v){
		if (0 <= x && x <= xSize && 0 <= y && y <= ySize) {
			values[ySize*x+y] = v;
		} else {
			Debug.Log ("Error in filling matrix, index out of range");
		}
	}

	public float Get(int x, int y){
		return values[ySize*x+y];
	}

	public Matrix Multiply(Matrix matrix){
		Matrix result;
		if(ySize != matrix.xSize){
			Debug.Log ("Error in dimensions of matrix");
			result = new Matrix (1, 1);
		}
		else{
			result = new Matrix (xSize, matrix.ySize);
			float sum;
			for (int i = 0; i < result.xSize; i++) {
				for (int j = 0; j < result.ySize; j++) {
					sum = 0;
					for (int k = 0; k < ySize; k++) {
						sum += this.Get (i,k) * matrix.Get (k,j);
					}
					result.Set(i,j,sum);
				}
			}
		}
		return result;
	}

	public Matrix Concat(Matrix matrix){
		if (this.xSize == 1 && matrix.xSize == 1) {
			Matrix concat = new Matrix (1, this.ySize + matrix.ySize);
			for (int i = 0; i < this.ySize; i++) {
				concat.Set (0, i, this.Get (0,i));
			}
			for (int i = 0; i < matrix.ySize; i++) {
				concat.Set (0, i+this.ySize, matrix.Get (0,i));
			}
			return concat;
		}
		else {
			Debug.Log ("Cannot concatenate non-line matrix");
			return new Matrix (1, 1);
		}
	}

	public void Randomize(float factor){
		for (int i = 0; i < xSize * ySize; i++) {
			values [i] = Random.Range (-1*factor,factor);
		}
	}

	public Matrix ToLine(){
		Matrix line = new Matrix (1,xSize*ySize);
		for (int i = 0; i < xSize; i++) {
			for (int j = 0; j < ySize; j++) {
				line.Set (0, ySize * i + j, this.Get (i, j));
			}
		}
		return line;
	}

	public Matrix ToLogistic(){
		Matrix log = new Matrix (xSize, ySize);
		for (int i = 0; i < xSize; i++) {
			for (int j = 0; j < ySize; j++) {
				log.Set (i, j, Utility.Logistic(this.Get (i, j)));
			}
		}
		return log;
	}

	public void Print(){
		string line;
		for (int i = 0; i < xSize; i++) {
			line = i+": |";
			for (int j = 0; j < ySize; j++) {
				line+=" "+values[ySize*i+j]+" |";
			}
			Debug.Log (line);
		}
	}
}
