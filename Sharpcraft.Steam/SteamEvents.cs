﻿/*
 * SteamEvents.cs
 * 
 * Copyright © 2011-2012 by Sijmen Schoon and Adam Hellberg.
 * 
 * This file is part of Sharpcraft.
 * 
 * Sharpcraft is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * Sharpcraft is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with Sharpcraft.  If not, see <http://www.gnu.org/licenses/>.
 * 
 * Disclaimer: Sharpcraft is in no way affiliated with Mojang AB and/or
 * any of its employees and/or licensors.
 * Sijmen Schoon and Adam Hellberg do not take responsibility for
 * any harm caused, direct or indirect, to your Minecraft account
 * via the use of Sharpcraft.
 * 
 * "Minecraft" is a trademark of Mojang AB.
 */

using System;
using System.Collections.Generic;

namespace Sharpcraft.Steam
{
	/// <summary>
	/// <c>SteamFriendsEventArgs</c> class used to communicate friend updates to listeners.
	/// </summary>
	public class SteamFriendsEventArgs : EventArgs
	{
		/// <summary>
		/// The name of the currently logged in user.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// String representation of the current user's status.
		/// </summary>
		public string Status { get; private set; }

		/// <summary>
		/// List of the user's friends.
		/// </summary>
		public List<SteamFriend> Friends { get; private set; }
		/// <summary>
		/// Total number of friends.
		/// </summary>
		public int FriendCount { get; private set; }
		/// <summary>
		/// Number of friends who are currently online.
		/// </summary>
		public int FriendOnlineCount { get; private set; }

		/// <summary>
		/// Initialize a new instance of <c>SteamFriendsEventArgs</c>.
		/// </summary>
		/// <param name="name">Current name of logged in user.</param>
		/// <param name="status">String representation of the user's status.</param>
		/// <param name="friends">List of user's friends.</param>
		/// <param name="count">Total number of friends.</param>
		/// <param name="onlineCount">Number of friends who are currently online.</param>
		internal SteamFriendsEventArgs(string name, string status, List<SteamFriend> friends, int count, int onlineCount)
		{
			Name = name;
			Status = status;
			Friends = friends;
			FriendCount = count;
			FriendOnlineCount = onlineCount;
		}
	}

	/// <summary>
	/// <c>SteamMinecraftDataEventArgs</c> class used to send updated Minecraft Data to event listeners.
	/// </summary>
	public class SteamMinecraftDataEventArgs : EventArgs
	{
		/// <summary>
		/// Name of the server.
		/// </summary>
		public string ServerName { get; private set; }

		/// <summary>
		/// Address of the server.
		/// </summary>
		public string ServerAddress { get; private set; }

		/// <summary>
		/// Port of the server.
		/// </summary>
		public short ServerPort { get; private set; }

		/// <summary>
		/// Current number of players on the server.
		/// </summary>
		public int Players { get; private set; }

		/// <summary>
		/// Max number of players the server can hold.
		/// </summary>
		public int MaxPlayers { get; private set; }

		/// <summary>
		/// Initialze a new instance of <see cref="SteamMinecraftDataEventArgs" />.
		/// </summary>
		/// <param name="name">Name of the server.</param>
		/// <param name="address">Address of the server.</param>
		/// <param name="port">Port of the server.</param>
		/// <param name="players">Current number of players on the server.</param>
		/// <param name="maxPlayers">Max number of players the server can hold.</param>
		public SteamMinecraftDataEventArgs(string name, string address, short port, int players, int maxPlayers)
		{
			ServerName = name;
			ServerAddress = address;
			ServerPort = port;
			Players = players;
			MaxPlayers = maxPlayers;
		}
	}

	/// <summary>
	/// SteamClose event handler.
	/// </summary>
	public delegate void SteamCloseEventHandler();

	/// <summary>
	/// SteamFriends event handler.
	/// </summary>
	/// <param name="e"><see cref="SteamFriendsEventArgs" /> associated with the event.</param>
	public delegate void SteamFriendsEventHandler(SteamFriendsEventArgs e);

	/// <summary>
	/// SteamMinecraftData event handler.
	/// </summary>
	/// <param name="e"><see cref="SteamMinecraftDataEventArgs" /> associated with the event.</param>
	public delegate void SteamMinecraftDataEventHandler(SteamMinecraftDataEventArgs e);
}
