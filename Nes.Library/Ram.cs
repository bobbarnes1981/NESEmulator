namespace Nes.Library
{
    class Ram : IMemory
    {
        private byte[] m_ram;

        public Ram(uint size)
        {
            m_ram = new byte[size];
        }

        public byte Read(uint address)
        {
            return m_ram[address];
        }

        public void Write(uint address, byte data)
        {
            m_ram[address] = data;
        }
    }
}
