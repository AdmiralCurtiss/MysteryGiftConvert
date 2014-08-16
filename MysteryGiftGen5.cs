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

		public void ShowInfo() {
			Console.WriteLine( "Card ID: " + CardID );

			Console.WriteLine();

			// short "name" string
			Console.WriteLine( Encoding.Unicode.GetString( File, 0x60, 0x4A ) );

			Console.WriteLine();
		}
	}
}
