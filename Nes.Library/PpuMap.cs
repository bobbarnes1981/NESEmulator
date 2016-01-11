using System;

namespace Nes.Library
{
    public class PpuMap
    {
        private IMemory m_ram;

        private IMemory m_chr;

        private IMemory m_palette;

        private VRAMLayout m_layout;

        public PpuMap(IMemory ram, IMemory chr, VRAMLayout layout)
        {
            m_ram = ram;
            m_chr = chr;
            m_palette = new Ram(0xFF);
            m_layout = layout;
        }

        public void Write(uint address, byte data)
        {
            // mirror everything above 0x4000
            address = address % 0x4000;

            if (address < 0x2000)
            {
                m_chr.Write(address, data);
            }
            else if (address < 0x3F00)
            {
                if (address >= 0x3000)
                {
                    address -= 0x1000;
                }
                switch(m_layout)
                {
                    case VRAMLayout.Horizontal:
                        if (address < 0x2800)
                        {
                            // mirror 0 and 1
                            address = address % 0x0400;
                        }
                        else
                        {
                            // mirror 2 and 3
                            address = (address % 0x0400) + 0x0800;
                        }
                        m_ram.Write(address, data);
                        m_ram.Write(address + 0x0400, data);
                        break;

                    case VRAMLayout.Vertical:
                        throw new Exception("Mirror nametable 0 into 2 and 1 into 3");

                    default:
                        throw new NotImplementedException(string.Format("VRAM layout {0} not implemented", m_layout));
                }
            }
            else if (address < 0x4000)
            {
                if (address >= 0x3F20)
                {
                    address -= 0x0020;
                }
                address -= 0x3F00;
                m_palette.Write(address, data);
            }
        }

        public byte Read(uint address)
        {
            throw new System.NotImplementedException();
            if (address < 0x2000)
            {

            }
            else if (address < 0x3F00)
            {

            }
            else if (address < 0x4000)
            {

            }
            else
            {
                throw new Exception(string.Format("illegal address 0x{0:X4}", address));
            }
        }
    }
}
