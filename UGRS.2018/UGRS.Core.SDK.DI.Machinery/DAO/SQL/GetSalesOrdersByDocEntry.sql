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
END AS Total
FROM ORDR T0 WHERE T0.DocEntry = '{DocEntry}'