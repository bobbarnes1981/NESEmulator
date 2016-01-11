namespace Nes.Library
{
    public interface IMemory
    {
        byte Read(uint address);
        void Write(uint address, byte data);
    }
}
