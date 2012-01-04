﻿using System;

namespace Sharpcraft.Protocol
{
	public class Packet
	{
		public byte PacketID;
	}

	public class PacketKeepAlive : Packet
	{
		public Int32 KeepAliveID;
	}

	public class PacketLoginRequestSC : Packet
	{
		public Int32 EntityID;
		public Int64 MapSeed;
		public Int32 Gamemode;
		public sbyte Dimension;
		public sbyte Difficulty;
		public byte WorldHeight;
		public byte MaxPlayers;
	}

	public class PacketLoginRequestCS : Packet
	{
		public Int32 ProtocolVersion;
		public string Username;
	}

	public class PacketHandshakeSC : Packet
	{
		public string ConnectionHash;
	}

	public class PacketHandshakeCS : Packet
	{
		public string Username;
	}

	public class Packet3 : Packet
	{
		public string Message;
	}

	public class Packet4 : Packet
	{
		public Int64 Time;
	}

	public class Packet5 : Packet
	{
		public Int32 EntityID;
		public Int16 Slot;
		public Int16 ItemID;
		public Int16 Damage;
	}

	public class Packet6 : Packet
	{
		public Int32 X;
		public Int32 Y;
		public Int32 Z;
	}
}