namespace Nes.Library
{
    public class Computer
    {
        public Cpu Cpu;

        public CpuMap CpuMap;

        public Ppu Ppu;

        public PpuMap PpuMap;

        public Computer(NesFile file, TextLogger logger)
        {
            if (file.Version != 0)
            {
                throw new System.Exception(string.Format("Version {0} not implemented", file.Version));
            }

            if(file.BatteryRAM)
            {
                throw new System.Exception("Battery backed RAM not implemented");
            }

            if (file.HasTrainer)
            {
                throw new System.Exception("Trainer not implemented");
            }

            switch (file.Mapper)
            {
                case 0x00: // NROM

                    //CPU $6000 -$7FFF: Family Basic only: PRG RAM, mirrored as necessary to fill entire 8 KiB window, write protectable with an external switch
                    //CPU $8000 -$BFFF: First 16 KB of ROM.
                    //CPU $C000 -$FFFF: Last 16 KB of ROM(NROM - 256) or mirror of $8000 -$BFFF(NROM - 128).

                    break;

                default:
                    throw new System.Exception(string.Format("Mapper {0} not implemented", file.Mapper));
            }

            if (file.VSUnisystem)
            {
                throw new System.Exception("VSUnisystem not implemented");
            }

            if (file.PlayChoice10)
            {
                throw new System.Exception("PlayChoice10 not implemented");
            }

            PpuMap = new PpuMap(new Ram(0x2000), new Rom(file.chr_rom), file.VRAMLayout);

            Ppu = new Ppu(PpuMap);
            Ppu.Logger = logger;

            CpuMap = new CpuMap(new Ram(0x0800), new Rom(file.prg_rom), Ppu);

            Cpu = new Cpu(CpuMap);
            Cpu.Logger = logger;
        }

        public uint Step()
        {
            uint cycles = 0;

            uint cpuCyclces = Cpu.Step();

            cycles += cpuCyclces;

            uint ppuCycles = 0;

            //while (ppuCycles < cpuCyclces * 3)
            {
                ppuCycles = Ppu.Step();
            }

            cycles += ppuCycles;

            return cycles;
        }
    }
}
