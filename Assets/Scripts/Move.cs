using System;

namespace AssemblyCSharp
{
	public class Move
	{

		 minmaxAI.predict[] data;

		// Using a method
		public void ImportData(minmaxAI source)
		{
			this.data = source.ExportData();
		}

		public struct position{
			int x;
			int y;
			public minmaxAI.predict z;
		}
		private position start;
		private position end;
		public Move (int startX, int startY, int endX, int endY, minmaxAI.predict z, minmaxAI.predict a)
		{
			start = new position (startX, startY, z);
			end = new position (endX, endX, a);
		}
		public Move (position start, position end)
		{
			this.start = start;
			this.end = end;
		}
		public position getStart()
		{
			return start;
		}
		public position getEnd()
		{
			return end;
		}

		public minmaxAI.predict[][] makingMoves(Move x, minmaxAI.predict[][] old)
		{

			minmaxAI.predict[][] temp = old;
			end.z = start.z;
			minmaxAI.clearPosition (x.getStart().z);

		}
	}
}

