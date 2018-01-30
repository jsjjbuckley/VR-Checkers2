using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
namespace AssemblyCSharp
{
	public class minmaxAI : MonoBehaviour {
		private int MAX_DEPTH = 10;
		private bool isRed;

		public struct predict{
			public bool black = false;
			public bool bKing = false;
			public bool rKing = false;
			public bool red = false;
		}
		predict[] data;

		public predict[] ExportData()
		{
			return data;
		}


		public bool isEmpty(predict x){
			if (x.black || x.red) {
				return false;
			} else {
				return true;
			}
		}
		public void clearPosition(predict x)
		{
			x.black = false;
			x.bKing = false;
			x.rKing = false;
			x.red = false;
		}
		//				public int minmax(int[][] board)
		//				{
		//					bestBoard = NULL;
		//				}
		//
		//				public static T DeepClone<T>(T obj)
		//				{
		//					using (var ms = new MemoryStream())
		//					{
		//						var formatter = new BinaryFormatter();
		//						formatter.Serialize(ms, obj);
		//						ms.Position = 0;
		//
		//						return (T) formatter.Deserialize(ms);
		//					}
		//				}

		predict[][] BoardToIntMatrix(Piece[,] board)
		{
			predict[][] twoDArr = new int[8][8];
			for(int i = 0; i < 9; i++)
			{
				for(int j = 0; j <9; j++)
				{
					Piece p = board[i,j];
					if(p.isRed)
					{

						twoDArr[i][j].red = true; //red
						if (p.isKing) {
							twoDArr[i][j].rKing = true; //red king
						}
					}
					else if(p != null)
					{
						twoDArr[i][j].black = true; //black
						if (p.isKing) {
							twoDArr[i][j].bKing = true; //black king
						}
					}
					else
					{
					}

				}
			}
			return twoDArr;
		}
		float minimax(int depth, predict[][] curBoard, float signFactor)
		{
			if ( depth >= MAX_DEPTH ){
				return evaluateBoard(curBoard);
			}
			List<Move> moves = GenerateMoveList(curBoard);
			float posValue = -FLT_MAX;
			foreach (var Move in moves){
					predict[][] newPredictMatrix = Move.makingMoves(curBoard, Move);
					float newValue = signFactor*minimax(depth+1, newPredictMatrix, -signFactor);
					if ( newValue > posValue ) 
					{
						posValue = newValue;
					}
				}
			return signFactor*posValue;
		}
		float evaluateBoard(predict[][] curBoard)
		{
			float score = 0;

			for(int i = 0; i < 9; i++)
			{
				for(int j = 0; j <9; j++)
				{
					if(curBoard[i][j].red)
					{

						if (curBoard[i][j].rKing) {
							score = score + 1.3;
						}
						else{
							score = score + 1;
						}
					}
					else if(curBoard[i][j].black)
					{
						if (curBoard[i][j].bKing) {
							score = score - 1.3;
						}
						else{
							score = score - 1;
						}
					}

				}
			}
			return score;
		}			


//		GenerateMoveList
//		<check>[][] Board = new <check>[8][8];


		public List<Move> GenerateMoveList(predict[][] twoDArr)
		{
			List<Move> allKindsOfMoves = new List<Move>();
			for(int i = 0; i < 9; i++)
			{
				for(int j = 0; j <9; j++)
				{
					if(twoDArr[i][j].red)
					{
						if(twoDArr[i+1][j+1].black)
						{
							if(isEmpty(twoDArr[i+2][j+2]))
							{
								//add jump move
								allKindsOfMoves.Add(i, j, i+2, j+2, twoDArr[i][j], twoDArr[i+2][j+2]);
							}

						}
						else if(isEmpty(twoDArr[i+1][j+1]))
						{
							//add move
							allKindsOfMoves.Add(i, j, i+2, j+2, twoDArr[i][j], twoDArr[i+1][j+1]);

						}

						if(twoDArr[i-1][j+1].black)
						{
							if(isEmpty(twoDArr[i-2][j+2]))
							{
								//add jump move
								allKindsOfMoves.Add(i, j, i-2, j+2, twoDArr[i][j], twoDArr[i-2][j+2]);

							}

						}
						else if(isEmpty(twoDArr[i-1][j+1]))
						{
							//add move
							allKindsOfMoves.Add(i, j, i-1, j+1, twoDArr[i][j], twoDArr[i-1][j+1]);

						}
						//backwards

						if(	twoDArr[i][j].rKing)
						{
							if(twoDArr[i+1][j-1].black)
							{
								if(isEmpty(twoDArr[i+2][j-2]))
								{
									//add jump move
									allKindsOfMoves.Add(i, j, i+2, j-2, twoDArr[i][j], twoDArr[i+2][j-2]);

								}

							}
							else if(isEmpty(twoDArr[i+1][j-1]))
							{
								//add move
								allKindsOfMoves.Add(i, j, i+1, j-1, twoDArr[i][j], twoDArr[i+1][j-1]);

							}


							if(twoDArr[i-1][j-1].black)
							{
								if(isEmpty(twoDArr[i-2][j-2]))
								{
									//add jump move
									allKindsOfMoves.Add(i, j, i-2, j-2, twoDArr[i][j], twoDArr[i-2][j-2]);

								}

							}
							else if(isEmpty(twoDArr[i-1][j-1]))
							{
								//add move
								allKindsOfMoves.Add(i, j, i-1, j-1, twoDArr[i][j], twoDArr[i-1][j-1]);

							}
						}
					}

					if(twoDArr[i][j].black)
					{
						if(twoDArr[i+1][j-1].red)
						{
							if(isEmpty(twoDArr[i+2][j-2]))
							{
								//add jump move
								allKindsOfMoves.Add(i, j, i+2, j-2, twoDArr[i][j], twoDArr[i+2][j-2]);

							}

						}
						else if(isEmpty(twoDArr[i+1][j-1]))
						{
							//add move
							allKindsOfMoves.Add(i, j, i+1, j-1, twoDArr[i][j], twoDArr[i+1][j-1]);

						}


						if(twoDArr[i-1][j-1].red)
						{
							if(isEmpty(twoDArr[i-2][j-2]))
							{
								//add jump move
								allKindsOfMoves.Add(i, j, i-2, j-2, twoDArr[i][j], twoDArr[i-2][j-2]);

							}

						}
						else if(isEmpty(twoDArr[i-1][j-1]))
						{
							//add move
							allKindsOfMoves.Add(i, j, i-1, j-1, twoDArr[i][j], twoDArr[i-1][j-1]);

						}
						//backwards

						if(	twoDArr[i][j].bKing)
						{
							if(twoDArr[i+1][j+1].red)
							{
								if(isEmpty(twoDArr[i+2][j+2]))
								{
									//add jump move
									allKindsOfMoves.Add(i, j, i+2, j+2, twoDArr[i][j], twoDArr[i+2][j+2]);

								}

							}
							else if(isEmpty(twoDArr[i+1][j+1]))
							{
								//add move
								allKindsOfMoves.Add(i, j, i+1, j+1, twoDArr[i][j], twoDArr[i+1][j+1]);

							}


							if(twoDArr[i-1][j+1].red)
							{
								if(isEmpty(twoDArr[i-2][j+2]))
								{
									//add move
									allKindsOfMoves.Add(i, j, i-1, j+1, twoDArr[i][j], twoDArr[i-2][j+2]);

								}

							}
							else if(isEmpty(twoDArr[i-1][j+1]))
							{
								//add move
								allKindsOfMoves.Add(i, j, i-1, j+1, twoDArr[i][j], twoDArr[i-1][j+1]);

							}
						}
					}
				}
			}
			return allKindsOfMoves;
		}





	}
}
////Did we kill anything
////If this is a jump
//if(Mathf.Abs(x2-x1) == 2)
//{
//	Piece p = pieces[(x1 + x2) / 2, (y1 + y2) / 2];
//	if(p != null)
//	{
//		pieces[(x1 + x2) / 2, (y1 + y2) / 2] = null;
//		Destroy(p);
//	}
//}
