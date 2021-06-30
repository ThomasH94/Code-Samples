using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Exception
{
	/// <summary>
    /// This classes sole responsibility is to raise game events on an object
    /// </summary>
	public class EventInvoker : MonoBehaviour
	{
		public GameEvent EventToRaise;

		public void Raise()
		{
			EventToRaise.Raise();
		}
	}
}