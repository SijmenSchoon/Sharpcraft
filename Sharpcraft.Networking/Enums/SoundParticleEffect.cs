﻿/*
 * SoundParticleEffect.cs
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

using Sharpcraft.Networking.Packets;

namespace Sharpcraft.Networking.Enums
{
	/// <summary>
	/// Different sound/particle effects sent by <see cref="SoundParticleEffectPacket" />.
	/// </summary>
	/// <remarks>http://wiki.vg/Protocol#Effects_2</remarks>
	public enum SoundParticleEffect
	{
		Click2			= 1000,
		Click1			= 1001,
		BowFire			= 1002,
		DoorToggle		= 1003,
		Extinguish		= 1004,

		/// <summary>
		/// Sound of a record playing.
		/// </summary>
		/// <remarks>Data property will contain Record ID.</remarks>
		RecordPlay		= 1005,

		/// <summary>
		/// Sound of Ghast firing.
		/// </summary>
		Charge			= 1007,

		/// <summary>
		/// Sound of Blaze firing.
		/// </summary>
		FireballA		= 1008,

		/// <summary>
		/// Sound of Blaze firing.
		/// </summary>
		FireballB		= 1009,

		/// <summary>
		/// Smoke effect.
		/// </summary>
		/// <remarks>Data property contains direction of smoke.</remarks>
		Smoke			= 2000,

		/// <summary>
		/// Sound/Particle effect of block breaking.
		/// </summary>
		/// <remarks>Data property contains Block ID broken.</remarks>
		BlockBreak		= 2001,

		/// <summary>
		/// Particle animation and sound of using potion.
		/// </summary>
		/// <remarks>Data property contains Potion ID.</remarks>
		SplashPotion	= 2002,

		/// <summary>
		/// Particle animation of portal.
		/// </summary>
		Portal			= 2003,

		/// <summary>
		/// Particle animation of Blaze(?) (fire + smoke).
		/// </summary>
		Blaze			= 2004
	}
}
