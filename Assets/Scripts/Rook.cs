using UnityEngine;
using System.Collections;

public class Rook : Chessman {
		public override bool[,] PossibleMove(){
		bool[,] r = new bool[8,8];
		Chessman c;
		int i;

		//R
		i = CurrentX;
		while (true) 
		{
			i++;
			if (i >= 8)
				break;
			c = BoardManager.Instance.Chessmans [i, CurrentY];
			if (c == null)
				r [i, CurrentY] = true;
			else {
				
				if (c.isWhite != isWhite)
					r [i, CurrentY] = true;
				
				break;
			}
			
		}

		//L
		i = CurrentX;
		while(true){
			i--;
			if (i < 0)
				break;
			c = BoardManager.Instance.Chessmans [i, CurrentY];
			if (c == null)
				r [i, CurrentY] = true;
			else {

				if (c.isWhite != isWhite)
					r [i, CurrentY] = true;

				break;
			}

		}
			
		//U
		i = CurrentY;
		while(true)
		{
			i++;
			if (i >= 8)
				break;
			c = BoardManager.Instance.Chessmans [CurrentX, i];
			if (c == null)
				r [CurrentX,i] = true;
			else {

				if (c.isWhite != isWhite)
					r [CurrentX,i] = true;

				break;
			}

		}

		//D
		i = CurrentY;
		while(true)
		{
			i--;
			if (i < 0)
				break;
			c = BoardManager.Instance.Chessmans [CurrentX, i];
			if (c == null)
				r [CurrentX,i] = true;
			else {

				if (c.isWhite != isWhite)
					r [CurrentX,i] = true;

				break;
			}

		}

		return r;
	}
}
