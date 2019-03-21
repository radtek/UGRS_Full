SELECT T1.Code, T1.U_DocEntry, T2.DocNum, T1.U_IdRise, T2.DocDate, T2.U_MQ_TipoCont, T2.CardName,
CASE
    WHEN T2.CANCELED = 'Y'
		THEN 'Cancelled'
    ELSE T2.U_GLO_Status
END AS Status,
CASE 
	WHEN T2.DocCur = 'MXP' 
		THEN T2.DocTotal 
	ELSE T2.DocTotalFC
END AS Total,
ISNULL(T2.U_GLO_Municipality, '') AS 'MunicipalityId', T3.Name AS 'Municipality'
FROM [@UG_TBL_MQ_RIDC] T1
INNER JOIN ORDR T2
	ON T1.U_DocEntry = T2.DocEntry
INNER JOIN [@UG_GLO_TOWN] T3
	ON T3.Code = T2.U_GLO_Municipality
WHERE T1.U_IdRise = '{RiseId}'