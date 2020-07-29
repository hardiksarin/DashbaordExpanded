using GravitonLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dashbaord
{
    public interface IBillRequestor
    {
        void BillComplete(PaymentBill model);
    }
}
