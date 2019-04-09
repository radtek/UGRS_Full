

select TL.Code, TL.Name, max(TL.U_Total) U_Total 
from [@UG_EX_LOCAL] TL
left join [@UG_EX_LOC_CONTRACT] TC on TL.Code = TC.U_LocalID
left join ORDR on ORDR.DocEntry = TC.U_DocEntryO and (ORDR.CANCELED = 'N' and ORDR.U_GLO_Status <> 'CN')
where 
U_Group = '{Group}' and ORDR.DocEntry is null and TC.Code is null 
group by TL.Code, TL.Name
