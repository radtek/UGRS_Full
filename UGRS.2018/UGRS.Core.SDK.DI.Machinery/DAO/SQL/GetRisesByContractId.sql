SELECT T1.U_IdRise AS IdRise, SUM(ISNULL(T2.U_HrFeet, 0)) AS HrFeet 
FROM [@UG_TBL_MQ_RIDC] T1
LEFT JOIN [@UG_MQ_RIHR] T2
	ON T1.U_IdRise = T2.U_IdRise
WHERE T1.U_DocEntry = '{ContractDocEntry}'
GROUP BY T1.U_IdRise