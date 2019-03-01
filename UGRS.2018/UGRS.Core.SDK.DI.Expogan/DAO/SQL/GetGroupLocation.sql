select TG.Code, TG.Name from [@UG_EX_GRLOC] TG
Left join [@UG_EX_LOCAL] TL on TG.Code = TL.U_Group
left join [@UG_EX_LOC_CONTRACT] TC on TL.Code = TC.U_LocalID
left join ORDR  on ORDR.DocEntry = TC.U_DocEntryO  and ORDR.CANCELED = 'N' and ORDR.U_GLO_Status <> 'CN'
where TC.Code is null and ORDR.DocEntry is null and TL.Code is not null
group by TG.Code, TG.Name