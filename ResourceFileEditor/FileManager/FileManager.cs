/*
===========================================================================

BFG Resource File Manager GPL Source Code
Copyright (C) 2021 George Kalampokis

This file is part of the BFG Resource File Manager GPL Source Code ("BFG Resource File Manager Source Code").

BFG Resource File Manager Source Code is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

BFG Resource File Manager Source Code is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with BFG Resource File Manager Source Code.  If not, see <http://www.gnu.org/licenses/>.

===========================================================================
*/
using System;
using System.IO;

namespace ResourceFileEditor.FileManager;

public sealed class FileManager
{
	public static ushort ReadUint16(Stream stream, int pos)
	{
		byte[] buffer = new byte[2];
		stream.Position = pos;
		_ = stream.Read(buffer, 0, buffer.Length);
		return BitConverter.ToUInt16(buffer, 0);
	}

	public static ushort ReadUint16Swapped(Stream stream, int pos)
	{
		byte[] buffer = new byte[2];
		stream.Position = pos;
		_ = stream.Read(buffer, 0, buffer.Length);
		Array.Reverse(buffer);
		return BitConverter.ToUInt16(buffer, 0);
	}

	public static uint ReadUint32(Stream stream, int pos)
	{
		byte[] buffer = new byte[4];
		stream.Position = pos;
		_ = stream.Read(buffer, 0, buffer.Length);
		return BitConverter.ToUInt32(buffer, 0);
	}

	public static uint ReadUint32Swapped(Stream stream, int pos)
	{
		byte[] buffer = new byte[4];
		stream.Position = pos;
		_ = stream.Read(buffer, 0, buffer.Length);
		Array.Reverse(buffer);
		return BitConverter.ToUInt32(buffer, 0);
	}

	public static int ReadInt32(Stream stream, int pos)
	{
		byte[] buffer = new byte[4];
		stream.Position = pos;
		_ = stream.Read(buffer, 0, buffer.Length);
		return BitConverter.ToInt32(buffer, 0);
	}

	public static int ReadInt32Swapped(Stream stream, int pos)
	{
		byte[] buffer = new byte[4];
		stream.Position = pos;
		_ = stream.Read(buffer, 0, buffer.Length);
		Array.Reverse(buffer);
		return BitConverter.ToInt32(buffer, 0);
	}

	public static ulong ReadUint64(Stream stream, int pos)
	{
		byte[] buffer = new byte[8];
		stream.Position = pos;
		_ = stream.Read(buffer, 0, buffer.Length);
		return BitConverter.ToUInt64(buffer, 0);
	}

	public static ulong ReadUint64Swapped(Stream stream, int pos)
	{
		byte[] buffer = new byte[8];
		stream.Position = pos;
		_ = stream.Read(buffer, 0, buffer.Length);
		Array.Reverse(buffer);
		return BitConverter.ToUInt64(buffer, 0);
	}

	public static byte[] ReadByteArray(Stream stream, int pos, int size)
	{
		byte[] buffer = new byte[size];
		stream.Position = pos;
		_ = stream.Read(buffer, 0, buffer.Length);
		return buffer;
	}

	public static void WriteUint32(Stream stream, int pos, uint value)
	{
		byte[] buffer = BitConverter.GetBytes(value);
		stream.Position = pos;
		stream.Write(buffer, 0, buffer.Length);
	}

	public static void WriteUint32Swapped(Stream stream, int pos, uint value)
	{
		byte[] buffer = BitConverter.GetBytes(value);
		Array.Reverse(buffer);
		stream.Position = pos;
		stream.Write(buffer, 0, buffer.Length);
	}

	public static void WriteUint64Swapped(Stream stream, int pos, ulong value)
	{
		byte[] buffer = BitConverter.GetBytes(value);
		Array.Reverse(buffer);
		stream.Position = pos;
		stream.Write(buffer, 0, buffer.Length);
	}

	public static void WriteByteArray(Stream stream, int pos, byte[] data)
	{
		stream.Position = pos;
		stream.Write(data, 0, data.Length);
	}
}
