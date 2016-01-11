using System;

namespace Nes.Library
{
    public class CpuMap
    {
        private IMemory m_ram;
        private IMemory m_prg;
        private IMemory m_ppu;

        public CpuMap(IMemory ram, IMemory prg, IMemory ppu)
        {
            m_ram = ram;
            m_prg = prg;
            m_ppu = ppu;
        }

        public void Write(uint address, byte data)
        {
            if (address < 0x2000) // 2KB of mirrored work ram 0x0800
            {
                m_ram.Write(address % 0x0800, data);
            }
            else if (address < 0x4000) // 2KB of mirroed PPU registers 0x0008
            {
                m_ppu.Write((address % 0x0008) + 0x2000, data);
            }
            else if (address < 0x4020) // Registers (Mostly APU)
            {
                // 4000-4013 APU
                // 4014 PPU OAMDMA

                // 4016 Joypad 1
                // 4017 Joypad 2

                throw new Exception("APU registers not implemented");
            }
            else if (address < 0x6000) // Cartridge expansion ROM
            {
                throw new Exception("Cartridge expansion not implemented");
            }
            else if (address < 0x10000)
            {
                throw new Exception("Cannot write to PRG ROM");
            }
            else
            {
                throw new Exception(string.Format("illegal address 0x{0:4}", address));
            }
        }

        public byte Read(uint address)
        {
            if (address < 0x2000) // 2KB of mirrored work ram 0x0800
            {
                return m_ram.Read(address % 0x0800);
            }
            else if (address < 0x4000) // 2KB of mirrored PPU registers 0x0008
            {
                return m_ppu.Read((address % 0x0008) + 0x2000);
            }
            else if (address < 0x4020) // Registers (APU)
            {
                throw new Exception("APU registers not implemented");
            }
            else if (address < 0x6000) // Cartridge expansion ROM
            {
                throw new Exception("Cartridge expansion not implemented");
            }
            else if (address < 0x10000) // PRG ROM mirrored 0x8000 
            {
                return m_prg.Read(address % 0x8000);
            }
            else
            {
                throw new Exception(string.Format("illegal address 0x{0:4}", address));
            }
        }
    }
}
