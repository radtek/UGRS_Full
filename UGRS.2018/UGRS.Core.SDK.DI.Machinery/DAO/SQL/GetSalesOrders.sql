SELECT T0.DocEntry, T0.DocNum, T0.DocDate, T0.U_MQ_TipoCont, T0.CardName,
CASE
    WHEN CANCELED = 'Y'
		THEN 'Cancelled'
    ELSE T0.U_GLO_Status
END AS Status,
CASE 
	WHEN T0.DocCur = 'MXP' 
		THEN T0.DocTotal 
	ELSE T0.DocTotalFC
END AS Total,
ISNULL(T0.U_GLO_Municipality, '') AS 'MunicipalityId', T2.Name AS 'Municipality'
FROM ORDR T0
LEFT JOIN (SELECT A1.U_DocEntry FROM [@UG_TBL_MQ_RISE] A0 INNER JOIN [@UG_TBL_MQ_RIDC] A1 ON A0.Code= A1.U_IdRise 
            WHERE A0.U_DocStatus = 1 GROUP BY A1.U_DocEntry) T1 ON T0.DocEntry= T1.U_DocEntry
INNER JOIN [@UG_GLO_TOWN] T2
	ON T2.Code = T0.U_GLO_Municipality
 WHERE (T0. CANCELED = 'N' AND T0.U_GLO_Status = 'O')
AND ((T0.U_MQ_TipoCont = 1 AND ISNULL(t1.U_DocEntry, 0) = 0) OR (T0.U_MQ_TipoCont IN (2, 3)))