using System.Diagnostics;

namespace Nes.Library
{
    class Rom : IMemory
    {
        private byte[] m_rom;

        public Rom(uint size)
        {
            m_rom = new byte[size];
        }

        public Rom(byte[] data)
        {
            m_rom = new byte[data.Length];
            data.CopyTo(m_rom, 0);
        }

        public byte Read(uint address)
        {
            return m_rom[address];
        }

        public void Write(uint address, byte data)
        {
            Debug.WriteLine("Write to ROM!!!");
        }
    }
}
