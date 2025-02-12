﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NES
{
    /// <summary>
    /// Класс для переключения банков памяти 
    /// </summary>
    public class Mapper
    {
        /// <summary>
        /// Функцмя записи в Mapper 
        /// </summary>
        /// <param name="adr">Адрес</param>
        /// <param name="val">Значение</param>
        delegate void MapWrite(ushort adr, byte val);

        static MapWrite[] mappers = new MapWrite[]
        {
          NROM,
          MMC1.Write
        };
        /// <summary>
        /// Инициализация Mapper
        /// </summary>
        public static void Init() // Крюков Никита
        {
           Memory.WriteROM1(Cartridge.GetPrgBank(0));
           Memory.WriteROM2(Cartridge.GetPrgBank(Cartridge.prg_count - 1));
        }

        /// <summary>
        /// Записать значения в Mapper
        /// </summary>
        /// <param name="adr">Адрес</param>
        /// <param name="val">Значнение</param>
        public static void Write(ushort adr, byte val)
	{
            mappers[Cartridge.mapper](adr, val);
	    
	}

	static void NROM(ushort adr, byte val)
	{
            throw new Exception(" NROM не поддерживает переключения ");
	}
        
    }
}
