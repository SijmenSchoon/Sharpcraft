﻿/* 
 * Sharpcraft.Protocol
 * Copyright (c) 2012 by Sijmen Schoon and Adam Hellberg.
 * All Rights Reserved.
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Sharpcraft.Networking
{
	/// <summary>
	/// Provides various networking tools.
	/// </summary>
	internal class NetworkTools
	{
		private readonly NetworkStream _stream;

		public NetworkTools(NetworkStream stream)
		{
			_stream = stream;
		}

		public string ReadString()
		{
			byte[] bteStringLength = {(byte) _stream.ReadByte(), (byte) _stream.ReadByte()};
			short stringLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(bteStringLength, 0));
			var str = new StringBuilder();
			for (short s = 0; s < stringLength; s++)
			{
				byte[] bte = {(byte) _stream.ReadByte(), (byte) _stream.ReadByte()};
				str.Append(Encoding.BigEndianUnicode.GetString(bte));
			}

			return str.ToString();
		}

		public byte ReadByte()
		{
			return (byte) _stream.ReadByte();
		}

		public sbyte ReadSignedByte()
		{
			return (sbyte) _stream.ReadByte();
		}

		public Int16 ReadInt16()
		{
			var bte = new byte[2];
			_stream.Read(bte, 0, bte.Length);
			return IPAddress.NetworkToHostOrder(BitConverter.ToInt16(bte, 0));
		}

		public Int32 ReadInt32()
		{
			var bte = new byte[4];
			_stream.Read(bte, 0, bte.Length);
			return IPAddress.NetworkToHostOrder(BitConverter.ToInt16(bte, 0));
		}

		public Int64 ReadInt64()
		{
			var bte = new byte[8];
			_stream.Read(bte, 0, bte.Length);
			return IPAddress.NetworkToHostOrder(BitConverter.ToInt16(bte, 0));
		}

		public void WriteString(string s)
		{
			_stream.Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)s.Length)), 0, 2);
			byte[] byteString = Encoding.BigEndianUnicode.GetBytes(s);
			_stream.Write(byteString, 0, byteString.Length);
		}

		public void WriteBoolean(bool b)
		{
			_stream.WriteByte(Convert.ToByte(b));
		}

		public void WriteByte(byte i)
		{
			_stream.WriteByte(i);
		}

		public void WriteInt16(Int16 i)
		{
			byte[] bte = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(i));
			_stream.Write(bte, 0, bte.Length);
		}

		public void WriteInt32(Int32 i)
		{
			byte[] bte = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(i));
			_stream.Write(bte, 0, bte.Length);
		}

		public void WriteInt64(Int64 i)
		{
			byte[] bte = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(i));
			_stream.Write(bte, 0, bte.Length);
		}

		public void Skip(int amount = 1)
		{
			for (int i = 0; i < amount; i++)
				_stream.ReadByte();
		}
	}
}
