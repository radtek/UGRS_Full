declare @FolioOri as nvarchar(10);
set @FolioOri= '{RiseId}'

select 
@FolioOri U_IdRise,
CASE WHEN A1.Code IS NULL THEN '' ELSE A1.Code END Code,
T1.Equipo U_PrcCode, T1.FrgnName U_EcoNum, T1.EqType EqType,
CASE WHEN A1.U_DieselM IS NULL THEN 0 ELSE A1.U_DieselM END U_DieselM,
CASE WHEN A1.U_DieselT IS NULL THEN 0 ELSE A1.U_DieselT END U_DieselT,
CASE WHEN A1.U_Gas IS NULL THEN 0 ELSE A1.U_Gas END U_Gas,
CASE WHEN A1.U_F15W40 IS NULL THEN 0 ELSE A1.U_F15W40 END U_F15W40,
CASE WHEN A1.U_Hidraulic IS NULL THEN 0 ELSE A1.U_Hidraulic END U_Hidraulic,
CASE WHEN A1.U_SAE40 IS NULL THEN 0 ELSE A1.U_SAE40 END U_SAE40,
CASE WHEN A1.U_Transmition IS NULL THEN 0 ELSE A1.U_Transmition END U_Transmition,
CASE WHEN A1.U_Oils IS NULL THEN 0 ELSE A1.U_Oils END U_Oils,
CASE WHEN A1.U_KmHr IS NULL THEN 0 ELSE A1.U_KmHr END U_KmHr
from
(Select l0.Itemcode Equipo, l1.FrgnName, l2.U_TypeEQ EqType
from 
	( ---- Lista de Todos los AF usados en la Subida
	select a0.OcrCode2 Itemcode from (
		select T1.OcrCode2 from OPCH T0 with (nolock) inner join PCH1 T1 with (nolock) on t0.DocEntry= t1.DocEntry where T1.OcrCode2 is not null and T0.U_MQ_Rise = @FolioOri and t1.Quantity>0 and t0.CANCELED='N' union all
		select T1.OcrCode2 from OWTR T0 with (nolock) inner join WTR1 T1 with (nolock) on t0.DocEntry= t1.DocEntry where T1.OcrCode2 is not null and T0.U_MQ_Rise = @FolioOri and t1.Quantity>0 and t0.CANCELED='N' union all
		select T1.OcrCode2 from OWTQ T0 with (nolock) inner join WTQ1 T1 with (nolock) on t0.DocEntry= t1.DocEntry where T1.OcrCode2 is not null and T0.U_MQ_Rise = @FolioOri and t1.Quantity>0
    ) A0
    group by a0.OcrCode2
  ) L0  
    INNER JOIN oitm L1 with (nolock) on L1.ItemCode = L0.Itemcode 
    INNER JOIN [@UG_GLO_EQUIPO] L2 with (nolock) on L2.Code = L1.U_GLO_Equipo
) T1
	LEFT JOIN [@UG_MQ_RIFR] A1 with (nolock) ON A1.U_PrcCode = T1.Equipo AND A1.U_IdRise = @FolioOri