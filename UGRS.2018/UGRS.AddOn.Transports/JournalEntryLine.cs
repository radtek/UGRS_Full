using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Transports;
using UGRS.Core.SDK.DI.Transports.DTO;

namespace UGRS.AddOn.Transports
{
    class JournalEntryLine
    {
        TransportServiceFactory mObjTransportsFactory = new TransportServiceFactory();
        //Creacion de lineas de asiento por algoritmo
        public List<JournalLineDTO> CreateJournalEntryLine(CommissionDriverDTO pObjCmsnDriverDTO, AccountsJournalEntryDTO pObjAccounts)
        {
            List<JournalLineDTO> lLstJounralLineDTO = new List<JournalLineDTO>();
            if (Convert.ToDecimal(pObjCmsnDriverDTO.TotComm) > 0 || Convert.ToDecimal(pObjCmsnDriverDTO.Doubt) > 0)
            {
                decimal lDecTotal = Convert.ToDecimal(pObjCmsnDriverDTO.TotComm) - Convert.ToDecimal(pObjCmsnDriverDTO.Doubt);
                decimal lDecDeuda = Convert.ToDecimal(pObjCmsnDriverDTO.LstDisc);
                decimal lDecDeuSem = Convert.ToDecimal(pObjCmsnDriverDTO.WkDisc);
                decimal lDecComision = Convert.ToDecimal(pObjCmsnDriverDTO.Comm);
                decimal lDecRemanente = 0;
                decimal lDecSemD = 0;

                if (lDecDeuda > 0 && lDecComision > 0)
                {
                    lDecRemanente = lDecComision;
                    //pObjCmsnDriverDTO.ListDebt =  pLstListDebt;// GetCommissionDebt(pObjCmsnDriverDTO.Folio, pObjCmsnDriverDTO.DriverId);
                    foreach (CommissionDebtDTO DebtDTO in pObjCmsnDriverDTO.ListDebt)
                    {
                        if (lDecRemanente <= 0)
                        {
                            break;
                        }
                        else
                        {
                            //if (DebtDTO.Importe <= lDecRemanente)
                            if (DebtDTO.Importe > lDecRemanente)
                            {
                                lLstJounralLineDTO.Add(new JournalLineDTO
                                {
                                    AccountCode = pObjAccounts.AccountFuncEmpl,
                                    Debit = 0,
                                    Credit = Convert.ToDouble(DebtDTO.Importe - lDecRemanente),
                                    TypeAux = "2",
                                    Auxiliar = pObjCmsnDriverDTO.DriverId,
                                    Ref1 = DebtDTO.Folio,
                                    CostingCode = "TR_TRANS",
                                });
                                lDecRemanente -= DebtDTO.Importe;
                            }
                            else
                            {
                                lLstJounralLineDTO.Add(new JournalLineDTO
                                {
                                    AccountCode = pObjAccounts.AccountFuncEmpl,
                                    Debit = 0,
                                    Credit = Convert.ToDouble(DebtDTO.Importe),//Convert.ToDouble(lDecRemanente),
                                    Ref1 = DebtDTO.Folio,
                                    TypeAux = "2",
                                    Auxiliar = pObjCmsnDriverDTO.DriverId,
                                    CostingCode = "TR_TRANS",
                                });
                                lDecRemanente = 0;
                            }

                        }
                    }
                }

                if (lDecTotal > 0)
                {
                    lLstJounralLineDTO.Add(new JournalLineDTO
                    {
                        AccountCode = pObjAccounts.AccountLiquid,
                        Debit = 0,
                        Credit = Convert.ToDouble(lDecTotal),
                        Ref1 = pObjCmsnDriverDTO.Folio,
                        TypeAux = "2",
                        Auxiliar = pObjCmsnDriverDTO.DriverId,
                        CostingCode = "TR_TRANS",
                    });
                }
                else if (lDecDeuda > 0)
                {
                    lDecSemD = lDecDeuSem - lDecRemanente;

                    lLstJounralLineDTO.Add(new JournalLineDTO
                    {
                        AccountCode = pObjAccounts.AccountFuncEmpl,
                        Debit = Convert.ToDouble(lDecSemD),
                        Credit = 0,
                        Ref1 = pObjCmsnDriverDTO.Folio,
                        TypeAux = "2",
                        Auxiliar = pObjCmsnDriverDTO.DriverId,
                        CostingCode = "TR_TRANS",
                    });
                    // lDecTotal = lDecTotal - 
                }
                else
                {
                    decimal lDec = lDecTotal < 0 ? -1 : 1;
                    lLstJounralLineDTO.Add(new JournalLineDTO
                    {
                        AccountCode = pObjAccounts.AccountFuncEmpl,
                        Debit = Convert.ToDouble(lDecTotal * lDec + lDecRemanente),
                        Credit = 0,
                        Ref1 = pObjCmsnDriverDTO.Folio,
                        TypeAux = "2",
                        Auxiliar = pObjCmsnDriverDTO.DriverId,
                        CostingCode = "TR_TRANS",
                    });
                }

                lDecTotal = Convert.ToDecimal(lLstJounralLineDTO.Sum(x => x.Credit)) - Convert.ToDecimal(lLstJounralLineDTO.Sum(x => x.Debit));

                if (lDecTotal >= pObjAccounts.Tope || lDecTotal <= pObjAccounts.Tope * -1)
                {
                    decimal lDec = lDecTotal < 0 ? -1 : 1;

                    lLstJounralLineDTO.Add(new JournalLineDTO
                    {
                        AccountCode = pObjAccounts.AccountViat,
                        Debit = Convert.ToDouble(pObjAccounts.Tope * lDec),
                        Credit = 0,
                        Ref1 = pObjCmsnDriverDTO.Folio,
                        TypeAux = "2",
                        Auxiliar = pObjCmsnDriverDTO.DriverId,
                        CostingCode = "TR_TRANS",
                    });

                    lLstJounralLineDTO.Add(new JournalLineDTO
                    {
                        AccountCode = pObjAccounts.AccountRepMen,
                        Debit = Convert.ToDouble((lDecTotal) - (pObjAccounts.Tope * lDec)),
                        Credit = 0,
                        Ref1 = pObjCmsnDriverDTO.Folio,
                        TypeAux = "2",
                        Auxiliar = pObjCmsnDriverDTO.DriverId,
                        CostingCode = "TR_TRANS",
                    });
                }
                else
                {
                    decimal lDec = lDecTotal < 0 ? -1 : 1;
                    lLstJounralLineDTO.Add(new JournalLineDTO
                    {
                        AccountCode = pObjAccounts.AccountViat,
                        Debit = Convert.ToDouble(lDecTotal),
                        Credit = 0,
                        Ref1 = pObjCmsnDriverDTO.Folio,
                        TypeAux = "2",
                        Auxiliar = pObjCmsnDriverDTO.DriverId,
                        CostingCode = "TR_TRANS",
                    });
                }
            }
            lLstJounralLineDTO = AddAsset(lLstJounralLineDTO, pObjCmsnDriverDTO);
            return lLstJounralLineDTO;
        }

        private List<JournalLineDTO> AddAsset(List<JournalLineDTO> pLstJELine, CommissionDriverDTO pObjInvoices)
        {
            List<CommissionDriverDTO> lLstCmsDriverDTO = new List<CommissionDriverDTO>();
            lLstCmsDriverDTO = mObjTransportsFactory.GetCommissionService().GetCommissionDriverSaved(pObjInvoices.Folio);
            foreach (JournalLineDTO lObjLine in pLstJELine)
            {
                lLstCmsDriverDTO = lLstCmsDriverDTO.Where(x => x.DriverId == lObjLine.Auxiliar).GroupBy(g => g.AF).Select( y => new CommissionDriverDTO {
                    DriverId = y.First().DriverId,
                    AF = y.First().AF,
                    Comm = y.Sum(s => s.Comm),
                }).ToList();

                if (lLstCmsDriverDTO.Count > 0)
                {
                    lObjLine.CostingCode2 = mObjTransportsFactory.GetCommissionService().GetAsset(lLstCmsDriverDTO.OrderBy(x => x.TotComm).Take(1).First().AF);
                }
            }
            return pLstJELine;
        }

    }
}
