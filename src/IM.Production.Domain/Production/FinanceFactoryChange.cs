using System;

namespace Epam.ImitationGames.Production.Domain.Production
{
    [Serializable]
    public class FinanceFactoryChange : FinanceCustomerChange
    {
        /// <summary>
        /// С какой фабрикой связаны изменения.
        /// </summary>
        public Factory Factory { get; set; }

        public decimal OnRd { get; set; }

        public decimal OnTax { get; set; }

        public decimal OnSalary { get; set; }

        public FinanceFactoryChange(Factory factory, decimal onRd, decimal onTax, decimal onSalary, string description = null) :
            base(factory.Customer, onRd + onTax + onSalary, description)
        {
            Factory = factory;
            OnRd = onRd;
            OnTax = onTax;
            OnSalary = onSalary;
        }

        public override void DoAction()
        {
            base.DoAction();

            Factory.TotalOnRD += OnRd;
            Factory.TotalOnTaxes += OnTax;
            Factory.TotalOnSalary += OnSalary;
        }
    }
}