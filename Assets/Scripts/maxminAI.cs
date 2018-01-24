using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace AssemblyCSharp
{
	public class minmaxAI : MonoBehaviour {
		private int MAX_DEPTH = 10;
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
		int [][] BoardToIntMatrix(board)
		{
			
		}
		float EvaluatePositionRecursive(int depth, int[][] curBoard, float signFactor)
		{
			if ( depth >= MAX_DEPTH ){
				return EvaluatePositionStatic(curBoard);
			}
			<Move>[] moves = GenerateMoveList(curBoard);
			float posValue = -FLT_MAX;
			foreach (move in moves) do {
					Board newBoard = MakeMove(curBoard, move);
					float newValue = signFactor*EvaluatePositionRecursive(depth+1, newBoard, -signFactor);
					if ( newValue > posValue ) posValue = newValue;
				}
			return signFactor*posValue;
		}

		float MakeMove()
		{
				

		}
		Checkers MovePiece(Board curBoard, move)
		{



		}

		GenerateMoveList
		<check>[][] Board = new <check>[8][8];

	}

}
GenerateMoveList(board)
{
	foreach (move in moves)




		foreach (move in moves)



			foreach (move in moves)

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