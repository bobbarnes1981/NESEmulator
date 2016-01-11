namespace Nes.Library
{
    public class Ppu : IMemory
    {
        private PpuMap m_map;

        public TextLogger Logger;
        
        private int m_scanline = -1; // -1 to 260
        public int Scanline
        {
            get
            {
                return m_scanline;
            }
        }

        private int m_pixel = 0; // 0 to 340
        public int Pixel
        {
            get
            {
                return m_pixel;
            }
        }

        public byte[,] m_pixels = new byte[340, 260];

        private AddressLatch m_addressLatch = AddressLatch.None;

        private ScrollLatch m_scrollLatch = ScrollLatch.None;

        private byte[] m_oam = new byte[0xFF];

        private byte m_regCtrl;
        /// <summary>
        /// PPUCTRL	$2000	VPHB SINN   NMI enable(V), PPU master/slave(P), sprite height(H), background tile select(B), sprite tile select(S), increment mode(I), nametable select(NN)
        /// 7  bit  0
        /// --------
        /// VPHB SINN
        /// |||| ||||
        /// |||| ||++-Base nametable address
        /// |||| ||   (0 = $2000; 1 = $2400; 2 = $2800; 3 = $2C00)
        /// |||| |+---VRAM address increment per CPU read / write of PPUDATA
        /// |||| |    (0: add 1, going across; 1: add 32, going down)
        /// |||| +----Sprite pattern table address for 8x8 sprites
        /// ||||      (0: $0000; 1: $1000; ignored in 8x16 mode)
        /// |||+------Background pattern table address(0: $0000; 1: $1000)
        /// ||+-------Sprite size(0: 8x8; 1: 8x16)
        /// |+--------PPU master / slave select
        /// |         (0: read backdrop from EXT pins; 1: output color on EXT pins)
        /// +---------Generate an NMI at the start of the
        ///           vertical blanking interval (0: off; 1: on)
        /// </summary>
        public byte RegCtrl
        {
            get
            {
                // write only
                return m_regCtrl;
            }
            set
            {
                m_regCtrl = value;
            }
        }

        private byte m_regMask;
        /// <summary>
        /// PPUMASK	$2001	BGRs bMmG   color emphasis(BGR), sprite enable(s), background enable(b), sprite left column enable(M), background left column enable(m), greyscale(G)
        /// 7  bit  0
        /// --------
        /// BGRs bMmG
        /// |||| ||||
        /// |||| |||+-Grayscale(0: normal color, 1: produce a greyscale display)
        /// |||| ||+--1: Show background in leftmost 8 pixels of screen, 0: Hide
        /// |||| |+---1: Show sprites in leftmost 8 pixels of screen, 0: Hide
        /// |||| +----1: Show background
        /// |||+------1: Show sprites
        /// ||+-------Emphasize red *
        /// |+--------Emphasize green *
        /// +---------Emphasize blue *
        /// </summary>
        public byte RegMask
        {
            get
            {
                // write only
                return m_regMask;
            }
            set
            {
                m_regMask = value;
            }
        }

        private byte m_regStat;
        /// <summary>
        /// PPUSTATUS	$2002	VSO- ----	vblank(V), sprite 0 hit(S), sprite overflow(O), read resets write pair for $2005/2006
        /// 7  bit  0
        /// --------
        /// VSO. ....
        /// |||| ||||
        /// |||+-++++-Least significant bits previously written into a PPU register
        /// |||       (due to register not being updated for this address)
        /// ||+-------Sprite overflow.The intent was for this flag to be set
        /// ||        whenever more than eight sprites appear on a scanline, but a
        /// ||        hardware bug causes the actual behavior to be more complicated
        /// ||        and generate false positives as well as false negatives; see
        /// ||        PPU sprite evaluation.This flag is set during sprite
        /// ||        evaluation and cleared at dot 1(the second dot) of the
        /// ||        pre - render line.
        /// |+--------Sprite 0 Hit.Set when a nonzero pixel of sprite 0 overlaps
        /// |         a nonzero background pixel; cleared at dot 1 of the pre - render
        /// |         line.Used for raster timing.
        /// +---------Vertical blank has started(0: not in vblank; 1: in vblank).
        ///           Set at dot 1 of line 241(the line * after * the post - render
        ///           line); cleared after reading $2002 and at dot 1 of the
        ///           pre - render line.
        /// </summary>
        public byte RegStat
        {
            get
            {
                m_addressLatch = AddressLatch.MSB;
                m_scrollLatch = ScrollLatch.X;

                return m_regStat;
            }
            set
            {
                throw new System.Exception("PPU RegStat Cannot Write");
            }
        }

        private byte m_regOAMAddr;
        /// <summary>
        /// OAMADDR	$2003	aaaa aaaa   OAM read/write address
        /// 
        /// Write the address of OAM you want to access here.Most games just write $00 here and then use OAMDMA. (DMA is implemented in the 2A03/7 chip and works by repeatedly writing to OAMDATA)
        /// Values during rendering
        /// OAMADDR is set to 0 during each of ticks 257-320 (the sprite tile loading interval) of the pre-render and visible scanlines.
        /// The value of OAMADDR when sprite evaluation starts at tick 65 of the visible scanlines will determine where in OAM sprite evaluation starts, and hence which sprite gets treated as sprite 0. The first OAM entry to be checked during sprite evaluation is the one starting at OAM[OAMADDR]. If OAMADDR is unaligned and does not point to the y position (first byte) of an OAM entry, then whatever it points to(tile index, attribute, or x coordinate) will be reinterpreted as a y position, and the following bytes will be similarly reinterpreted.No more sprites will be found once the end of OAM is reached, effectively hiding any sprites before OAM[OAMADDR].
        /// OAMADDR precautions
        /// On the 2C02, writes to OAMADDR reliably corrupt OAM.[5] This can then be worked around by writing all 256 bytes of OAM.
        /// It is also the case that if OAMADDR is not less than eight when rendering starts, the eight bytes starting at OAMADDR & 0xF8 are copied to the first eight bytes of OAM; it seems likely that this is related.The former bug is known to have been fixed in the 2C07; the latter is suspected to be.On the Dendy, the latter bug is required for 2C02 compatibility.
        /// </summary>
        public byte RegOAMAddr
        {
            get
            {
                return m_regOAMAddr;
            }
            set
            {
                m_regOAMAddr = value;
            }
        }
        
        /// <summary>
        /// OAMDATA	$2004	dddd dddd   OAM data read/write
        ///
        /// Write OAM data here.Writes will increment OAMADDR after the write; reads during vertical or forced blanking return the value from OAM at that address but do not increment.
        /// Because changes to OAM should normally be made only during vblank, writing through OAMDATA is only effective for partial updates (it is too slow). Most games will use the DMA feature through OAMDMA instead.
        /// Reading OAMDATA while the PPU is rendering will expose internal OAM accesses during sprite evaluation and loading; Micro Machines does this.
        /// Writes to OAMDATA during rendering (on the pre-render line and the visible lines 0-239, provided either sprite or background rendering is enabled) do not modify values in OAM, but do perform a glitchy increment of OAMADDR, bumping only the high 6 bits (i.e., it bumps the [n]
        /// value in PPU sprite evaluation - it's plausible that it could bump the low bits instead depending on the current status of sprite evaluation). This extends to DMA transfers via OAMDMA, since that uses writes to $2004. For emulation purposes, it is probably best to completely ignore writes during rendering.
        /// It used to be thought that reading from this register wasn't reliable[6], however more recent evidence seems to suggest that this is solely due to corruption by OAMADDR writes.
        /// In the oldest instantiations of the PPU, as found on earlier Famicoms and NESes, this register is not readable [7]. The readability was added on the RP2C02G, found on most NESes and later Famicoms.[8]
        /// In the 2C07, sprite evaluation can never be fully disabled, and will always start 20 scanlines after the start of vblank [9] (same as when the prerender scanline would have been on the 2C02). As such, you must upload anything to OAM that you intend to within the first 20 scanlines after the 2C07 signals vertical blanking.
        /// </summary>
        public byte RegOAMData
        {
            get
            {
                throw new System.NotImplementedException("PPU RegOAMData Read");
            }
            set
            {
                m_oam[m_regOAMAddr] = value;
                m_regOAMAddr += 1;
            }
        }

        private byte m_scrollX;
        private byte m_scrollY;
        /// <summary>
        /// PPUSCROLL	$2005	xxxx xxxx   fine scroll position(two writes: X, Y)
        ///
        /// This register is used to change the scroll position, that is, to tell the PPU which pixel of the nametable selected through PPUCTRL should be at the top left corner of the rendered screen. Typically, this register is written to during vertical blanking, so that the next frame starts rendering from the desired location, but it can also be modified during rendering in order to split the screen. Changes made to the vertical scroll during rendering will only take effect on the next frame.
        /// 
        /// Horizontal offsets range from 0 to 255. "Normal" vertical offsets range from 0 to 239, while values of 240 to 255 are treated as -16 through -1 in a way, but tile data is incorrectly fetched from the attribute table.
        /// By changing the values here across several frames and writing tiles to newly revealed areas of the nametables, one can achieve the effect of a camera panning over a large background.
        /// </summary>
        public byte RegScrl
        {
            get
            {
                throw new System.NotImplementedException("PPU RegScrl Read");
            }
            set
            {
                switch (m_scrollLatch)
                {
                    case ScrollLatch.X:
                        m_scrollX = value;
                        m_scrollLatch = ScrollLatch.Y;
                        break;
                    case ScrollLatch.Y:
                        m_scrollY = value;
                        m_scrollLatch = ScrollLatch.None;
                        break;
                }
            }
        }

        private uint m_regAddr;
        /// <summary>
        /// PPUADDR	$2006	aaaa aaaa   PPU read/write address(two writes: MSB, LSB)
        /// </summary>
        public byte RegAddr
        {
            get
            {
                throw new System.NotImplementedException("PPU RegAddr Read");
            }
            set
            {
                switch(m_addressLatch)
                {
                    case AddressLatch.MSB:
                        m_regAddr = (uint)(value & 0x00FF);
                        m_addressLatch = AddressLatch.LSB;
                        break;
                    case AddressLatch.LSB:
                        m_regAddr |= (uint)(value << 8);
                        m_addressLatch = AddressLatch.None;
                        break;
                }
            }
        }

        /// <summary>
        /// PPUDATA	$2007	dddd dddd   PPU data read/write
        /// </summary>
        public byte RegData
        {
            get
            {
                throw new System.NotImplementedException("PPU RegData Read");
            }
            set
            {
                m_map.Write(m_regAddr, value);

                //VRAM read/ write data register. After access, the video memory address will increment by an amount determined by $2000:2.
                //When the screen is turned off by disabling the background / sprite rendering flag with the PPUMASK or during vertical blank, you can read or write data from VRAM through this port.Since accessing this register increments the VRAM address, it should not be accessed outside vertical or forced blanking because it will cause graphical glitches, and if writing, write to an unpredictable address in VRAM.However, two games are known to read from PPUDATA during rendering: see Tricky-to - emulate games.
                //VRAM reading and writing shares the same internal address register that rendering uses.So after loading data into video memory, the program should reload the scroll position afterwards with PPUSCROLL writes in order to avoid wrong scrolling.

                // VRAM address increment per CPU read / write of PPUDATA
                // (0: add 1, going across; 1: add 32, going down)
                if ((m_regCtrl & 0x02) == 0x02)
                {
                    throw new System.NotImplementedException("add 32, going down");
                }
                else
                {
                    m_regAddr += 1;
                }
            }
        }

        /// <summary>
        /// OAMDMA	$4014	aaaa aaaa   OAM DMA high address
        /// </summary>
        public byte OAMDMA
        {
            get
            {
                throw new System.NotImplementedException("PPU RegOAMDMA Read");

            }
            set
            {
                throw new System.NotImplementedException("PPU RegOAMDMA Write");
            }
        }

        public Ppu(PpuMap map)
        {
            m_map = map;
        }

        public byte Read(uint address)
        {
            byte data;

            switch (address)
            {
                case 0x2000:
                    data = RegCtrl;
                    break;

                case 0x2001:
                    data = RegMask;
                    break;

                case 0x2002:
                    data = RegStat;
                    break;

                case 0x2003:
                    data = RegOAMAddr;
                    break;

                case 0x2004:
                    data = RegOAMData;
                    break;

                default:
                    throw new System.Exception(string.Format("PPU read not implemented or illegal 0x{0:X4}", address));
            }

            return data;
        }

        public void Write(uint address, byte data)
        {
            switch (address)
            {
                case 0x2000:
                    RegCtrl = data;
                    break;

                case 0x2001:
                    RegMask = data;
                    break;

                case 0x2005:
                    RegScrl = data;
                    break;

                case 0x2006:
                    RegAddr = data;
                    break;

                case 0x2007:
                    RegData = data;
                    break;

                default:
                    throw new System.Exception(string.Format("PPU write not implemented or illegal 0x{0:X4}", address));
            }
        }

        public uint Step()
        {
            // TODO: http://www.matrix44.net/old/sdl/sdlnet.html

            uint cycles = 0;


            //OAMADDR is set to 0 during each of ticks 257 - 320(the sprite tile loading interval) of the pre - render and visible scanlines.
            if (m_pixel >= 257 && m_pixel <= 320 && m_scanline >= -1 && m_scanline <= 239)
            {
                m_regOAMAddr = 0x00;
            }


            //http://wiki.nesdev.com/w/index.php/PPU_sprite_evaluation
            //Each scanline, the PPU reads the spritelist (that is, Object Attribute Memory) to see which to draw:
            //First, it clears the list of sprites to draw.
            //Second, it reads through OAM, checking which sprites will be on this scanline.It chooses the first eight it finds that do.
            //Third, if eight sprites were found, it checks (in a wrongly - implemented fashion) for further sprites on the scanline to see if the sprite overflow flag should be set.
            //Fourth, using the details for the eight (or fewer) sprites chosen, it determines which pixels each has on the scanline and where to draw them.


            //Line - by - line timing

            //The PPU renders 262 scanlines per frame.Each scanline lasts for 341 PPU clock cycles(113.667 CPU clock cycles; 1 CPU cycle = 3 PPU cycles), with each clock cycle producing one pixel.The line numbers given here correspond to how the internal PPU frame counters count lines.
            //The information in this section is summarized in the diagram in the next section.
            //The timing below is for NTSC PPUs.PPUs for 50 Hz TV systems differ: PAL NES PPUs render 70 vblank scanlines instead of 20, and Dendy PPUs render 51 post-render scanlines instead of 1.
            if (m_scanline == -1)
            {
                //Pre-render scanline (-1, 261)


                // This is a dummy scanline, whose sole purpose is to fill the shift registers with the data for the first two tiles of the next 
                // scanline.Although no pixels are rendered for this scanline, the PPU still makes the same memory accesses it would for a 
                // regular scanline.
                // This scanline varies in length, depending on whether an even or an odd frame is being rendered.For odd frames, the cycle at 
                // the end of the scanline is skipped (this is done internally by jumping directly from(339, 261) to(0, 0), replacing the idle 
                // tick at the beginning of the first visible scanline with the last tick of the last dummy nametable fetch).For even frames, the
                // last cycle occurs normally. This is done to compensate for some shortcomings with the way the PPU physically outputs its video
                // signal, the end result being a crisper image when the screen isn't scrolling. However, this behavior can be bypassed by
                // keeping rendering disabled until after this scanline has passed, which results in an image that looks more like a 
                // traditionally interlaced picture.
                // During pixels 280 through 304 of this scanline, the vertical scroll bits are reloaded if rendering is enabled.
            }
            else if (m_scanline < 240)
            {
                //Visible scanlines (0 - 239)


                // These are the visible scanlines, which contain the graphics to be displayed on the screen.This includes the rendering of both
                // the background and the sprites.During these scanlines, the PPU is busy fetching data, so the program should not access PPU 
                // memory during this time, unless rendering is turned off.

                if (m_pixel == 0)
                {
                    //Cycle 0


                    // This is an idle cycle.The value on the PPU address bus during this cycle appears to be the same CHR address that is later
                    // used to fetch the low background tile byte starting at dot 5(possibly calculated during the two unused NT fetches at the 
                    // end of the previous scanline).

                }
                else if (m_pixel < 257)
                {
                    //Cycles 1 - 256


                    // The data for each tile is fetched during this phase.Each memory access takes 2 PPU cycles to complete, and 4 must be performed per tile:
                    //   Nametable byte
                    //   Attribute table byte
                    //   Tile bitmap low
                    //   Tile bitmap high(+8 bytes from tile bitmap low)
                    // The data fetched from these accesses is placed into internal latches, and then fed to the appropriate shift registers when it's time to 
                    // do so (every 8 cycles). Because the PPU can only fetch an attribute byte every 8 cycles, each sequential string of 8 pixels is forced 
                    // to have the same palette attribute.

                    // TODO: load nametable byte, load attribute table byte, load bitmap lo, load bitmap hi

                    // Sprite zero hits act as if the image starts at cycle 2 (which is the same cycle that the shifters shift for the first time), so the 
                    // sprite zero flag will be raised at this point at the earliest. Actual pixel output is delayed further due to internal render pipelining,
                    // and the first pixel is output during cycle 4.
                    
                    // The shifters are reloaded during ticks 9, 17, 25, ..., 257.
                    // Note: At the beginning of each scanline, the data for the first two tiles is already loaded into the shift registers(and ready to be
                    // rendered), so the first tile that gets fetched is Tile 3.
                    // While all of this is going on, sprite evaluation for the next scanline is taking place as a seperate process, independent to what's 
                    // happening here.

                }
                else if (m_pixel < 321)
                {
                    //Cycles 257-320


                    //The tile data for the sprites on the next scanline are fetched here.Again, each memory access takes 2 PPU cycles to complete, and 4 are performed for each of the 8 sprites:
                    // Garbage nametable byte
                    // Garbage nametable byte
                    // Tile bitmap low
                    // Tile bitmap high (+8 bytes from tile bitmap low)
                    //The garbage fetches occur so that the same circuitry that performs the BG tile fetches could be reused for the sprite tile fetches.
                    //If there are less than 8 sprites on the next scanline, then dummy fetches to tile $FF occur for the left-over sprites, because of the dummy sprite data in the secondary OAM (see sprite evaluation). This data is then discarded, and the sprites are loaded with a transparent bitmap instead.
                    //In addition to this, the X positions and attributes for each sprite are loaded from the secondary OAM into their respective counters/latches. This happens during the second garbage nametable fetch, with the attribute byte loaded during the first tick and the X coordinate during the second.

                }
                else if (m_pixel < 337)
                {
                    //Cycles 321-336


                    //This is where the first two tiles for the next scanline are fetched, and loaded into the shift registers.Again, each memory access takes 2 PPU cycles to complete, and 4 are performed for the two tiles:
                    // Nametable byte
                    // Attribute table byte
                    // Tile bitmap low
                    // Tile bitmap high (+8 bytes from tile bitmap low)

                }
                else if (m_pixel < 341)
                {
                    //Cycles 337-340


                    //Two bytes are fetched, but the purpose for this is unknown.These fetches are 2 PPU cycles each.
                    // Nametable byte
                    // Nametable byte
                    //Both of the bytes fetched here are the same nametable byte that will be fetched at the beginning of the next scanline(tile 3, in other words). At least one mapper is known to use this string of three consecutive nametable fetches to clock a scanline counter.

                }
            }
            else if (m_scanline == 240)
            {
                //Post - render scanline(240)


                //The PPU just idles during this scanline.Even though accessing PPU memory from the program would be safe here, the VBlank flag isn't set until after this scanline.
            }
            else if (m_scanline < 261)
            {
                //Vertical blanking lines(241 - 260)


                //The VBlank flag of the PPU is set at tick 1(the second tick) of scanline 241, where the VBlank NMI also occurs. The PPU makes no memory accesses during these scanlines, so PPU memory can be freely accessed by the program.

                if (m_scanline != 241 || m_pixel > 0)
                {
                    m_regStat |= 0x80; // Set bit 7
                }
                else
                {
                    m_regStat &= 0x7F; // Clear bit 7
                }
            }
            

            




            // Probably not right
            m_pixel++;
            if (m_pixel == 341)
            {
                m_pixel = 0;
                m_scanline++;
            }
            if (m_scanline == 261)
            {
                m_scanline = -1;
            }

            return cycles;
        }
    }
}
