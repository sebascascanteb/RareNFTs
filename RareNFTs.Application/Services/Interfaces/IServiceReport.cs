using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Application.Services.Interfaces;

public interface IServiceReport
{
    Task<byte[]> ProductReport();

    Task<byte[]> ClientReport();

    Task<byte[]> SalesReport(DateTime startDate, DateTime endDate);
}
