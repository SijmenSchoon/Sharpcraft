﻿/*
 * SteamException.cs
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
 * Sijmen Schoon and Adam Hellberg does not take responsibility for
 * any harm caused, direct or indirect, to your Minecraft account
 * via the use of Sharpcraft.
 * 
 * "Minecraft" is a trademark of Mojang AB.
 */

using System;

namespace Sharpcraft.Steam
{
	/// <summary>
	/// Exception thrown when a Steam component encounters an error.
	/// </summary>
	[Serializable]
	public class SteamException : Exception
	{
		/// <summary>
		/// <see cref="SteamExceptionType" /> representing the type of exception thrown.
		/// </summary>
		public SteamExceptionType Type { get; private set; }

		/// <summary>
		/// Initialize a new instance of <c>SteamException</c>.
		/// </summary>
		/// <param name="message">Message associated with the <c>SteamException</c>.</param>
		/// <param name="type"><see cref="SteamExceptionType" /> representing the type of exception thrown.</param>
		public SteamException(string message, SteamExceptionType type) : base(message)
		{
			Type = type;
		}
	}
}
