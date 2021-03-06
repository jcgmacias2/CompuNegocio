using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IMainView : IBaseView
    {
        //MT: A la par del desarrollo de las Vistas habrá que crear sus eventos aquí
        event Action OpenSignIn;
        event Action OpenTaxes;
        event Action OpenUnitsOfMeasure;
        event Action OpenItems;
        event Action OpenUsers;
        event Action OpenClients;
        event Action OpenClassifications;
        event Action Quit;
        event Action OpenPrices;
        event Action SignOut;
        event Action OpenPaymentMethods;
        event Action OpenSuppliers;
        event Action OpenPurchases;
        event Action OpenPurchasePayments;
        event Action OpenGeneralProperties;
        event Action OpenBusinesses;
        event Action OpenStations;
        event Action OpenSales;
        event Action OpenInvoices;
        event Action OpenFiscalPayments;
        event Action OpenDeposits;
        event Action OpenBillsOfSale;
        event Action OpenPaymentsByPeriod;
        event Action OpenRegistersCashOut;
        event Action OpenRegistersCashOutSummary;
        event Action OpenAdjustments;
        event Action OpenStockReport;
        event Action OpenAdjustmentsReport;
        event Action OpenInvoiceReport;
        event Action OpenDatabaseConfig;
        event Action OpenPurchasesByPeriodReport;
        event Action OpenPayableBalancesReport;
        event Action OpenSupplierStatementReport;
        event Action OpenStockFlowReport;
        event Action OpenKardexReport;
        event Action OpenItemsTransfer;
        event Action OpenCollectableBalancesReport;
        event Action OpenClientStatementReport;
        event Action OpenMigrationTools;
        event Action OpenBanks;
        event Action OpenBankAccounts;
        event Action OpenProductsServices;
        event Action OpenCFDIUses;
        event Action OpenPropertyAccounts;
        event Action OpenFiscalPaymentReport;
        event Action OpenGuardianManualSend;
        event Action OpenHomologationTool;
        event Action OpenQuotes;
        event Action OpenBillsOfSalePerPeriodReport;
        event Action OpenSalesPerPeriodReport;
        event Action OpenCommissionsPerPeriodReport;
        event Action OpenOrders;
        event Action OpenInvoiceBillsOfSale;
        event Action OpenOrdersReport;
        event Action OpenPurchaseOrders;
        event Action OpenClientPayments;
        event Action OpenQuotesPerPeriodReport;
        event Action OpenAssociatedCompanies;
        event Action OpenTransfers;
        event Action OpenTransferRequestsList;
        event Action OpenTransfersByPeriodReport;
        event Action OpenSalesPerItemReport;
        event Action OpenCreditNotes;
        event Action OpenCreditNotesByPeriodReport;
        event Action OpenDiscountNotes;
        event Action OpenTaxesByPeriodReport;
        event Action OpenAppraisalReport;
        event Action OpenCompanyStatusReport;
        event Action OpenSoldItemsCostReport;
        event Action OpenPriceListsReport;
        event Action OpenDiscountNotesReport;

        void HideWindow();
    }
}
