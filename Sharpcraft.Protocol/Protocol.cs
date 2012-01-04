﻿/* 
 * Sharpcraft.Protocol
 * Copyright (c) 2012 by Sijmen Schoon and Adam Hellberg.
 * All Rights Reserved.
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Sharpcraft.Protocol
{
	public enum EndianType
	{
		LittleEndian,
		BigEndian
	}

	public class Protocol
	{
		private readonly TcpClient _client = new TcpClient();
		private readonly NetworkStream _stream;
		private readonly NetworkTools _tools;

		public Protocol(string server, int port)
		{
			_client.Connect(server, port);
			_stream = _client.GetStream();
			_tools = new NetworkTools(_stream);
		}


		public byte[] StringToBytes(string str)
		{
			byte[] strLength = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(str.Length));
			List<Byte> bytes = strLength.ToList();

			byte[] bteString = Encoding.BigEndianUnicode.GetBytes(str);
			bytes.AddRange(bteString);

			return bytes.ToArray();
		}

		public string BytesToString(byte[] bytes) {
			byte[] bteStrLength = { bytes[0], bytes[1] };
			int strLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(bteStrLength, 0));

			string str = "";

			for (short s = 1; s < strLength + 1; s++)
			{
				byte[] tmp = { bytes[s * 2], bytes[(s * 2) + 1] };
				str += Encoding.BigEndianUnicode.GetString(tmp);
			}

			return str;
		}


		public Packet GetPacket()
		{
			var packetID = (byte)_stream.ReadByte();
			Packet pack = null;

			if (packetID == 0x00) // Keep alive
			{
				var packet = new PacketKeepAlive {PacketID = 0x00};
				packet.KeepAliveID = _tools.ReadInt32();
				pack = packet;
			}
			else if (packetID == 0x01) // Login Request
			{
				var packet = new PacketLoginRequestSC {PacketID = 0x01};

				packet.EntityID = _tools.ReadInt32();
				_tools.StreamSkip(2);
				packet.MapSeed     = _tools.ReadInt64();
				packet.Gamemode    = _tools.ReadInt32();
				packet.Dimension   = (sbyte)_stream.ReadByte();
				packet.Difficulty  = (sbyte)_stream.ReadByte();
				packet.WorldHeight = (byte)_stream.ReadByte();
				packet.MaxPlayers  = (byte)_stream.ReadByte();

				pack = packet;
			}
			else if (packetID == 0x02)
			{
				var packet = new PacketHandshakeSC {PacketID = 0x02};
				packet.ConnectionHash = _tools.ReadString();
				pack = packet;
			}
			else if (packetID == 0x03)
			{
				var packet = new PacketChatMessage {PacketID = 0x03};
				packet.Message = _tools.ReadString();
				pack = packet;
			}
			else if (packetID == 0x04)
			{
				var packet = new PacketTimeUpdate {PacketID = 0x04};
				packet.Time = _tools.ReadInt32();
				pack = packet;
			}
			else if (packetID == 0x05)
			{
				var packet = new PacketEntityEquipment {PacketID = 0x05};

				packet.EntityID = _tools.ReadInt32();
				packet.Slot     = _tools.ReadInt16();
				packet.ItemID   = _tools.ReadInt16();
				packet.Damage   = _tools.ReadInt16();

				pack = packet;
			}
			else if (packetID == 0x06)
			{
				var packet = new PacketSpawnPosition() {PacketID = 0x06};

				packet.X = _tools.ReadInt32();
				packet.Y = _tools.ReadInt32();
				packet.Z = _tools.ReadInt32();

				pack = packet;
			}

			return pack;
		}

		public void SendPacket(Packet packet)
		{
			byte packetID = packet.PacketID;

			if (packetID == 0x00)
			{
				var pack = (PacketKeepAlive)packet;
				_tools.WriteByte(packetID);
				_tools.WriteInt32(pack.KeepAliveID);
			}
			else if (packetID == 0x01)
			{
				var pack = (PacketLoginRequestCS)packet;
				_tools.WriteByte(packetID);
				_tools.WriteInt32(pack.ProtocolVersion);
				_tools.WriteString(pack.Username);
				_tools.WriteInt64(0);						// Not Used
				_tools.WriteInt32(0);						// Not Used
				_tools.WriteByte(0);						// Not Used
				_tools.WriteByte(0);						// Not Used
				_tools.WriteByte(0);						// Not Used
				_tools.WriteByte(0);						// Not Used
			}
			else if (packetID == 0x02)
			{

			}

			_stream.Flush();
		}
		
		// Packet 0x00
		public void PacketKeepAlive(Int32 id)
		{
			_tools.WriteByte(0x00);			// Packet ID
			_tools.WriteInt32(id);			
		}

		// Packet 0x01
		public void PacketLoginRequest(Int32 version, string username)
		{
			_tools.WriteByte(0x01);			// Packet ID
			_tools.WriteInt32(version);		// Protocol version (22 for 1.0.0)
			_tools.WriteString(username);	// Username
			_tools.WriteInt64(0);			// Not Used
			_tools.WriteInt32(0);			// Not Used
			_tools.WriteByte(0);			// Not Used
			_tools.WriteByte(0);			// Not Used
			_tools.WriteByte(0);			// Not Used
			_tools.WriteByte(0);			// Not Used

			_stream.Flush();
		}

		// Packet 0x02
		public void PacketHandshake(string username)
		{
			_tools.WriteByte(0x02);			// Packet ID
			_tools.WriteString(username);	// Username

			_stream.Flush();
		}

		// Packet 0x03
		public void PacketChatMessage(string message)
		{
			_tools.WriteByte(0x03);			// Packet ID
			_tools.WriteString(message);	// Message

			_stream.Flush();
		}

		// Packet 0x04
		public void PacketEntityEquipment(Int32 entityID, Int16 slot, Int16 itemID, Int16 damage)
		{
			_tools.WriteByte(0x04);			// Packet ID
			_tools.WriteInt32(entityID);	// Entity ID
			_tools.WriteInt16(slot);		// Slot
			_tools.WriteInt16(itemID);		// Item ID
			_tools.WriteInt16(damage);		// Damage

			_stream.Flush();
		}
	}
}
