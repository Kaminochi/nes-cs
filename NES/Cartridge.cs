﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NES
{
    /// <summary>
    /// класс картриджей
    /// </summary>
    public class Cartridge
    {
        /// <summary>
        /// число банков программ
        /// </summary>
        public static int prg_count;

        /// <summary>
        /// число банков изображений
        /// </summary>
        public static int chr_count;

        /// <summary>
        /// зеркальное отображение: горизонтальное (вертикальное расположение)
        /// </summary>
        public static bool mirroring;

        /// <summary>
        /// если значение true, то трейнер присутствует
        /// </summary>
        public static bool trainer;

        /// <summary>
        /// картридж содержит RPG RAM с батарейным питанием или другую постоянную память
        /// </summary>
        public static bool prg_ram;

        /// <summary>
        /// Игнорировать управление зеркалированием или бит выше зеркалирования
        /// </summary>
        public static bool ignore_mirroring;

        /// <summary>
        /// номер маппера
        /// </summary>
        public static int mapper;

        /// <summary>
        /// память программ
        /// </summary>
        static byte[] prg_mem;

        /// <summary>
        /// память изображений
        /// </summary>
        static byte[] chr_mem;

        /// <summary>
        /// память трейнера
        /// </summary>
        static byte[] trainer_mem;

        /// <summary>
        /// получить банк программы
        /// </summary>
        /// <param name="n">номер банка</param>
        /// <returns>банк программы</returns>
        public static byte[] GetPrgBank(int n)
        {
            if (n < 0 || n >= prg_count)
		return null;
            return prg_mem.Skip<byte>(n * Memory.PRG_SIZE).Take(Memory.PRG_SIZE).ToArray();
        }

        /// <summary>
        /// получить банк изображений
        /// </summary>
        /// <param name="n">номер банка</param>
        /// <returns>банк изображений</returns>
        public static byte[] GetChrBank(int n)
        {
            if (n < 0 || n >= chr_count)
		return null;
            return chr_mem.Skip<byte>(n * Memory.CHR_SIZE).Take(Memory.CHR_SIZE).ToArray();
        }

        /// <summary>
        /// прочитать файл картриджа
        /// </summary>
        /// <param name="fileName">имя файла</param>
        /// <returns>true - если правильный NES файл, иначе false</returns>
        public static bool ReadFile(string fileName) // Пилипенко Никита
        {
	        byte[] Header = { 0x4E, 0x45, 0x53, 0x1A };
            byte[] header;
            using (var stream = File.Open(fileName, FileMode.Open))
	        {
		        using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
		        {
		            header = reader.ReadBytes(16);
		            for (int i = 0; i < 4; i++)
			            if (header[i] != Header[i])
			                return false;
                    prg_count = header[4];
                    prg_mem = new byte[prg_count * Memory.PRG_SIZE];
                    chr_count = header[5];
                    chr_mem = new byte[chr_count * Memory.CHR_SIZE];
                    mirroring = (header[6] & 0x01) != 0;
                    prg_ram = (header[6] & 0x02) != 0;
                    trainer = (header[6] & 0x04) != 0;
                    ignore_mirroring = (header[6] & 0x08) != 0;
		    // trainer ???
                    prg_mem = reader.ReadBytes(prg_count * Memory.PRG_SIZE);
		    // chr_mem ???
                }                                            
            }
            return true;
        }
    }
}
