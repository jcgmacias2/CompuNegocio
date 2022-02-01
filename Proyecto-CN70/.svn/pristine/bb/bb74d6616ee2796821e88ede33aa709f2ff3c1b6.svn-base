using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Aprovi.Business.Helpers
{
    public class QRCode
    {
        //autoConfigurate: set it to true to select a larger barcode version if the amount of data requires it.
        //backColor: back color of the barcode.
        //barColor: color of the bars.
        //Texto: this is the text to be encoded.
        //correctionLevel: Four levels of error correction recovery are defined in QR Code:
        //        LEVEL_L = 0 ( 7 % )
        //        LEVEL_M = 1 ( 15 % )
        //        LEVEL_Q = 2 ( 25% )
        //        LEVEL_H = 3 ( 30% )
        //encoding: valid encoding algorithms are:
        //        ENC_ALPHA = 0: encodes alphanumeric characters (digits 0 - 9; upper case letters A -Z; nine other characters: space, $ % * + - . / : ); )
        //        ENC_BYTE = 1: encodes binary values ( 8-bit data)
        //        ENC_NUMERIC = 2: encodes numeric values only (digits 0-9)
        //        ENC_KANJI = 3: encodes Kanji characters. Kanji characters in QR Code can have values 8140 -9FFC and E040 - EBBF
        //        ENC_AUTO = 4: automatic seleccion of the encoding algorithm
        //marginpixels: margin in pixels
        //moduleWidth: size in pixels of the dots in the barcode
        //VISITA www.validacfd.com


        public enum TQRCodeEncoding
        {
            ceALPHA,
            ceBYTE,
            ceNUMERIC,
            ceKANJI,
            ceAUTO
        }

        public enum TQRCodeECLevel
        {
            LEVEL_L,
            LEVEL_M,
            LEVEL_Q,
            LEVEL_H
        }


        [DllImport("QRCodeLib.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "FastQRCode")]
        public static extern void FastQRCode(string texto, string fileName);

        [DllImport("QRCodeLib.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "FullQRCode")]
        public static extern void FullQRCode(bool autoConfigurate, bool autoFit, Int32 backColor, Int32 barColor, string texto, TQRCodeECLevel correctionLevel, TQRCodeEncoding encoding, int marginpixels, int moduleWidth, int height, int width, string fileName);
    }
}
