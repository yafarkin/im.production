using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IM.Production.WebApp.Dtos
{
    public class ContractDto
    {
        /// <summary>
        /// Позиция контракта в таблице
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// Если указано, то контракт действует до указанной даты.
        /// </summary>
        public int TillDate { get; set; }

        /// <summary>
        /// Если указано, то контракт действует до поставки определенного количества материала.
        /// </summary>
        public int TillCount { get; set; }
        
        /// <summary>
        /// Общая сумма на закупку/продажу, прошедшую по контракту.
        /// </summary>
        public decimal TotalSumm { get; set; }

        /// <summary>
        /// Исходная фабрика, если не задано - поставляется игрой.
        /// </summary>
        public string SourceFactoryCustomerLogin { get; set; }

        /// <summary>
        /// Фабрика назначения, если не задано - продается игре.
        /// </summary>
        public string DestinationFactoryCustomerLogin { get; set; }
    }
}
