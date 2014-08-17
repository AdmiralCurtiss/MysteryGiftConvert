using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MysteryGiftConvert {
	public class MysteryGiftGen5 {
		private byte[] File;

		public ushort CardID {
			get {
				return BitConverter.ToUInt16( File, 0xB0 );
			}
		}

		public MysteryGiftGen5( byte[] file ) {
			this.File = file;
		}

		public void ClearDate() {
			File[0xAC] = 0; File[0xAD] = 0; File[0xAE] = 0; File[0xAF] = 0;
		}

		public void ShowInfo() {
			Console.WriteLine( "Card ID: " + CardID );

			Console.WriteLine();

			// short "name" string
			Console.WriteLine( Encoding.Unicode.GetString( File, 0x60, 0x4A ) );

			Console.WriteLine();
		}

		public static uint GetVersionBitmaskFromString( string s ) {
			bool white1 = false;
			bool black1 = false;
			bool white2 = false;
			bool black2 = false;

			s = s.ToLowerInvariant();
			white2 = s.Contains( "w2" );
			black2 = s.Contains( "b2" );
			s = s.Replace( "w2", "" ).Replace( "b2", "" );
			white1 = s.Contains( "w" );
			black1 = s.Contains( "b" );

			uint versions = 0;
			if ( white1 ) { versions = versions | 0x00100000u; }
			if ( black1 ) { versions = versions | 0x00200000u; }
			if ( white2 ) { versions = versions | 0x00400000u; }
			if ( black2 ) { versions = versions | 0x00800000u; }
			return versions;
		}
	}
}
