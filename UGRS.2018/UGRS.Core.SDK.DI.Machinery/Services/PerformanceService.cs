using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.SDK.DI.Machinery.Tables;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.Services
{
    public class PerformanceService
    {
        private TableDAO<Performance> mObjPerformanceTableDAO;

        public PerformanceService()
        {
            mObjPerformanceTableDAO = new TableDAO<Performance>();
        }

        #region DAO

        #endregion

        #region Entities
        public int Add(Performance pObjPerformance)
        {
            try
            {
                int result = mObjPerformanceTableDAO.Add(pObjPerformance);

                return result;
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[PerformanceService - Add]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public int Update(Performance pObjPerformance)
        {
            try
            {
                return mObjPerformanceTableDAO.Update(pObjPerformance);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[PerformanceService - Update]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public int Remove(string pStrCode)
        {
            try
            {
                int lIntResult = 0;
                if (!string.IsNullOrEmpty(pStrCode))
                {
                    lIntResult =  mObjPerformanceTableDAO.Remove(pStrCode);
                }

                return lIntResult;
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[PerformanceService - Remove]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        /*public IList<Performance> GetByRiseId(int pIntRiseId)
        {
            return new QueryManager().GetObjectsList<Performance>("U_IdRise", pIntRiseId.ToString(), string.Format("[@{0}]", mObjPerformanceTableDAO.GetUserTableName()));
        }*/

        public string GetLastCode()
        {
            try
            {
                return new QueryManager().Max<string>("Code", string.Format("[@{0}]", mObjPerformanceTableDAO.GetUserTableName()));
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[PerformanceService - GetLastCode]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public List<PerformanceDTO> GetByRiseId(int pIntRiseId)
        {
            try
            {
                List<Performance> lLstInitialRds = new QueryManager().GetObjectsList<Performance>("U_IdRise", pIntRiseId.ToString(), string.Format("[@{0}]", mObjPerformanceTableDAO.GetUserTableName())).ToList();

                return ToPerformanceDTO(lLstInitialRds);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[PerformanceService - GetByRiseId]: {0}", lObjException.Message));
                throw lObjException;
            }
        }
        #endregion

        #region Extras
        public List<PerformanceDTO> ToPerformanceDTO(List<Performance> pLstPerformance)
        {
            return pLstPerformance.Select(x => new PerformanceDTO
            {
                Code = x.RowCode,
                IdRise = x.IdRise,
                PrcCode = x.PrcCode,
                EcoNum = x.EcoNum,
                Type = x.Type,
                HrKm = x.HrKm,
                PerformanceF = x.PerformanceF,
            }).ToList();
        }

        public List<PerformanceDTO> CalculateMachineryPerformance(List<ConsumablesDocumentsDTO> pLstConsumables, List<HoursRecordsDTO> pLstHours, List<TransitHoursRecordsDTO> pLstTransitHours)
        {
            List<PerformanceDTO> lLstPerformance = new List<PerformanceDTO>();
            try
            {
                List<PerformanceDTO> lLstPerformanceConsm = pLstConsumables.GroupBy(p => new { ActiveCode = p.ActivoCode, EcoNum = p.EcoNum, EqType = p.EquipmentType })
                                                            .Select(g => new PerformanceDTO
                                                            {
                                                                PrcCode = g.Key.ActiveCode,
                                                                EcoNum = g.Key.EcoNum,
                                                                Type = 1,
                                                                HrKm = g.Sum(i => i.KmHr),
                                                                PerformanceF = g.Sum(i => i.DieselT) + g.Sum(i => i.DieselM),
                                                            }).ToList();

                List<PerformanceDTO> lLstPerformanceHrs = pLstHours.GroupBy(p => new { ActiveCode = p.PrcCode, EcoNum = p.EcoNum })
                                                            .Select(g => new PerformanceDTO
                                                            {
                                                                PrcCode = g.Key.ActiveCode,
                                                                EcoNum = g.Key.EcoNum,
                                                                Type = 1,
                                                                HrKm = 0,
                                                                PerformanceF = g.Sum(i => i.HrFeet),
                                                            }).ToList();

                List<PerformanceDTO> lLstPerformanceTransitHrs = pLstTransitHours.GroupBy(p => new { ActiveCode = p.PrcCode, EcoNum = p.EcoNum })
                                                            .Select(g => new PerformanceDTO
                                                            {
                                                                PrcCode = g.Key.ActiveCode,
                                                                EcoNum = g.Key.EcoNum,
                                                                Type = 1,
                                                                HrKm = 0,
                                                                PerformanceF = g.Sum(i => i.Hrs),
                                                            }).ToList();

                var lLstPerformanHrs = (from hours in lLstPerformanceHrs
                                        join transitHrs in lLstPerformanceTransitHrs
                                                on hours.PrcCode equals transitHrs.PrcCode
                                        into a
                                        from b in a.DefaultIfEmpty(new PerformanceDTO())
                                        select new PerformanceDTO
                                        {
                                            PrcCode = hours.PrcCode,
                                            EcoNum = hours.EcoNum,
                                            Type = 1,
                                            HrKm = 0,
                                            PerformanceF = hours.PerformanceF + b.PerformanceF,
                                        }).ToList();

                lLstPerformance = (from cons in lLstPerformanceConsm
                                   join hrs in lLstPerformanHrs
                                            on cons.PrcCode equals hrs.PrcCode
                                    into a
                                   from b in a.DefaultIfEmpty(new PerformanceDTO())
                                   select new PerformanceDTO
                                   {
                                       PrcCode = cons.PrcCode,
                                       EcoNum = cons.EcoNum,
                                       Type = 1,
                                       HrKm = b.PerformanceF,
                                       PerformanceF = b.PerformanceF == 0 ? 0 : cons.PerformanceF / b.PerformanceF,
                                   }).ToList();

                //var lLstPerformanHrs = from hours in pLstHours
                //                       join transitHrs in pLstTransitHours
                //                               on hours.PrcCode equals transitHrs.PrcCode
                //                       select new PerformanceDTO
                //                       {
                //                           PrcCode = hours.PrcCode,
                //                           EcoNum = hours.EcoNum,
                //                           Type = 1,
                //                           HrKm = hours.KmHt + transitHrs.Hrs,
                //                           PerformanceF = transitHrs.Hrs == 0 ? 0 : hours.HrFeet + transitHrs.Hrs,
                //                       };

                ////var lLstPerformanHrs = (from hours in pLstHours
                ////                       join transitHrs in pLstTransitHours
                ////                       on hours.PrcCode equals transitHrs.PrcCode into perf
                ////                       select new PerformanceDTO
                ////                       {
                ////                           PrcCode = hours.PrcCode,
                ////                           EcoNum = hours.EcoNum,
                ////                           Type = 1,
                ////                           HrKm = hours.KmHt + perf.FirstOrDefault().Hrs,
                ////                           PerformanceF = perf.FirstOrDefault().Hrs == 0 ? 0 : hours.HrFeet + perf.FirstOrDefault().Hrs,
                ////                       }).ToList();

                //var a = lLstPerformanHrs.ToList();

                //var lLstPerformanceTmp = from hours in lLstPerformanHrs
                //                      join consm in lLstPerformanceConsm
                //                               on hours.PrcCode equals consm.PrcCode
                //                       select new PerformanceDTO
                //                       {
                //                           PrcCode = hours.PrcCode,
                //                           EcoNum = hours.EcoNum,
                //                           Type = 1,
                //                           HrKm = consm.HrKm + hours.HrKm,
                //                           PerformanceF = hours.PerformanceF == 0 ? 0 : consm.PerformanceF / hours.PerformanceF,
                //                       };

                //lLstPerformance = lLstPerformanceTmp.GroupBy(x => new { ActiveCode = x.PrcCode, EcoNum = x.EcoNum })
                //                                            .Select(g => new PerformanceDTO
                //                                            {
                //                                                PrcCode = g.Key.ActiveCode,
                //                                                EcoNum = g.Key.EcoNum,
                //                                                Type = 1,
                //                                                HrKm = g.Sum(i => i.HrKm),
                //                                                PerformanceF = g.Sum(i => i.PerformanceF),
                //                                            }).ToList();
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[PerformanceService - CalculateMachineryPerformance]: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al calcular el rendimiento de las maquinarias: {0}", lObjException.Message));
            }

            return lLstPerformance;
        }

        public List<PerformanceDTO> CalculateVehiclePerformance(List<ConsumablesDocumentsDTO> pLstConsumablesTotals)
        {
            List<PerformanceDTO> lLstPerformance = new List<PerformanceDTO>();
            try
            {
                lLstPerformance = pLstConsumablesTotals.GroupBy(p => new { ActiveCode = p.ActivoCode, EcoNum = p.EcoNum, EqType = p.EquipmentType })
                                                                 .Select(g => new PerformanceDTO
                                                                 {
                                                                     PrcCode = g.Key.ActiveCode,
                                                                     EcoNum = g.Key.EcoNum,
                                                                     Type = 2,
                                                                     HrKm = g.Sum(i => i.KmHr),
                                                                     PerformanceF = g.Sum(i => i.KmHr) == 0 ? 0 : (g.Sum(i => i.Gas) == 0 && g.Sum(i => i.KmHr) == 0) ? 0 : g.Sum(i => i.KmHr) / g.Sum(i => i.Gas),
                                                                 }).ToList();
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[PerformanceService - CalculateVehiclePerformance]: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al calcular el rendimiento de los vehículos: {0}", lObjException.Message));
            }
            return lLstPerformance;
        }
        #endregion
    }
}
