using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MysteryGiftConvert {
	class Program {
		static void Main( string[] args ) {
			if ( args.Length < 1 ) {
				Console.WriteLine( "Usage: MysteryGiftConvert infile.pcd [outfile.myg]" );
				return;
			}

			string inFilename = args[0];
			var file = File.ReadAllBytes( inFilename );

			if ( file.Length != 856 ) {
				Console.WriteLine( "Input is not a Generation 4 pcd file!" );
				return;
			}

			// display some info about the wonder card
			ushort cardId = BitConverter.ToUInt16( file, 0x150 );
			Console.WriteLine( "Card ID: " + cardId );

			string validGamesShortString = "";
			ushort gameBitmask = BitConverter.ToUInt16( file, 0x14C );
			Console.Write( "This Mystery Gift is valid for:" );
			if ( ( gameBitmask & 0x0400 ) == 0x0400 ) { Console.Write( " Diamond" ); validGamesShortString += 'd'; } else { validGamesShortString += '_'; }
			if ( ( gameBitmask & 0x0800 ) == 0x0800 ) { Console.Write( " Pearl" ); validGamesShortString += 'p'; } else { validGamesShortString += '_'; }
			if ( ( gameBitmask & 0x1000 ) == 0x1000 ) { Console.Write( " Platinum" ); validGamesShortString += 'p'; } else { validGamesShortString += '_'; }
			if ( ( gameBitmask & 0x0080 ) == 0x0080 ) { Console.Write( " HeartGold" ); validGamesShortString += 'g'; } else { validGamesShortString += '_'; }
			if ( ( gameBitmask & 0x0100 ) == 0x0100 ) { Console.Write( " SoulSilver" ); validGamesShortString += 's'; } else { validGamesShortString += '_'; }
			Console.WriteLine();

			// to convert a pcd into a wifi-distributable wonder card file, copy the short description and "header" data from 0x104 to 0x154 to the start of the file
			string outFilename = args.Length >= 2 ? args[1] : cardId + validGamesShortString + ".myg";
			var outStream = new FileStream( outFilename, FileMode.Create );

			outStream.Write( file, 0x104, 0x50 );
			outStream.Write( file, 0, file.Length );

			outStream.Close();

			Console.WriteLine( "Converted " + Path.GetFileName( inFilename ) + " to " + Path.GetFileName( outFilename ) + "!" );
		}
	}
}
