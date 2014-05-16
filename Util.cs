using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MysteryGiftConvert {
	public static class Util {

		#region SwapEndian
		public static Int16 SwapEndian( this Int16 x ) {
			return (Int16)SwapEndian( (UInt16)x );
		}
		public static UInt16 SwapEndian( this UInt16 x ) {
			return x = (UInt16)
					   ( ( x << 8 ) |
						( x >> 8 ) );
		}

		public static Int32 SwapEndian( this Int32 x ) {
			return (Int32)SwapEndian( (UInt32)x );
		}
		public static UInt32 SwapEndian( this UInt32 x ) {
			return x = ( x << 24 ) |
					  ( ( x << 8 ) & 0x00FF0000 ) |
					  ( ( x >> 8 ) & 0x0000FF00 ) |
					   ( x >> 24 );
		}

		public static Int64 SwapEndian( this Int64 x ) {
			return (Int64)SwapEndian( (UInt64)x );
		}
		public static UInt64 SwapEndian( this UInt64 x ) {
			return x = ( x << 56 ) |
						( ( x << 40 ) & 0x00FF000000000000 ) |
						( ( x << 24 ) & 0x0000FF0000000000 ) |
						( ( x << 8 ) & 0x000000FF00000000 ) |
						( ( x >> 8 ) & 0x00000000FF000000 ) |
						( ( x >> 24 ) & 0x0000000000FF0000 ) |
						( ( x >> 40 ) & 0x000000000000FF00 ) |
						 ( x >> 56 );
		}
		#endregion

		#region NumberUtils
		public static uint ToUInt24( byte[] File, int Pointer ) {
			byte b1 = File[Pointer];
			byte b2 = File[Pointer + 1];
			byte b3 = File[Pointer + 2];

			return (uint)( b3 << 16 | b2 << 8 | b1 );
		}

		public static byte[] GetBytesForUInt24( uint Number ) {
			byte[] b = new byte[3];
			b[0] = (byte)( Number & 0xFF );
			b[1] = (byte)( ( Number >> 8 ) & 0xFF );
			b[2] = (byte)( ( Number >> 16 ) & 0xFF );
			return b;
		}

		/// <summary>
		/// converts a 32-bit int that's actually a byte representation of a float
		/// to an actual float for use in calculations or whatever
		/// </summary>
		public static float UIntToFloat( uint integer ) {
			byte[] b = BitConverter.GetBytes( integer );
			float f = BitConverter.ToSingle( b, 0 );
			return f;
		}

		public static int Align( int Number, int Alignment ) {
			return (int)Align( (uint)Number, (uint)Alignment );
		}
		public static uint Align( uint Number, uint Alignment ) {
			uint diff = Number % Alignment;
			if ( diff == 0 ) {
				return Number;
			} else {
				return ( Number + ( Alignment - diff ) );
			}
		}
		#endregion

		public static void CopyByteArrayPart( IList<byte> from, int locationFrom, IList<byte> to, int locationTo, int count ) {
			for ( int i = 0; i < count; i++ ) {
				to[locationTo + i] = from[locationFrom + i];
			}
		}

		public static void CopyStream( System.IO.Stream input, System.IO.Stream output, int count ) {
			byte[] buffer = new byte[4096];
			int read;

			int bytesLeft = count;
			while ( ( read = input.Read( buffer, 0, Math.Min( buffer.Length, bytesLeft ) ) ) > 0 ) {
				output.Write( buffer, 0, read );
				bytesLeft -= read;
				if ( bytesLeft <= 0 ) return;
			}
		}

		public static void FillNull( IList<byte> Array, int Location, int Count ) {
			for ( int i = 0; i < Count; ++i ) {
				Array[Location + i] = 0x00;
			}
		}

		public static bool IsByteArrayPartEqual( IList<byte> Array1, int Location1, IList<byte> Array2, int Location2, int count ) {
			for ( int i = 0; i < count; ++i ) {
				if ( Array1[i + Location1] != Array2[i + Location2] ) {
					return false;
				}
			}
			return true;
		}

		public static uint ReadUInt32( this Stream s ) {
			int b1 = s.ReadByte();
			int b2 = s.ReadByte();
			int b3 = s.ReadByte();
			int b4 = s.ReadByte();

			return (uint)( b4 << 24 | b3 << 16 | b2 << 8 | b1 );
		}
		public static uint PeekUInt32( this Stream s ) {
			long pos = s.Position;
			uint retval = s.ReadUInt32();
			s.Position = pos;
			return retval;
		}
		public static uint ReadUInt24( this Stream s ) {
			int b1 = s.ReadByte();
			int b2 = s.ReadByte();
			int b3 = s.ReadByte();

			return (uint)( b3 << 16 | b2 << 8 | b1 );
		}
		public static uint PeekUInt24( this Stream s ) {
			long pos = s.Position;
			uint retval = s.ReadUInt24();
			s.Position = pos;
			return retval;
		}
		public static ushort ReadUInt16( this Stream s ) {
			int b1 = s.ReadByte();
			int b2 = s.ReadByte();

			return (ushort)( b2 << 8 | b1 );
		}
		public static ushort PeekUInt16( this Stream s ) {
			long pos = s.Position;
			ushort retval = s.ReadUInt16();
			s.Position = pos;
			return retval;
		}
		public static string ReadAsciiNullterm( this Stream s ) {
			StringBuilder sb = new StringBuilder();
			int b = s.ReadByte();
			while ( b != 0 && b != -1 ) {
				sb.Append( (char)( b ) );
				b = s.ReadByte();
			}
			return sb.ToString();
		}
		public static string ReadAscii( this Stream s, int count ) {
			StringBuilder sb = new StringBuilder( count );
			int b;
			for ( int i = 0; i < count; ++i ) {
				b = s.ReadByte();
				sb.Append( (char)( b ) );
			}
			return sb.ToString();
		}
		public static string ReadUTF16Nullterm( this Stream s ) {
			StringBuilder sb = new StringBuilder();
			byte[] b = new byte[2];
			int b0 = s.ReadByte();
			int b1 = s.ReadByte();
			while ( !( b0 == 0 && b1 == 0 ) && b1 != -1 ) {
				b[0] = (byte)b0; b[1] = (byte)b1;
				sb.Append( Encoding.Unicode.GetString( b, 0, 2 ) );
				b0 = s.ReadByte(); b1 = s.ReadByte();
			}
			return sb.ToString();
		}
	}
}
