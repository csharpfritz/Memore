using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoRE.WebGame.Hubs
{

	public class GameHub : Hub
	{

		public static string DeckConnectionId { get; private set; }

		public override Task OnConnectedAsync()
		{

			if (this.Context.GetHttpContext().Request.Query["deck"] == "1") {

				DeckConnectionId = this.Context.ConnectionId;

			}

			return base.OnConnectedAsync();

		}

	}

}
