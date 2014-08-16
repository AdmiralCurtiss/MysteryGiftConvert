using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MysteryGiftConvert {
	public class MysteryGiftGen4 {
		private byte[] File;

		public string ValidGamesShortString {
			get {
				StringBuilder sb = new StringBuilder();
				ushort gameBitmask = BitConverter.ToUInt16( File, 0x14C );
				if ( ( gameBitmask & 0x0400 ) == 0x0400 ) { sb.Append( 'd' ); } else { sb.Append( '_' ); }
				if ( ( gameBitmask & 0x0800 ) == 0x0800 ) { sb.Append( 'p' ); } else { sb.Append( '_' ); }
				if ( ( gameBitmask & 0x1000 ) == 0x1000 ) { sb.Append( 'p' ); } else { sb.Append( '_' ); }
				if ( ( gameBitmask & 0x0080 ) == 0x0080 ) { sb.Append( 'g' ); } else { sb.Append( '_' ); }
				if ( ( gameBitmask & 0x0100 ) == 0x0100 ) { sb.Append( 's' ); } else { sb.Append( '_' ); }
				return sb.ToString();
			}
		}
		public ushort CardID {
			get {
				return BitConverter.ToUInt16( File, 0x150 );
			}
		}

		public MysteryGiftGen4( byte[] file ) {
			this.File = file;
		}

		public void ShowInfo() {
			Console.WriteLine( "Card ID: " + CardID );

			ushort gameBitmask = BitConverter.ToUInt16( File, 0x14C );
			Console.Write( "This Mystery Gift is valid for:" );
			if ( ( gameBitmask & 0x0400 ) == 0x0400 ) { Console.Write( " Diamond" ); }
			if ( ( gameBitmask & 0x0800 ) == 0x0800 ) { Console.Write( " Pearl" ); }
			if ( ( gameBitmask & 0x1000 ) == 0x1000 ) { Console.Write( " Platinum" ); }
			if ( ( gameBitmask & 0x0080 ) == 0x0080 ) { Console.Write( " HeartGold" ); }
			if ( ( gameBitmask & 0x0100 ) == 0x0100 ) { Console.Write( " SoulSilver" ); }
			Console.WriteLine();
			Console.WriteLine();

			// short description displayed on download
			Console.WriteLine( String4.GetStringGeneration4( File, 0x104, 36 ) );

			// long description on the wonder card
			Console.WriteLine( String4.GetStringGeneration4( File, 0x154, 250 ) );

			Console.WriteLine();
		}
	}
}
