using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IM.Production.WebApp.Dtos
{
    public class FactoryContractDto
    {
        /// Source Cusomer info
        public string SourceCustomerLogin { get; set; }
        /// Destination Cusomer info
        public string DestinationCustomerLogin {get; set; }
        /// Имя материала человеческим языком
        public string MaterialKey { get; set; }
        /// Количество материала
        public int TotalCountCompleted { get; set; }
        /// Общая сумма на закупку/продажу, прошедшую по контракту.
        public int TotalSumm { get; set; }
    }
}
