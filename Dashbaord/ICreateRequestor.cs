using GravitonLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dashbaord
{
    public interface ICreateRequestor
    {
        void LedgerClicked(LedgerModel model);
        void CategoryClicked(CostCategoryModel model);
        void GroupClicked(GroupModel model);
        void CostCenterClicked(CostCenterModel model);
        void Create(int index);
        void Home(int index);
        void NewPayment();
        void NewReceipt();
    }
}
