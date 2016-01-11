using System;
using System.IO;

namespace Nes.Library
{
    public class NesFile
    {
        public byte[] prg_rom;
        public byte[] chr_rom;

        private byte flags_6;
        private byte flags_7;
        private byte flags_9;

        public NesFile(string path)
            : this(File.ReadAllBytes(path))
        {
        }

        public NesFile(byte[] data)
        {
            int offset = 0;

            int i;

            char[] id = { ' ', ' ', ' ', ' ' };

            for (i = 0; i < 4; i++)
            {
                Console.Write("0x{0:X2} ", data[offset]);
                id[i] = (char)data[offset];
                offset++;
            }
            Console.WriteLine();

            Console.WriteLine("{0}{1}{2}{3}", id[0], id[1], id[2], id[3]);

            byte prg_rom_size = data[offset];
            Console.WriteLine("PRG ROM 16kb * {0}", prg_rom_size);
            offset++;

            byte chr_rom_size = data[offset];
            Console.WriteLine("CHR ROM 8kb * {0}", chr_rom_size);
            offset++;

            flags_6 = data[offset];
            Console.WriteLine("FLAGS 6 0x{0:X2}", flags_6);
            offset++;

            flags_7 = data[offset];
            Console.WriteLine("FLAGS 7 0x{0:X2}", flags_7);
            offset++;

            byte prg_ram_size = data[offset];
            Console.WriteLine("PRG RAM 8kb * {0}", prg_ram_size);
            offset++;

            flags_9 = data[offset];
            Console.WriteLine("FLAGS 9 0x{0:X2}", flags_9);
            offset++;

            byte flags_10 = data[offset];
            Console.WriteLine("FLAGS 10 0x{0:X2}", flags_10);
            offset++;

            for (i = 0; i < 5; i++)
            {
                Console.Write("0x{0:X2} ", data[offset]);
                offset++;
            }
            Console.WriteLine();

            if (HasTrainer)
            {
                for (i = 0; i < 512; i++)
                {
                    Console.Write("0x{0:X2} ", data[offset]);
                    offset++;
                }
                Console.WriteLine();
            }

            if (PlayChoice10)
            {
                throw new Exception("PlayChoice10 not implemented");
            }

            prg_rom = new byte[16384 * prg_rom_size];
            for (i = 0; i < 16384 * prg_rom_size; i++)
            {
                prg_rom[i] = data[offset];
                offset++;
            }

            chr_rom = new byte[8192 * chr_rom_size];
            for (i = 0; i < 8192 * chr_rom_size; i++)
            {
                chr_rom[i] = data[offset];
                offset++;
            }
        }

        public bool HasTrainer
        {
            get
            {
                return (0x04 & flags_6) == 0x04;
            }
        }

        public bool BatteryRAM
        {
            get
            {
                return (0x02 & flags_6) == 0x02;
            }
        }

        public VRAMLayout VRAMLayout
        {
            get
            {
                if ((0x08 & flags_6) == 0x08)
                {
                    return VRAMLayout.FourScreen;
                }
                if ((0x01 & flags_6) == 0x01)
                {
                    return VRAMLayout.Horizontal;
                }
                return VRAMLayout.Vertical;
            }
        }

        public byte Mapper
        {
            get
            {
                return (byte)((flags_6 >> 4) | (flags_7 & 0xF0));
            }
        }

        public bool VSUnisystem
        {
            get
            {
                return (0x01 & flags_7) == 0x01;
            }
        }

        public bool PlayChoice10
        {
            get
            {
                return (0x02 & flags_7) == 0x02;
            }
        }

        public int Version
        {
            get
            {
                return ((flags_7 & 0x08) == 0x08 ? 2 : 0) +
                    ((flags_7 & 0x04) == 0x04 ? 1 : 0);
            }
        }

        public TVSystem TVSystem
        {
            get
            {
                if ((0x01 & flags_9) == 0x01)
                {
                    return TVSystem.PAL;
                }
                return TVSystem.NTSC;
            }
        }
    }
}
