using OpenMacroBoard.SDK;
using OpenMacroBoard.VirtualBoard;
using StreamDeckSharp;
using System.Linq;

namespace MemoRE.App
{
	public static class DeckHelper
	{
		/// <summary>
		/// Searches for a real classic stream deck or creates a virtual one.
		/// All examples are designed for the 5x3 classic StreamDeck.
		/// </summary>
		/// <returns></returns>
		public static IMacroBoard OpenBoard()
		{
			var realDeck = StreamDeck.EnumerateDevices(Hardware.StreamDeck).FirstOrDefault();

			if (!(realDeck is null))
				return realDeck.Open();

			return BoardFactory.SpawnVirtualBoard(Hardware.StreamDeck.Keys);
		}
	}
}
