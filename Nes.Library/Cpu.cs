using System;
using System.Collections.Generic;
using System.Text;

namespace Nes.Library
{
    public class Cpu
    {
        private CpuMap m_map;

        public TextLogger Logger;

        private uint m_pc;
        public uint PC
        {
            get
            {
                return m_pc;
            }
        }

        private byte m_sp;
        public byte SP
        {
            get
            {
                return m_sp;
            }
        }

        private byte m_reg_a;
        public byte RegA
        {
            get
            {
                return m_reg_a;
            }
        }

        private byte m_reg_p;
        public byte RegP
        {
            get
            {
                return m_reg_p;
            }
        }

        private byte m_reg_x;
        public byte RegX
        {
            get
            {
                return m_reg_x;
            }
        }

        private byte m_reg_y;
        public byte RegY
        {
            get
            {
                return m_reg_y;
            }
        }

        delegate void Op(uint? address);

        private Dictionary<byte, Op> m_opcode_methods;

        private Dictionary<byte, string> m_opcode_names = new Dictionary<byte, string>
        {
            { 0x08, "PHP" },
            { 0x09, "ORA" },
            { 0x0A, "ASL" },
            { 0x10, "BPL" },
            { 0x18, "CLC" },
            { 0x20, "JSR" },
            { 0x28, "PLP" },
            { 0x29, "AND" },
            { 0x38, "SEC" },
            { 0x45, "EOR" },
            { 0x48, "PHA" },
            { 0x4A, "LSR" },
            { 0x56, "LSR" },
            { 0x60, "RTS" },
            { 0x65, "ADC" },
            { 0x68, "PLA" },
            { 0x6A, "ROR" },
            { 0x76, "ROR" },
            { 0x78, "SEI" },
            { 0x84, "STY" },
            { 0x85, "STA" },
            { 0x86, "STX" },
            { 0x88, "DEY" },
            { 0x8A, "TXA" },
            { 0x8C, "STY" },
            { 0x8D, "STA" },
            { 0x8E, "STX" },
            { 0x90, "BCC" },
            { 0x91, "STA" },
            { 0x98, "TYA" },
            { 0x99, "STA" },
            { 0x9A, "TXS" },
            { 0xA0, "LDY" },
            { 0xA2, "LDX" },
            { 0xA4, "LDY" },
            { 0xA5, "LDA" },
            { 0xA6, "LDX" },
            { 0xA9, "LDA" },
            { 0xAA, "TAX" },
            { 0xAD, "LDA" },
            { 0xAE, "LDX" },
            { 0xB0, "BCS" },
            { 0xB1, "LDA" },
            { 0xB5, "LDA" },
            { 0xBD, "LDA" },
            { 0xC6, "DEC" },
            { 0xC8, "INY" },
            { 0xC9, "CMP" },
            { 0xCA, "DEX" },
            { 0xD0, "BNE" },
            { 0xD8, "CLD" },
            { 0xE5, "SBC" },
            { 0xE8, "INX" },
            { 0xF0, "BEQ" }
        };

        private Dictionary<byte, uint> m_opcode_cycles = new Dictionary<byte, uint>
        {
            { 0x08, 3 },
            { 0x09, 2 },
            { 0x0A, 2 },
            { 0x10, 2 },
            { 0x18, 2 },
            { 0x20, 6 },
            { 0x28, 4 },
            { 0x29, 2 },
            { 0x38, 2 },
            { 0x45, 3 },
            { 0x48, 3 },
            { 0x4A, 2 },
            { 0x56, 6 },
            { 0x60, 6 },
            { 0x65, 3 },
            { 0x68, 4 },
            { 0x6A, 2 },
            { 0x76, 6 },
            { 0x78, 2 },
            { 0x84, 3 },
            { 0x85, 3 },
            { 0x86, 3 },
            { 0x88, 2 },
            { 0x8A, 2 },
            { 0x8C, 4 },
            { 0x8D, 4 },
            { 0x8E, 4 },
            { 0x90, 2 },
            { 0x91, 6 },
            { 0x98, 2 },
            { 0x99, 5 },
            { 0x9A, 2 },
            { 0xA0, 2 },
            { 0xA2, 2 },
            { 0xA4, 3 },
            { 0xA5, 3 },
            { 0xA6, 3 },
            { 0xA9, 2 },
            { 0xAA, 2 },
            { 0xAD, 4 },
            { 0xAE, 4 },
            { 0xB0, 2 },
            { 0xB1, 5 },
            { 0xB5, 4 },
            { 0xBD, 4 },
            { 0xC6, 5 },
            { 0xC8, 2 },
            { 0xC9, 2 },
            { 0xCA, 2 },
            { 0xD0, 2 },
            { 0xD8, 2 },
            { 0xE5, 3 },
            { 0xE8, 2 },
            { 0xF0, 2 }
        };

        private Dictionary<byte, byte> m_opcode_lengths = new Dictionary<byte, byte>
        {
            { 0x08, 1 },
            { 0x09, 2 },
            { 0x0A, 1 },
            { 0x10, 2 },
            { 0x18, 1 },
            { 0x20, 3 },
            { 0x28, 1 },
            { 0x29, 2 },
            { 0x38, 1 },
            { 0x45, 2 },
            { 0x48, 1 },
            { 0x4A, 1 },
            { 0x56, 2 },
            { 0x60, 1 },
            { 0x65, 2 },
            { 0x68, 1 },
            { 0x6A, 1 },
            { 0x76, 2 },
            { 0x78, 1 },
            { 0x84, 2 },
            { 0x85, 2 },
            { 0x86, 2 },
            { 0x88, 1 },
            { 0x8A, 1 },
            { 0x8C, 3 },
            { 0x8D, 3 },
            { 0x8E, 3 },
            { 0x90, 2 },
            { 0x91, 2 },
            { 0x98, 1 },
            { 0x99, 3 },
            { 0x9A, 1 },
            { 0xA0, 2 },
            { 0xA2, 2 },
            { 0xA4, 2 },
            { 0xA5, 2 },
            { 0xA6, 2 },
            { 0xA9, 2 },
            { 0xAA, 1 },
            { 0xAD, 3 },
            { 0xAE, 3 },
            { 0xB0, 2 },
            { 0xB1, 2 },
            { 0xB5, 2 },
            { 0xBD, 3 },
            { 0xC6, 2 },
            { 0xC8, 1 },
            { 0xC9, 2 },
            { 0xCA, 1 },
            { 0xD0, 2 },
            { 0xD8, 1 },
            { 0xE5, 2 },
            { 0xE8, 1 },
            { 0xF0, 2 }
        };
        
        private Dictionary<byte, AddressMode> m_opcode_modes = new Dictionary<byte, AddressMode>
        {
            { 0x08, AddressMode.Implied },
            { 0x09, AddressMode.Immediate },
            { 0x0A, AddressMode.Accumulator },
            { 0x10, AddressMode.Relative },
            { 0x18, AddressMode.Implied },
            { 0x20, AddressMode.Absolute },
            { 0x28, AddressMode.Implied },
            { 0x29, AddressMode.Immediate },
            { 0x38, AddressMode.Implied },
            { 0x45, AddressMode.ZeroPage },
            { 0x48, AddressMode.Implied },
            { 0x4A, AddressMode.Accumulator },
            { 0x56, AddressMode.ZeroPageX },
            { 0x60, AddressMode.Implied },
            { 0x65, AddressMode.ZeroPage },
            { 0x68, AddressMode.Implied },
            { 0x6A, AddressMode.Accumulator },
            { 0x76, AddressMode.ZeroPageX },
            { 0x78, AddressMode.Implied },
            { 0x84, AddressMode.ZeroPage },
            { 0x85, AddressMode.ZeroPage },
            { 0x86, AddressMode.ZeroPage },
            { 0x88, AddressMode.Implied },
            { 0x8A, AddressMode.Implied },
            { 0x8C, AddressMode.Absolute },
            { 0x8D, AddressMode.Absolute },
            { 0x8E, AddressMode.Absolute },
            { 0x90, AddressMode.Relative },
            { 0x91, AddressMode.IndirectY },
            { 0x98, AddressMode.Implied },
            { 0x99, AddressMode.AbsoluteY },
            { 0x9A, AddressMode.Implied },
            { 0xA0, AddressMode.Immediate },
            { 0xA2, AddressMode.Immediate },
            { 0xA4, AddressMode.ZeroPage },
            { 0xA5, AddressMode.ZeroPage },
            { 0xA6, AddressMode.ZeroPage },
            { 0xA9, AddressMode.Immediate },
            { 0xAA, AddressMode.Implied },
            { 0xAD, AddressMode.Absolute },
            { 0xAE, AddressMode.Absolute },
            { 0xB0, AddressMode.Relative },
            { 0xB1, AddressMode.IndirectY },
            { 0xB5, AddressMode.ZeroPageX },
            { 0xBD, AddressMode.AbsoluteX },
            { 0xC6, AddressMode.ZeroPage },
            { 0xC8, AddressMode.Implied },
            { 0xC9, AddressMode.Immediate },
            { 0xCA, AddressMode.Implied },
            { 0xD0, AddressMode.Relative },
            { 0xD8, AddressMode.Implied },
            { 0xE5, AddressMode.ZeroPage },
            { 0xE8, AddressMode.Implied },
            { 0xF0, AddressMode.Relative }
        };

        public Cpu(CpuMap map)
        {
            m_map = map;

            // read pc from fffc lo and fffd hi
            m_pc = m_map.Read(0xFFFC);
            m_pc |= (uint)(m_map.Read(0xFFFD)<<8);

            m_opcode_methods = new Dictionary<byte, Op>
            {
                { 0x08, Op_PHP },
                { 0x09, Op_ORA },
                { 0x10, Op_BPL },
                { 0x0A, Op_ASL },
                { 0x18, Op_CLC },
                { 0x20, Op_JSR },
                { 0x28, Op_PLP },
                { 0x29, Op_AND },
                { 0x38, Op_SEC },
                { 0x45, Op_EOR },
                { 0x48, Op_PHA },
                { 0x4A, Op_LSR },
                { 0x56, Op_LSR },
                { 0x60, Op_RTS },
                { 0x65, Op_ADC },
                { 0x68, Op_PLA },
                { 0x6A, Op_ROR },
                { 0x76, Op_ROR },
                { 0x78, Op_SEI },
                { 0x84, Op_STY },
                { 0x85, Op_STA },
                { 0x86, Op_STX },
                { 0x88, Op_DEY },
                { 0x8A, Op_TXA },
                { 0x8C, Op_STY },
                { 0x8D, Op_STA },
                { 0x8E, Op_STX },
                { 0x90, Op_BCC },
                { 0x91, Op_STA },
                { 0x98, Op_TYA },
                { 0x99, Op_STA },
                { 0x9A, Op_TXS },
                { 0xA0, Op_LDY },
                { 0xA2, Op_LDX },
                { 0xA4, Op_LDY },
                { 0xA5, Op_LDA },
                { 0xA6, Op_LDX },
                { 0xA9, Op_LDA },
                { 0xAA, Op_TAX },
                { 0xAD, Op_LDA },
                { 0xAE, Op_LDX },
                { 0xB0, Op_BCS },
                { 0xB1, Op_LDA },
                { 0xB5, Op_LDA },
                { 0xBD, Op_LDA },
                { 0xC6, Op_DEC },
                { 0xC8, Op_INY },
                { 0xC9, Op_CMP },
                { 0xCA, Op_DEX },
                { 0xD0, Op_BNE },
                { 0xD8, Op_CLD },
                { 0xE5, Op_SBC },
                { 0xE8, Op_INX },
                { 0xF0, Op_BEQ }
            };
        }

        public uint Step()
        {
            uint cycles = 0;

            byte opcode;

            opcode = m_map.Read(m_pc);

            //http://homepage.ntlworld.com/cyborgsystems/CS_Main/6502/6502.htm

            if (!m_opcode_methods.ContainsKey(opcode))
            {
                // dump memory
                //StringBuilder builder = new StringBuilder();
                //for (uint i = 0; i < 0xFFFF; i++)
                //{
                //    builder.AppendFormat("0x{0:X4} ", i);
                //    for (uint j = 0; j < 0x0F; j++)
                //    {
                //        builder.AppendFormat("0x{0:X2} ", m_map.Read(i));
                //        i++;
                //    }
                //    builder.Append("\r\n");
                //}
                //Logger(builder.ToString());

                throw new NotImplementedException(string.Format("Unimplemented OPCODE 0x{0:X2} at 0x{1:X4}", opcode, m_pc));
            }

            byte?[] bytes = new byte?[3];
            for (byte b = 0; b < 3; b++)
            {
                if (b < m_opcode_lengths[opcode])
                {
                    bytes[b] = m_map.Read(m_pc + b);
                }
                else
                {
                    bytes[b] = null;
                }
            }

            Logger(string.Format("0x{0:X4} 0x{1:X2} {2} {3} ; {4} {5}",
                m_pc,
                bytes[0],
                bytes[1] != null ? string.Format("0x{0:X2}", bytes[1]) : "    ",
                bytes[2] != null ? string.Format("0x{0:X2}", bytes[2]) : "    ",
                m_opcode_names[opcode],
                m_opcode_modes[opcode]));

            uint? address = null;
            switch(m_opcode_modes[opcode])
            {
                case AddressMode.Implied:
                    // do nothing
                    break;

                case AddressMode.Accumulator:
                    // do nothing
                    break;

                case AddressMode.Absolute:
                    address = Read_Absolute();
                    break;

                case AddressMode.AbsoluteX:
                    address = Read_AbsoluteX();
                    break;

                case AddressMode.AbsoluteY:
                    address = Read_AbsoluteY();
                    break;

                case AddressMode.IndirectY:
                    address = Read_IndirectY();
                    break;

                case AddressMode.Immediate:
                    address = Read_Immediate();
                    break;

                case AddressMode.Relative:
                    address = Read_Relative();
                    break;

                case AddressMode.ZeroPage:
                    address = Read_ZeroPage();
                    break;

                case AddressMode.ZeroPageX:
                    address = Read_ZeroPageX();
                    break;

                case AddressMode.ZeroPageY:
                    address = Read_ZeroPageY();
                    break;

                default:
                    throw new NotImplementedException(string.Format("Unimplemented Addressing Mode {0}", m_opcode_modes[opcode].ToString()));
            }

            // jsr
            if (opcode == 0x20)
            {
                m_pc += m_opcode_lengths[opcode];
            }

            m_opcode_methods[opcode](address);

            cycles = m_opcode_cycles[opcode];

            // not jsr or rts
            if (opcode != 0x20 && opcode != 0x60)
            {
                m_pc += m_opcode_lengths[opcode];
            }

            Logger("CPU\tA: 0x{0:X2}\tP: 0x{1:X2}\tX: 0x{2:X2}\tY: 0x{3:X2}\tSP:0x{4:X4}", m_reg_a, m_reg_p, m_reg_x, m_reg_y, m_sp);

            return cycles;
        }

        // Address Read

        private uint Read_Relative()
        {
            sbyte offset = (sbyte)m_map.Read(m_pc + 1);

            uint address = (uint)(m_pc + offset);

            Logger("ADDRESS 0x{0:X4}", address);

            return address;
        }

        private uint Read_Immediate()
        {
            uint address = m_pc + 1;

            Logger("ADDRESS 0x{0:X4}", address);

            return address;
        }

        private uint Read_ZeroPage()
        {
            uint address = m_map.Read(m_pc);

            Logger("ADDRESS 0x{0:X4}", address);

            return address;
        }

        private uint Read_ZeroPageX()
        {
            uint address = m_map.Read(m_pc);
            address += m_reg_x;

            Logger("ADDRESS 0x{0:X4}", address);

            return address;
        }

        private uint Read_ZeroPageY()
        {
            uint address = m_map.Read(m_pc);
            address += m_reg_y;

            Logger("ADDRESS 0x{0:X4}", address);

            return address;
        }

        private uint Read_IndirectY()
        {
            byte lo_address = m_map.Read(m_pc+1);
            uint lo = m_map.Read(lo_address);

            byte hi_address = (byte)(lo_address + 0x01);
            uint hi = ((uint)m_map.Read(hi_address) << 8);

            uint address = (hi | lo) + m_reg_y;

            Logger("ADDRESS 0x{0:X4}", address);

            return address;
        }

        private uint Read_Absolute()
        {
            uint address = m_map.Read(m_pc + 1);
            address |= ((uint)m_map.Read(m_pc + 2) << 8);

            Logger("ADDRESS 0x{0:X4}", address);

            return address;
        }

        private uint Read_AbsoluteX()
        {
            uint address = m_map.Read(m_pc + 1);
            address |= ((uint)m_map.Read(m_pc + 2) << 8);
            address += m_reg_x;

            Logger("ADDRESS 0x{0:X4}", address);

            return address;
        }

        private uint Read_AbsoluteY()
        {
            uint address = m_map.Read(m_pc + 1);
            address |= ((uint)m_map.Read(m_pc + 2) << 8);
            address += m_reg_y;

            Logger("ADDRESS 0x{0:X4}", address);

            return address;
        }

        // 

        public string Disassemble(uint origin)
        {
            string[] assembly = new string[0x8000];

            for (uint i = origin; i < origin+0x8000; i++)
            {
                if (assembly[i-0x8000] == null)
                {
                    byte opcode = m_map.Read(i);

                    string text = string.Empty;

                    switch (opcode)
                    {
                        default:
                            text = string.Format("0x{0:X4} Unhandled OpCode 0x{1:X2}", i, opcode);
                            break;
                    }

                    assembly[i - 0x8000] = text;
                }
            }

            StringBuilder output = new StringBuilder();
            foreach (string line in assembly)
            {
                if(line != null)
                {
                    output.AppendFormat("{0}\r\n", line);
                }
            }
            return output.ToString();
        }

        // A

        private void Op_ADC(uint? address)
        {
            byte m = m_map.Read(address.Value);

            //t = A + M + P.C
            int t = m_reg_a + m;
            if (Flag_C)
            {
                t += 1;
            }

            //P.V = (A.7 != t.7) ? 1:0
            Flag_V = (m_reg_a & 0x80) != (t & 0x80);

            //P.N = A.7
            FlagN(m_reg_a);

            //P.Z = (t == 0) ? 1 : 0
            FlagZ((byte)t);

            //IF(P.D)
            if (Flag_D)
            {
                throw new NotImplementedException("BCD");
                //t = bcd(A) + bcd(M) + P.C
                //P.C = (t > 99) ? 1 : 0
            }
            else
            {
                //P.C = (t > 255) ? 1 : 0
                Flag_C = (t > 255);
            }

            //A = t & 0xFF
            m_reg_a = (byte)t;
        }

        private void Op_AND(uint? address)
        {
            byte b = m_map.Read(address.Value);
            m_reg_a = (byte)(m_reg_a & b);
            FlagN(m_reg_a);
            FlagZ(m_reg_a);
        }

        private void Op_ASL(uint? address)
        {
            if (address.HasValue)
            {
                throw new NotImplementedException("ASL");
            }
            else
            {
                Flag_C = (m_reg_a & 0x80) == 0x80;
                m_reg_a = (byte)(m_reg_a << 1);
                FlagN(m_reg_a);
                FlagZ(m_reg_a);
            }
        }

        // B

        private void Op_BCC(uint? address)
        {
            if (!Flag_C)
            {
                m_pc = address.Value;
            }
        }

        private void Op_BCS(uint? address)
        {
            if (Flag_C)
            {
                m_pc = address.Value;
            }
        }

        private void Op_BEQ(uint? address)
        {
            if (Flag_Z)
            {
                m_pc = address.Value;
            }
        }

        private void Op_BNE(uint? address)
        {
            if (!Flag_Z)
            {
                m_pc = address.Value;
            }
        }

        private void Op_BPL(uint? address)
        {
            if (!Flag_N)
            {
                m_pc = address.Value;
            }
        }

        // C

        private void Op_CLC(uint? address)
        {
            Flag_C = false;
        }

        private void Op_CLD(uint? address)
        {
            Flag_D = false;
        }

        private void Op_CMP(uint? address)
        {
            byte b = m_map.Read(address.Value);
            // N Flag only valid if signed comparison... how do we know if it is signed? signed flag?
            if (m_reg_a < b)
            {
                Flag_N = true;
                Flag_C = false;
                Flag_Z = false;
            }
            else if (m_reg_a == b)
            {
                Flag_N = false;
                Flag_C = true;
                Flag_Z = true;
            }
            else
            {
                Flag_N = false;
                Flag_C = true;
                Flag_Z = false;
            }
        }

        // D

        private void Op_DEC(uint? address)
        {
            byte b = m_map.Read(address.Value);
            b -= 1;
            FlagZ(b);
            FlagN(b);

            m_map.Write(address.Value, b);
        }

        private void Op_DEX(uint? address)
        {
            m_reg_x -= 1;
            FlagZ(m_reg_x);
            FlagN(m_reg_x);
        }

        private void Op_DEY(uint? address)
        {
            m_reg_y -= 1;
            FlagZ(m_reg_y);
            FlagN(m_reg_y);
        }

        // E

        private void Op_EOR(uint? address)
        {
            m_reg_a = (byte)(m_reg_a ^ m_map.Read(address.Value));
            FlagN(m_reg_a);
            FlagZ(m_reg_a);
        }

        // I

        private void Op_INX(uint? address)
        {
            m_reg_x++;

            FlagZ(m_reg_x);
            FlagN(m_reg_x);
        }

        private void Op_INY(uint? address)
        {
            m_reg_y++;

            FlagZ(m_reg_y);
            FlagN(m_reg_y);
        }

        // J

        private void Op_JSR(uint? address)
        {
            byte hi = (byte)((m_pc - 1) >> 8);
            Stack_Push(hi);
            byte lo = (byte)(m_pc - 1);
            Stack_Push(lo);

            m_pc = address.Value;
        }

        // L

        private void Op_LDA(uint? address)
        {
            m_reg_a = m_map.Read(address.Value);
            
            FlagN(m_reg_a);
            FlagZ(m_reg_a);
        }

        private void Op_LDX(uint? address)
        {
            m_reg_x = m_map.Read(address.Value);

            FlagN(m_reg_x);
            FlagZ(m_reg_x);
        }

        private void Op_LDY(uint? address)
        {
            m_reg_y = m_map.Read(address.Value);

            FlagN(m_reg_y);
            FlagZ(m_reg_y);
        }

        private void Op_LSR(uint? address)
        {
            if (address.HasValue)
            {
                byte b = m_map.Read(address.Value);

                Flag_N = false;
                byte b0 = (byte)(b & 0x01);
                Flag_C = b0 == 0x01;
                b = (byte)((b >> 1) & 0x7F);
                FlagZ(b);

                m_map.Write(address.Value, b);
            }
            else
            {
                byte b = m_reg_a;
                Flag_N = false;
                byte b0 = (byte)(b & 0x01);
                Flag_C = b0 == 0x01;
                b = (byte)((b >> 1) & 0x7F);
                FlagZ(b);

                m_reg_a = b;
            }
        }

        // O

        private void Op_ORA(uint? address)
        {
            m_reg_a = (byte)(m_reg_a | m_map.Read(address.Value));
            FlagN(m_reg_a);
            FlagZ(m_reg_a);
        }

        // P

        private void Op_PHA(uint? address)
        {
            Stack_Push(m_reg_a);
        }

        private void Op_PHP(uint? address)
        {
            Stack_Push(m_reg_p);
        }

        private void Op_PLA(uint? address)
        {
            m_reg_a = Stack_Pull();
            FlagN(m_reg_a);
            FlagZ(m_reg_a);
        }

        private void Op_PLP(uint? address)
        {
            m_reg_p = Stack_Pull();
        }

        // R

        private void Op_ROR(uint? address)
        {
            if (address.HasValue)
            {
                byte b = m_map.Read(address.Value);
                byte b0 = (byte)(b & 0x01);
                b = (byte)((b >> 1) & 0x7F);
                b = (byte)(b | (Flag_C ? 0x80 : 0x00));
                Flag_C = b0 == 0x01;
                FlagZ(b);
                FlagN(b);

                m_map.Write(address.Value, b);
            }
            else
            {
                byte b = m_reg_a;
                byte b0 = (byte)(b & 0x01);
                b = (byte)((b >> 1) & 0x7F);
                b = (byte)(b | (Flag_C ? 0x80 : 0x00));
                Flag_C = b0 == 0x01;
                FlagZ(b);
                FlagN(b);

                m_reg_a = b;
            }
        }

        private void Op_RTS(uint? address)
        {
            //SP = SP + 1
            //l = bPeek(SP)
            byte l = Stack_Pull();
            //SP = SP + 1
            //h = bPeek(SP) << 8
            uint h = (uint)(Stack_Pull() << 8);
            //PC = (h | l) + 1
            m_pc = (h | l) + 1;
        }

        // S

        private void Op_SBC(uint? address)
        {
            byte m = m_map.Read(address.Value);

            int t;

            //IF(P.D)
            if (Flag_D)
            {
                throw new NotImplementedException("BCD");
                //  t = bcd(A) - bcd(M) - !P.C
                //  P.V = (t > 99 OR t< 0) ? 1:0
            }
            else
            {
                //  t = A - M - !P.C
                t = m_reg_a - m;
                if (!Flag_C)
                {
                    t -= 1;
                }
                //  P.V = (t > 127 OR t < -128) ? 1:0
                Flag_V = (t > 127 || t < -128);
            }
            //P.C = (t >= 0) ? 1 : 0
            Flag_C = t >= 0;
            //P.N = t.7
            FlagN((byte)t);
            //P.Z = (t == 0) ? 1 : 0
            FlagZ((byte)t);
            //A = t & 0xFF
            m_reg_a = (byte)t;
        }

        private void Op_SEC(uint? address)
        {
            Flag_C = true;
        }

        private void Op_SEI(uint? address)
        {
            Flag_I = true;
        }

        private void Op_STA(uint? address)
        {
            // M = A
            m_map.Write(address.Value, m_reg_a);
        }

        private void Op_STX(uint? address)
        {
            // M = X
            m_map.Write(address.Value, m_reg_x);
        }

        private void Op_STY(uint? address)
        {
            // M = Y
            m_map.Write(address.Value, m_reg_y);
        }

        // T

        private void Op_TAX(uint? address)
        {
            m_reg_x = m_reg_a;
            FlagN(m_reg_x);
            FlagZ(m_reg_x);
        }

        private void Op_TYA(uint? address)
        {
            m_reg_a = m_reg_y;
            FlagN(m_reg_x);
            FlagZ(m_reg_x);
        }

        private void Op_TXA(uint? address)
        {
            m_reg_a = m_reg_x;
        }

        private void Op_TXS(uint? address)
        {
            m_sp = m_reg_x;
        }

        // STACK

        private void Stack_Push(byte data)
        {
            uint address = (uint)(0x0100 | m_sp);
            Logger("STACK 0x{0:X4}", address);
            m_map.Write(address, data);
            m_sp--;
        }

        private byte Stack_Pull()
        {
            m_sp++;
            uint address = (uint)(0x0100 | m_sp);
            Logger("STACK 0x{0:X4}", address);
            return m_map.Read(address);
        }

        // ILLEGAL

        private void Op_ILLEGAL()
        {
            throw new Exception(string.Format("Illegal Opcode 0x{0:X2}", m_map.Read(m_pc)));
        }

        // FLAGS

        /// <summary>
        /// P.N = data.7 
        /// </summary>
        /// <param name="data"></param>
        private void FlagN(byte data)
        {
            Flag_N = (0x80 & data) == 0x80;
        }
        private bool Flag_N
        {
            get
            {
                return (m_reg_p & 0x80) == 0x80;
            }

            set
            {
                if (value)
                {
                    m_reg_p |= 0x80;
                }
                else
                {
                    m_reg_p &= 0x7F;
                }
            }
        }
        
        private bool Flag_V
        {
            get
            {
                return (m_reg_p & 0x40) == 0x40;
            }

            set
            {
                if (value)
                {
                    m_reg_p |= 0x40;
                }
                else
                {
                    m_reg_p &= 0xBF;
                }
            }
        }

        // 0x20

        // 0x10
        
        private bool Flag_D
        {
            get
            {
                return (m_reg_p & 0x08) == 0x08;
            }

            set
            {
                if (value)
                {
                    m_reg_p |= 0x08;
                }
                else
                {
                    m_reg_p &= 0xF7;
                }
            }
        }
        
        private bool Flag_I
        {
            get
            {
                return (m_reg_p & 0x04) == 0x04;
            }

            set
            {
                if (value)
                {
                    m_reg_p |= 0x04;
                }
                else
                {
                    m_reg_p &= 0xFB;
                }
            }
        }

        /// <summary>
        /// P.Z = (data==0) ? 1:0
        /// </summary>
        /// <param name="data"></param>
        private void FlagZ(byte data)
        {
            Flag_Z = (data == 0x00);
        }
        private bool Flag_Z
        {
            get
            {
                return (m_reg_p & 0x02) == 0x02;
            }

            set
            {
                if (value)
                {
                    m_reg_p |= 0x02;
                }
                else
                {
                    m_reg_p &= 0xFD;
                }
            }
        }
        
        private bool Flag_C
        {
            get
            {
                return (m_reg_p & 0x01) == 0x01;
            }

            set
            {
                if (value)
                {
                    m_reg_p |= 0x01;
                }
                else
                {
                    m_reg_p &= 0xFE;
                }
            }
        }
    }
}
