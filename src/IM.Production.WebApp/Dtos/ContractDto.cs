using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IM.Production.WebApp.Dtos
{
    public class ContractDto
    {
        /// <summary>
        /// Общая сумма на закупку/продажу, прошедшую по контракту.
        /// </summary>
        public decimal TotalSumm { get; set; }

        /// <summary>
        /// Количество материала, уже поставленное по контракту.
        /// </summary>
        public int TotalCountComplete { get; set; }

        /// <summary>
        /// Если указано, то контракт действует до поставки определенного количества материала.
        /// </summary>
        public int TillCount { get; set; }

        /// <summary>
        /// Если указано, то контракт действует до указанной даты.
        /// </summary>
        public int TillDate { get; set; }

        /// <summary>
        /// Source Cusomer info
        /// </summary>
        public string SourceCustomerLogin { get; set; }
        public string SourceFactoryName { get; set; }
        public int SourceGenerationLevel { get; set; }
        public int SourceWorkers { get; set; }

        /// <summary>
        /// Destination Cusomer info
        /// </summary>
        public string DestinationCustomerLogin { get; set; }
        public string DestinationFactoryName { get; set; }
        public int DestinationGenerationLevel { get; set; }
        public int DestinationWorkers { get; set; }
    }
}